import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Jogador } from '../../models/jogador';
import { ClassificacaoService } from '../../services/classificacao.service';

@Component({
  selector: 'app-artilheiros',
  templateUrl: './artilheiros.component.html',
  styleUrls: ['./artilheiros.component.css']
})

export class ArtilheirosComponent {
    jogadores: Jogador[] | undefined;

    constructor(private route: ActivatedRoute,
        private classificacaoService: ClassificacaoService) { }

    ngOnInit(): void {
        this.route.params.subscribe(params => {
            this.classificacaoService.getArtilheiros()?.subscribe(x => this.jogadores = x);
        });
    }

    public nomeJogador(nome: string) {
        const [primeiroNome, ...partesRestantes] = nome.trim().split(" ");
        const ultimoNome = partesRestantes.pop() || '';
        const nomeCompleto = [primeiroNome, ultimoNome].join(" ");

        return nomeCompleto;
    }

}
