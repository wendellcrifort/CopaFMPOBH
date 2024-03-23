import { Partida } from "./partida";

export interface PartidasHome {
  partidasAoVivo: Partida[];
  proximaPartida: Partida;
  partidasEncerradas: Partida[];
}
