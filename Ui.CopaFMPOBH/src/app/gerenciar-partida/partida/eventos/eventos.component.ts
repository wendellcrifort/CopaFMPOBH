import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Evento } from 'src/models/evento';
import { Jogador } from 'src/models/jogador';
import { Time } from 'src/models/time';
import { TipoEvento } from 'src/models/tipoEvento';
import { AlertService } from 'src/services/alert.service';
import { PartidaService } from 'src/services/partida.service';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent {
  @Input() jogador: Jogador | null = null;
  @Input() jogadoresTime: Jogador[] | null = null;
  @Input() adversario: Time | null = null;
  @Input() time: string | null = null;
  @Input() partida: number | null = null;
  @Output() partidaAtualizada = new EventEmitter<any>();

  public goleiro: Jogador | null = null;
  public mostrarModal = false;
  public mostrarMensagemGoleiro = false;
  public modalConfirmacao = false;
  public contra = false;

  private idJogador: number | null = null;
  private tipoEvento: TipoEvento | null = null;

  constructor(private partidaService: PartidaService, private alertService: AlertService) { }

  public gol() {
    if (!this.goleiro) {
      this.mostrarMensagemGoleiro = true;
      return;
    }

    this.tipoEvento = this.contra ? TipoEvento.GolContra : TipoEvento.GolMarcado;

    var mensagem = `gol ${this.contra ? 'contra' : '' } marcado`;
    this.evento(mensagem, this.goleiro?.id);
    this.fechaModal();
  }

  public amarelo(idJogador: number) {
    this.idJogador = idJogador;
    this.tipoEvento = TipoEvento.CartaoAmarelo;
    this.modalConfirmacao = true;
  }

  public vermelho(idJogador: number) {
    this.idJogador = idJogador;
    this.modalConfirmacao = true;
    this.tipoEvento = TipoEvento.CartaoVermelho;
  }

  public abrirModalGol(idJogador: number, contra = false) {
    this.idJogador = idJogador;
    this.contra = contra;
    this.goleiro = null;
    this.mostrarModal = true;
  }

  public confirmaCartao() {
    this.evento(`cartão ${this.tipoEvento == TipoEvento.CartaoAmarelo ? 'amarelo' : 'vermelho' } aplicado`);
    this.fechaModal();
  }

  public fechaModal() {
    this.goleiro = null;
    this.idJogador = null;
    this.mostrarMensagemGoleiro = false;
    this.mostrarModal = false;
    this.modalConfirmacao = false;
  }

  public definirGoleiro(goleiro: Jogador) {
    this.mostrarMensagemGoleiro = false;
    this.goleiro = goleiro;
  }

  public nomeJogador(nome: string){
    const [primeiroNome, ...partesRestantes] = nome.trim().split(" ");
    const ultimoNome = partesRestantes.pop() || '';
    const nomeCompleto = [primeiroNome, ultimoNome].join(" ");

    return nomeCompleto;
  }

  private evento(mensagem : string, idGoleiro: number | null = null) {
    this.partidaService
      .registrarEvento(new Evento(this.partida!, this.idJogador!, this.tipoEvento!, idGoleiro))
      .subscribe(x => {
        this.partidaAtualizada.emit(x);
        this.alertService.showAlertSuccess(mensagem);
      });
  }
}
