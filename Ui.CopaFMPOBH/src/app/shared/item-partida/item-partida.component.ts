import { Component, Input } from '@angular/core';
import { Jogador } from 'src/models/jogador';
import { Partida } from 'src/models/partida';
import { Sumula } from 'src/models/sumula';
import { Time } from 'src/models/time';
import { AlertService } from 'src/services/alert.service';
import { PartidaService } from 'src/services/partida.service';
import { SignalRService } from '../../../services/signalr.service ';


@Component({
  selector: 'app-item-partida',
  templateUrl: './item-partida.component.html',
  styleUrls: ['./item-partida.component.css']
})
export class ItemPartidaComponent {
  @Input() partida: Partida | null = null;
  
  public partidaSelecionada: Partida | null = null;

    constructor(private partidaService: PartidaService, private alertService: AlertService, private slRService: SignalRService){}

    ngOnInit() {
        this.slRService.getPlacarAtualizado().subscribe(data => {
            if (this.partida && this.partida.idPartida === data.idPartida) {
                this.partida.golsTimeMandante = data.golsMandante;
                this.partida.golsTimeVisitante = data.golsVisitante;
            }
        });
    }

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

  public buscarSumula(){
    this.partidaService.buscarSumula(this.partidaSelecionada!.idPartida)
    .subscribe((sumula: Sumula) => {
      if(!sumula || !sumula.arquivoSumula){
        this.alertService.showAlertDanger("súmula não encontrada");
        return;
      }
      const byteCharacters = atob(sumula.arquivoSumula); // Decodifica a string base64
      const byteNumbers = new Array(byteCharacters.length);
      for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
      }
      const byteArray = new Uint8Array(byteNumbers);
      const blob = new Blob([byteArray], { type: 'image/png' }); // Substitua 'image/png' pelo tipo de imagem retornado pela sua API
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    });
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

