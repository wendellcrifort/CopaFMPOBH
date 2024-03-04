import { Jogador } from "./jogador";

export interface Time {
  id: number;
  nome: string;
  grupo: string;
  pontos: number;
  vitorias: number;
  empates: number;
  derrotas: number;
  golsFeitos: number;
  golsSofridos: number;
  saldoGols: number;
  cartoesAmarelos: number;
  cartoesVermelhos: number;
  escudo: string;
  jogadores: Jogador[];
}