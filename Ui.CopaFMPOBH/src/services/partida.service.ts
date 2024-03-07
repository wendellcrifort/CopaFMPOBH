import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, of, tap } from 'rxjs';
import { Evento } from 'src/models/evento';
import { Partida } from 'src/models/partida';
import { PartidasHome } from '../models/partidaHome';
import { EventoPartida } from 'src/models/eventoPartida';

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
  
  obterEventosPartida(id: number): Observable<EventoPartida[]> | undefined {
    return this.http.get<EventoPartida[]>(`${this.apiUrl}/BuscarEventosPartidas/${id}`);
  }

  removerEventoPartida(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/RemoverEventoPartida/${id}`);
  }

  registrarEvento(evento: Evento): Observable<any> {
    var url = `${this.apiUrl}/RegistrarEventoPartida?idPartida=${evento.idPartida}&idJogador=${evento.idJogador}&evento=${evento.evento}`;
  
    if(evento.idGoleiro) url += `&idGoleiro=${evento.idGoleiro}`;

    return this.http.post<any>(url, this.httpOptions);
  }
}
