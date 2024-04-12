import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { error } from 'jquery';
import { EventoPartida } from 'src/models/eventoPartida';
import { Jogador } from 'src/models/jogador';
import { Partida } from 'src/models/partida';
import { Sumula } from 'src/models/sumula';
import { AlertService } from 'src/services/alert.service';
import { PartidaService } from 'src/services/partida.service';

@Component({
  selector: 'app-partida',
  templateUrl: './partida.component.html',
  styleUrls: ['./partida.component.css']
})
export class PartidaComponent implements OnInit {

  id: number | null = null;
  partida: Partida | null = null;
  eventos: EventoPartida[] | null = null;

  public modalFinalizarPartida = false;
  public jogadorSelecionado: Jogador | null = null;
  public nomeArquivoSumula: string = '';
  public file: File | undefined;

  constructor(private route: ActivatedRoute, private partidaService: PartidaService, private router: Router, private alertService: AlertService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.id = +params['id'];
      this.buscarPartida();
      this.buscarEventos();
    });
  }

  public buscarPartida() {
    this.partidaService.obterPartidaPorId(this.id!)
      ?.subscribe(partida => {
        this.partida = partida
      });
  }

  public atualizaEventos(evento: any) {
    this.partida!.golsTimeMandante = evento.golsMandante;
    this.partida!.golsTimeVisitante = evento.golsVisitante;
    this.buscarEventos();
  }

  public buscarEventos() {
    this.partidaService.obterEventosPartida(this.id!)?.subscribe(x => this.eventos = x);
  }

  public deletarEvento(idEvento: number) {
    this.partidaService.removerEventoPartida(idEvento).subscribe(
      x => {
        const index = this.eventos!.findIndex(item => item.id === idEvento);
        if (index !== -1) this.eventos!.splice(index, 1);
        this.partida!.golsTimeMandante = x.golsMandante;
        this.partida!.golsTimeVisitante = x.golsVisitante;
        this.alertService.showAlertSuccess("evento deletado");
      }
    );
  }

  public abrirModalFinalizarPartida() {
    var melhorJogador = this.eventos?.filter(x => x.descricaoEvento == "MelhorJogador").length;
    var melhorGoleiro = this.eventos?.filter(x => x.descricaoEvento == "MelhorGoleiro").length;
    
    if (melhorJogador == 0) {
      this.alertService.showAlertDanger("melhor JOGADOR não inserido");
      return;
    }
    if (melhorGoleiro == 0) {
      this.alertService.showAlertDanger("melhor GOLEIRO não inserido");
      return;
    }
    if (melhorJogador! > 1) {
      this.alertService.showAlertDanger("mais de um JOGADOR selecionado para melhor da partida");
      return;
    }
    if (melhorGoleiro! > 1) {
      this.alertService.showAlertDanger("mais de um GOLEIRO selecionado para melhor da partida");
      return;
    }

    this.modalFinalizarPartida = true;
  }

  public fecharModalFinalizar() {
    this.modalFinalizarPartida = false;
  }

  public finalizarPartida() {
    this.fecharModalFinalizar();
    this.partidaService.finalizarPartida(this.id!).subscribe(() => {
      this.irParaGerenciarPartidas()
      this.alertService.showAlertSuccess("partida finalizada");
    });
  }

  public selecionarJogador(jogador: Jogador) {
    if (this.jogadorSelecionado == jogador)
      this.jogadorSelecionado = null;
    else
      this.jogadorSelecionado = jogador;
  }

  public iniciarPartida() {
    this.partidaService.inciarPartida(this.id!).subscribe(() => {
      this.partida!.emAndamento = true
      this.alertService.showAlertSuccess("partida iniciada");
    });
  }

  private irParaGerenciarPartidas() {
    this.router.navigate(['/gerenciarPartida'])
  }

  public escolherArquivo() {
    const fileInput = document.getElementById('fileInput') as HTMLInputElement;
    fileInput.click();
  }

  public onFileSelected(event: any) {
    this.file = event.target.files[0];
    this.nomeArquivoSumula = this.file?.name ?? '';
  }

  public salvarArquivo() {
    if (!this.file) {
      console.error('Nenhum arquivo selecionado.');
      return;
    }

    if (this.file) {
      this.readFile(this.file).then((byteArray: Uint8Array) => {
        const byteString = byteArray.reduce((data, byte) => {
          return data + String.fromCharCode(byte);
        }, '');
        const sumulaString = btoa(byteString);
        this.salvarSumula(sumulaString);
      });
    }
  }

  private salvarSumula(sumulaString: string) {
    var sumula: Sumula = {
      idPartida: this.partida!.idPartida,
      arquivoSumula: sumulaString
    }

    this.partidaService.salvarSumula(sumula)
      .subscribe({
        next: response => console.log('Arquivo enviado com sucesso:', response),
        error: err => console.error('Observable emitted an error: ' + err)
      }
      );
  }

  private readFile(file: File): Promise<Uint8Array> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = () => {
        const arrayBuffer = reader.result as ArrayBuffer;
        const byteArray = new Uint8Array(arrayBuffer);
        resolve(byteArray);
      };
      reader.onerror = () => {
        reject(reader.error);
      };
      reader.readAsArrayBuffer(file);
    });
  }
}
