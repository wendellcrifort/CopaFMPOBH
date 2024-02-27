import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Jogador } from 'src/models/jogador';
import { JogadorService } from 'src/services/jogador.service';

@Component({
  selector: 'app-jogadores',
  templateUrl: './jogadores.component.html',
  styleUrls: ['./jogadores.component.css']
})

export class JogadorComponent {
  jogadores: Jogador[] = [];

  constructor(private route: ActivatedRoute,
              private jogadorService: JogadorService) { }

  ngOnInit(): void {
    const idTime = Number(this.route.snapshot.paramMap.get('id'));
    console.log(idTime)
    this.jogadorService.getJogadores(idTime).subscribe(data => {      
      this.jogadores = data
    });
  }
}

