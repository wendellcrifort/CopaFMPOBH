import { Component, EventEmitter, Input, Output } from '@angular/core';
import { EventoPartida } from 'src/models/eventoPartida';
import { Partida } from 'src/models/partida';

@Component({
  selector: 'app-historico-eventos',
  templateUrl: './historico-eventos.component.html',
  styleUrls: ['./historico-eventos.component.css']
})
export class HistoricoEventosComponent {
  @Input() eventos: EventoPartida[] | null = null;
  @Input() partida: Partida | null = null;
  @Output() deletarEvento = new EventEmitter<number>();

  public obterEscudo(idTime: number) {
    if (this.partida?.timeMandante.id == idTime)
      return this.partida?.timeMandante.escudo;
    else
      return this.partida?.timeVisitante.escudo;
  }

  public obterTime(idTime: number) {
    if (this.partida?.timeMandante.id == idTime)
      return this.partida?.timeMandante.nome;
    else
      return this.partida?.timeVisitante.nome;
  }

  public obterJogador(idJogador: number, idTime : number) : string {
    if (this.partida?.timeMandante.id == idTime)
      return this.partida?.timeMandante.jogadores.find(x=>x.id == idJogador)!.nome.split(' ')[0];
    else
      return this.partida?.timeVisitante.jogadores.find(x=>x.id == idJogador)!.nome.split(' ')[0]!;
  }

  public obterCamisa(idJogador: number, idTime : number) : number {
    if (this.partida?.timeMandante.id == idTime)
      return this.partida?.timeMandante.jogadores.find(x=>x.id == idJogador)!.numero!;
    else
      return this.partida?.timeVisitante.jogadores.find(x=>x.id == idJogador)!.numero!;
  }

  public deletar(id : number){
    this.deletarEvento.emit(id);
  }
}
