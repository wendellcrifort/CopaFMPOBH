import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { JogadorService } from 'src/services/jogador.service';
import { JogadoresTime } from '../../models/JogadoresTime';
import { Jogador } from 'src/models/jogador';

@Component({
  selector: 'app-jogadores',
  templateUrl: './jogadores.component.html',
  styleUrls: ['./jogadores.component.css']
})

export class JogadorComponent {
  jogadoresTime: JogadoresTime | undefined;

  constructor(private route: ActivatedRoute,
              private jogadorService: JogadorService) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const timeId = +params['id'];
      this.jogadorService.getJogadores(timeId)?.subscribe(x => this.jogadoresTime = x);
    });
  }

  public nomeJogador(nome: string){
    const [primeiroNome, ...partesRestantes] = nome.trim().split(" ");
    const ultimoNome = partesRestantes.pop() || '';
    const nomeCompleto = [primeiroNome, ultimoNome].join(" ");

    return nomeCompleto;
  }
}

