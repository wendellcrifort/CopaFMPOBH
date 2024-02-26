import { Component } from '@angular/core';
import { Jogador } from 'src/models/jogador';
import { JogadorService } from 'src/services/jogador.service';

@Component({
  selector: 'app-jogadores',
  templateUrl: './jogadores.component.html',
  styleUrls: ['./jogadores.component.css']
})

export class JogadorComponent {
  jogadores: Jogador[] = [];

  constructor(private jogadorService: JogadorService) { }

  ngOnInit(): void {
    this.jogadorService.getJogadores(1).subscribe(data => {
      console.log(data)
      this.jogadores = data
    });
  }
}

