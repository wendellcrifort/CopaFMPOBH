import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Jogador } from '../../models/jogador';
import { ClassificacaoService } from '../../services/classificacao.service';

@Component({
  selector: 'app-goleiro',
  templateUrl: './goleiro.component.html',
  styleUrls: ['./goleiro.component.css']
})

export class GoleiroComponent {
  goleiros: Jogador[] | undefined;

  constructor(private route: ActivatedRoute,
    private classificacaoService: ClassificacaoService) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.classificacaoService.getMelhorGoleiro()?.subscribe(x => this.goleiros = x);
    });
  }
}

