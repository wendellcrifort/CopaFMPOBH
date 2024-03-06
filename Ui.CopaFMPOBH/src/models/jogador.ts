import { Time } from "./time";

export interface Jogador {
  id: number;
  idTime: number;
  nome: string;
  numero: number;
  golsMarcados: number;
  golsSofridos: number;
  ehGoleiro: boolean;
  cartoesAmarelos: number;
  cartoesVermelhos: number;
  suspenso: boolean;
  jogos: number;
  escudo: string;
}
