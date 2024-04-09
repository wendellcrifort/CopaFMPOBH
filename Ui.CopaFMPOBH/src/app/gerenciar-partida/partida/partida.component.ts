import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { error } from 'jquery';
import { EventoPartida } from 'src/models/eventoPartida';
import { Jogador } from 'src/models/jogador';
import { Partida } from 'src/models/partida';
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
  public selectedFile: File | undefined;

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

  public iniciarPartida(){
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
    this.selectedFile = event.target.files[0];
    this.nomeArquivoSumula = this.selectedFile?.name ?? '';
  }

  public salvarArquivo() {
    if (!this.selectedFile) {
      console.error('Nenhum arquivo selecionado.');
      return;
    }

    const formData = new FormData();
    formData.append('file', this.selectedFile);
    this.partidaService.salvarSumula(this.partida!.idPartida, formData)
      .subscribe({
        next: response => console.log('Arquivo enviado com sucesso:', response),
        error: err => console.error('Observable emitted an error: ' + err)
        }
      );
  }
}
