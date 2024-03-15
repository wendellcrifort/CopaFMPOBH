import { Component, Input } from '@angular/core';
import { Jogador } from 'src/models/jogador';
import { Partida } from 'src/models/partida';
import { Time } from 'src/models/time';

@Component({
  selector: 'app-item-partida',
  templateUrl: './item-partida.component.html',
  styleUrls: ['./item-partida.component.css']
})
export class ItemPartidaComponent {
  @Input() partida: Partida | null = null;
  
  public partidaSelecionada: Partida | null = null;

  public selecionarPartida(partida: Partida) {
    this.partidaSelecionada = partida == this.partidaSelecionada ? null : partida;
  }

  eventos(partida: Partida, time: Time, adversario : Time): EventosJogador[] {
    const eventosJogadores: EventosJogador[] = [];
  
    time.jogadores.forEach(jogador => {
      const eventosAgrupados: { [descricaoEvento: string]: number } = {};
  
      partida.eventos.forEach(evento => {
        if (evento.descricaoEvento != 'GolContra' && evento.idJogador === jogador.id) {
          if (eventosAgrupados[evento.descricaoEvento]) {
            eventosAgrupados[evento.descricaoEvento]++;
          } else {
            eventosAgrupados[evento.descricaoEvento] = 1;
          }
        }
      });
  
      if (Object.keys(eventosAgrupados).length > 0) {
        eventosJogadores.push(new Implementacao(jogador.nome, jogador.numero, eventosAgrupados));
      }
    });

    adversario.jogadores.forEach(jogador => {
      const eventosAgrupados: { [descricaoEvento: string]: number } = {};
  
      partida.eventos.forEach(evento => {
        if (evento.descricaoEvento == 'GolContra' && evento.idJogador === jogador.id) {
          if (eventosAgrupados[evento.descricaoEvento]) {
            eventosAgrupados[evento.descricaoEvento]++;
          } else {
            eventosAgrupados[evento.descricaoEvento] = 1;
          }
        }
      });
  
      if (Object.keys(eventosAgrupados).length > 0) {
        eventosJogadores.push(new Implementacao(jogador.nome, jogador.numero, eventosAgrupados));
      }
    });
  
    return eventosJogadores;
  }

  public nomeJogador(nome : string){
    const [primeiroNome, ...partesRestantes] = nome.trim().split(" ");
    const ultimoNome = partesRestantes.pop() || '';
    const nomeCompleto = [primeiroNome, ultimoNome].join(" ");

    return nomeCompleto;
  }
  
}

interface EventosJogador {
  nome: string;
  numero: number;
  eventosAgrupados: { [descricaoEvento: string]: number };
}

class Implementacao implements EventosJogador {
  nome: string;
  numero: number;
  eventosAgrupados: { [descricaoEvento: string]: number };

  constructor(nome: string, numero: number, eventosAgrupados: { [descricaoEvento: string]: number }) {
    this.nome = nome;
    this.numero = numero;
    this.eventosAgrupados = eventosAgrupados;
  }
}

