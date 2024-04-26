import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Jogador } from 'src/models/jogador';
import { ClassificacaoService } from 'src/services/classificacao.service';

@Component({
  selector: 'app-melhor-jogador',
  templateUrl: './melhor-jogador.component.html',
  styleUrls: ['./melhor-jogador.component.css']
})
export class MelhorJogadorComponent {
  goleiros: Jogador[] | undefined;

  constructor(private route: ActivatedRoute,
    private classificacaoService: ClassificacaoService) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.classificacaoService.getMelhorJogador()!.subscribe(x => this.goleiros = x);
    });
  }

  public nomeJogador(nome: string){
    const [primeiroNome, ...partesRestantes] = nome.trim().split(" ");
    const ultimoNome = partesRestantes.pop() || '';
    const nomeCompleto = [primeiroNome, ultimoNome].join(" ");

    return nomeCompleto;
  }
}
