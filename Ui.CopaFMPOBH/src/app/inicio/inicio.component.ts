import { Component, OnInit } from '@angular/core';
import { PartidasHome } from '../../models/partidaHome';
import { ActivatedRoute } from '@angular/router';
import { PartidaService } from '../../services/partida.service';
import { Partida } from 'src/models/partida';
import { Time } from 'src/models/time';

@Component({
  selector: 'app-inicio',
  templateUrl: './inicio.component.html',
  styleUrls: ['./inicio.component.css']
})
export class InicioComponent {

  partidas: PartidasHome | undefined;
  public partida: Partida | null = null;

  constructor(private route: ActivatedRoute,
    private partidaService: PartidaService) { }

  ngOnInit(): void {
    //this.resize();
    this.route.params.subscribe(params => {
      this.partidaService.obterPartidasHome()?.subscribe(x => { this.partidas = x });
    });
  }

  public selecionarPartida(partida: Partida) {
    this.partida = partida == this.partida ? null : partida;
  }

  public eventos(partida: Partida, time: Time) {
    return partida.eventos.filter(evento => evento.idTime === time.id)
      .map(evento => ({
        nome: time.jogadores.find(x => x.id == evento.idJogador)!.nome,
        numero: time.jogadores.find(x => x.id == evento.idJogador)!.numero,
        descricaoEvento: evento.descricaoEvento
      }));
  }

  resize() {
    const mainSlider = document.getElementById('main-slider');
    const sliderItem = document.getElementById('slider-item');
    const windowHeight = window.innerHeight;
    const elementTop = sliderItem!.getBoundingClientRect().top;
    const itemHeight = windowHeight - elementTop;

    if (sliderItem) {
      sliderItem.style.height = itemHeight + 'px';
    }
    if (mainSlider) {
      mainSlider.style.height = itemHeight + 'px';
    }
  }
}
