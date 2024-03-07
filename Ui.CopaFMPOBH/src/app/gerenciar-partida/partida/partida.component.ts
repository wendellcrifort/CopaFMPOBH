import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EventoPartida } from 'src/models/eventoPartida';
import { Partida } from 'src/models/partida';
import { PartidaService } from 'src/services/partida.service';

@Component({
  selector: 'app-partida',
  templateUrl: './partida.component.html',
  styleUrls: ['./partida.component.css']
})
export class PartidaComponent implements OnInit {

  id: number | null = null;
  partida: Partida | null = null;
  eventos: EventoPartida[] | null = null;

  constructor(private route: ActivatedRoute, private partidaService: PartidaService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.id = +params['id'];
      this.buscarPartida();
      this.buscarEventos();
    });
  }

  public buscarPartida() {
    this.partidaService.obterPartidaPorId(this.id!)?.subscribe(x => this.partida = x);
  }

  public atualizaEventos(evento : any) {
    this.partida!.golsTimeMandante = evento.golsMandante;
    this.partida!.golsTimeVisitante = evento.golsVisitante;
    this.buscarEventos();
  }
  
  public buscarEventos() {
    this.partidaService.obterEventosPartida(this.id!)?.subscribe(x => this.eventos = x);
  }

  public deletarEvento(idEvento: number) {
    this.partidaService.removerEventoPartida(idEvento).subscribe(
      x => {
        const index = this.eventos!.findIndex(item => item.id === idEvento);
        if (index !== -1) this.eventos!.splice(index, 1);
        this.partida!.golsTimeMandante = x.golsMandante;
        this.partida!.golsTimeVisitante = x.golsVisitante;
      }
    );

  }
}
