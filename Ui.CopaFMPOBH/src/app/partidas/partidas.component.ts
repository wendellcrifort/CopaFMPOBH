import { Component } from '@angular/core';
import { Partida } from 'src/models/partida';
import { PartidaService } from 'src/services/partida.service';

@Component({
  selector: 'app-partidas',
  templateUrl: './partidas.component.html',
  styleUrls: ['./partidas.component.css']
})
export class PartidasComponent {
  public partidas: Partida[] = [];
  public partidasPorRodada: { [key: number]: Partida[] } = {};
  public rodadaSelecionada = 1;
  public primeiraRodada = 1;
  public ultimaRodada = 1;

  constructor(private partidaService: PartidaService) { }

  ngOnInit(): void {
    this.buscarPartidas();
  }

  public buscarPartidas() {
    this.partidaService.obterPartidas().subscribe(partidas => {
      this.agruparPartidasPorRodada(partidas);
    });
  }

  private agruparPartidasPorRodada(partidas: Partida[]) {
    partidas.forEach(partida => {
      const rodada = partida.rodada;
      if (rodada <= this.primeiraRodada) this.primeiraRodada = rodada;
      if (rodada >= this.ultimaRodada) this.ultimaRodada = rodada;

      if (!this.partidasPorRodada[rodada]) {
        this.partidasPorRodada[rodada] = [];
      }
      this.partidasPorRodada[rodada].push(partida);
    });
  }

  public keys(obj: any): number[] {
    return Object.keys(obj).map(Number);
  }

  public selecionarRodada(avancou: boolean) {
    if (avancou)
      this.rodadaSelecionada++;
    else
      this.rodadaSelecionada--;
  }

}
