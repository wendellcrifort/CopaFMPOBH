import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, of, tap } from 'rxjs';
import { Evento } from 'src/models/evento';
import { Partida } from 'src/models/partida';
import { PartidasHome } from '../models/partidaHome';

@Injectable({
  providedIn: 'root'
})
export class PartidaService {

  // private apiUrl = 'http://api.copafmpobh.com.br/Partida';
  private apiUrl = 'http://localhost:5097/Partida';
  
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };
  
  constructor(private http: HttpClient) { }

  obterPartidas(): Observable<Partida[]> {
    return this.http.get<Partida[]>(`${this.apiUrl}/BuscarPartidas`);
  }

  obterPartidasHome(): Observable<PartidasHome> {    
    return this.http.get<PartidasHome>(`${this.apiUrl}/BuscarPartidasHome`);
  }

  obterPartidaPorId(id: number): Observable<Partida> | undefined {
    return this.http.get<Partida>(`${this.apiUrl}/BuscarPartidaEmAndamento/${id}`);
  }

  registrarEvento(evento: Evento): void {
    var url = `${this.apiUrl}/RegistrarEventoPartida?
    idPartida=${evento.idPartida}
    &idJogador=${evento.idJogador}
    &evento=${evento.evento}`;
    
    if(evento.idGoleiro) url += `&idGoleiro=${evento.idGoleiro}`;

    this.http.post<any>(url, this.httpOptions).subscribe();
  }
}
