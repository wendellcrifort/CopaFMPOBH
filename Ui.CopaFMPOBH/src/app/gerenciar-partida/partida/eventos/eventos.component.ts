import { Component, Input } from '@angular/core';
import { Evento } from 'src/models/evento';
import { Jogador } from 'src/models/jogador';
import { Time } from 'src/models/time';
import { TipoEvento } from 'src/models/tipoEvento';
import { PartidaService } from 'src/services/partida.service';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent {
  @Input() jogador: Jogador | null = null;
  @Input() adversario: Time | null = null;
  @Input() time: string | null = null;
  @Input() partida: number | null = null;

  public goleiro: Jogador | null = null;
  public mostrarModal = false;
  public mostrarMensagemGoleiro = false;

  private idJogador = 0;
  private contra = false;

  constructor(private partidaService: PartidaService) { }

  public gol() {
    if (!this.goleiro) {
      this.mostrarMensagemGoleiro = true;
      return;
    }

    var evento = this.contra ? TipoEvento.GolContra : TipoEvento.GolMarcado;
    this.partidaService.registrarEvento(this.evento(evento, this.goleiro?.id));
    this.fechaModal();
  }

  public amarelo(idJogador: number) {
    this.idJogador = idJogador;
    this.partidaService.registrarEvento(this.evento(TipoEvento.CartaoAmarelo));
  }

  public vermelho(idJogador: number) {
    this.idJogador = idJogador;
    this.partidaService.registrarEvento(this.evento(TipoEvento.CartaoVermelho));
  }

  public abrirModalGol(idJogador: number, contra = false) {
    this.idJogador = idJogador;
    this.contra = contra;
    this.goleiro = null;
    this.mostrarModal = true;
  }

  public fechaModal() {
    this.goleiro = null;
    this.mostrarMensagemGoleiro = false;
    this.mostrarModal = false;
  }

  public definirGoleiro(goleiro: Jogador) {
    this.mostrarMensagemGoleiro = false;
    this.goleiro = goleiro;
  }

  private evento(evento: number, idGoleiro: number | null = null): Evento {
    return new Evento(this.partida!, this.idJogador, evento, idGoleiro);
  }
}
