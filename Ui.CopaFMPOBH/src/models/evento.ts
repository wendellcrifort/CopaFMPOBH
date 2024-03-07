
export class Evento {
    idPartida: number;
    idJogador: number;
    evento: number;
    idGoleiro: number | null;
  
    constructor(idPartida : number, idJogador: number, evento : number, idGoleiro : number | null) {
      this.idPartida = idPartida;
      this.idJogador = idJogador;
      this.evento = evento;
      this.idGoleiro = idGoleiro
    }
  }