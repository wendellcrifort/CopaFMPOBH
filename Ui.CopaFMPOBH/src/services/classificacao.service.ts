import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { classificacao } from '../models/classificacao';
import { Jogador } from '../models/jogador';

@Injectable({
  providedIn: 'root'
})
export class ClassificacaoService {

  //private apiUrl = 'http://localhost:5097/Ranking';
  private apiUrl = 'http://api.copafmpobh.com.br/Ranking';

  constructor(private http: HttpClient) { }

  getClassificacao(): Observable<classificacao> {
    return this.http.get<classificacao>(`${this.apiUrl}/BuscarClassificacao`);
  }

  getMelhorGoleiro(): Observable<Jogador[]> {
    return this.http.get<Jogador[]>(`${this.apiUrl}/BuscarMelhoresGoleiros`);
  }
}
