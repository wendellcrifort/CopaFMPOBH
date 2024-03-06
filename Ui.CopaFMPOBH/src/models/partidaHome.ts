import { Partida } from "./partida";

export interface PartidasHome {
  partidaAoVivo: Partida;
  proximaPartida: Partida;
  partidasEncerradas: Partida[];
}
