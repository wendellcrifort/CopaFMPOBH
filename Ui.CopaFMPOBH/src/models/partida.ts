import { EventoPartida } from "./eventoPartida";
import { Time } from "./time";

export interface Partida {
  idPartida: number;
  dataPartida: string;
  horaPartida: string;
  timeMandante: Time;
  timeVisitante: Time;
  rodada: number;
  golsTimeMandante: number;
  golsTimeVisitante: number;
  emAndamento: boolean,
  partidaFinalizada: boolean
  eventos: EventoPartida[]
}
