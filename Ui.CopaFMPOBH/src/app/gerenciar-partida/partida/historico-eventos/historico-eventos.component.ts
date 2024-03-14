import { Component, EventEmitter, Input, Output } from '@angular/core';
import { EventoPartida } from 'src/models/eventoPartida';
import { Jogador } from 'src/models/jogador';
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

  public idEvento = 0;
  public modalConfirmarDelecao = false;

  public obterEscudo(idTime: number) {
    if (this.partida?.timeMandante.id == idTime)
      return this.partida?.timeMandante.escudo;
    else
      return this.partida?.timeVisitante.escudo;
  }

  public obterJogador(idJogador: number, idTime : number) : string {
    if (this.partida?.timeMandante.id == idTime)
      return this.nomeJogador(this.partida!.timeMandante.jogadores.find(x=>x.id == idJogador)!.nome);
    else
      return this.nomeJogador(this.partida!.timeVisitante.jogadores.find(x=>x.id == idJogador)!.nome!);
  }

  public obterNumero(idJogador: number, idTime : number) : number {
    if (this.partida?.timeMandante.id == idTime)
      return this.partida!.timeMandante.jogadores.find(x=>x.id == idJogador)!.numero;
    else
      return this.partida!.timeVisitante.jogadores.find(x=>x.id == idJogador)!.numero!;
  }

  public nomeJogador(nome: string){
    const [primeiroNome, ...partesRestantes] = nome.trim().split(" ");
    const ultimoNome = partesRestantes.pop() || '';
    const nomeCompleto = [primeiroNome, ultimoNome].join(" ");

    return nomeCompleto;
  }

  public abrirModalDeletar(id : number){
    this.idEvento = id;
    this.modalConfirmarDelecao = true;
  }

  public fecharModal(){
    this.modalConfirmarDelecao = false;
  }

  public deletar(){
    this.fecharModal();
    this.deletarEvento.emit(this.idEvento);
  }
}
