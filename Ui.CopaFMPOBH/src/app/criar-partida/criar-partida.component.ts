import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { JogadorService } from 'src/services/jogador.service';
import { Time } from '../../models/time';
import { PartidaService } from '../../services/partida.service';

@Component({
  selector: 'app-criar-partida',
  templateUrl: './criar-partida.component.html',
  styleUrls: ['./criar-partida.component.css']
})

export class CriarPartidaComponent {
  times: Time[] | undefined;

  constructor(private route: ActivatedRoute,
    private jogadorService: JogadorService,
    private partidaService: PartidaService) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {      
      this.jogadorService.getTimes()?.subscribe(x => this.times = x);
    });
  }

  postPartida() {

  }
}

