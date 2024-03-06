import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, of, tap } from 'rxjs';
import { Partida } from 'src/models/partida';

@Injectable({
  providedIn: 'root'
})
export class PartidaService {

  // private apiUrl = 'http://api.copafmpobh.com.br/Partida';
  private apiUrl = 'http://localhost:5097/Partida';

  constructor(private http: HttpClient) { }

  obterPartidas(): Observable<Partida[]> {
    return this.http.get<Partida[]>(`${this.apiUrl}/BuscarPartidas`);
  }

  obterPartidaPorId(id: number): Observable<Partida> | undefined {
    return this.http.get<Partida>(`${this.apiUrl}/BuscarPartidaEmAndamento/${id}`);
  }

}
