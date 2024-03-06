import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ClassificacaoService } from '../../services/classificacao.service';
import { classificacao } from '../../models/classificacao';

@Component({
  selector: 'app-classificacao',
  templateUrl: './classificacao.component.html',
  styleUrls: ['./classificacao.component.css']
}) 

export class ClassificacaoComponent {
  classificacao: classificacao | undefined;

  constructor(private route: ActivatedRoute,
    private classificacaoService: ClassificacaoService) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {      
      this.classificacaoService.getClassificacao()?.subscribe(x => this.classificacao = x);
    });
  }
}

