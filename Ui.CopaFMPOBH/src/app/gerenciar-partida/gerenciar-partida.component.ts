import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { Partida } from 'src/models/partida';
import { PartidaService } from 'src/services/partida.service';

@Component({
  selector: 'app-gerenciar-partida',
  templateUrl: './gerenciar-partida.component.html',
  styleUrls: ['./gerenciar-partida.component.css']
})
export class GerenciarPartidaComponent {
  public partidas: Partida[] = []; 
  public data: string;
  
  constructor(private partidaService : PartidaService, private datePipe: DatePipe){ 
    this.data = new Date().toISOString().split('T')[0];
   }

  ngOnInit(): void {
    this.buscarPartidas();
  }

  public buscarPartidas(){
    this.partidaService.obterPartidas(this.datePipe.transform(this.data, 'dd-MM')!.toString()).subscribe(partidas => {      
      this.partidas = partidas
    });
  }

  onDateChange(event: any) {
    this.data = event.target.value;
    this.buscarPartidas();
  }
}
