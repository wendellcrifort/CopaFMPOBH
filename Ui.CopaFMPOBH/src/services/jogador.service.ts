import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Time } from 'src/models/time';

@Injectable({
  providedIn: 'root'
})
export class JogadorService {
  
  private apiUrl = 'http://localhost:5097/Jogador';
  //private apiUrl = 'https://copafmpobh.com.br/api/Jogador';

  constructor(private http: HttpClient) { }

  getTimes(): Observable<Time[]> {
    return this.http.get<Time[]>(`${this.apiUrl}/BuscarTimes`);
  }

}
