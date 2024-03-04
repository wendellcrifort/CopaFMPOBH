import { Component } from '@angular/core';
import { Partida } from 'src/models/partida';
import { PartidaService } from 'src/services/partida.service';

@Component({
  selector: 'app-gerenciar-partida',
  templateUrl: './gerenciar-partida.component.html',
  styleUrls: ['./gerenciar-partida.component.css']
})
export class GerenciarPartidaComponent {
  partidas: Partida[] = []; 

  constructor(private partidaService : PartidaService){  }

  ngOnInit(): void {
    this.partidaService.obterPartidas().subscribe(data => {      
      this.partidas = data
    });
  }
}
