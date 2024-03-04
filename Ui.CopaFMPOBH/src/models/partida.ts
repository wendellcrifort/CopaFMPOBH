import { Time } from "./time";

export interface Partida {
    idPartida: number;
    dataPartida: string;
    horaPartida: string;
    timeMandante: Time;
    timeVisitante: Time;
    rodada: number;
  }