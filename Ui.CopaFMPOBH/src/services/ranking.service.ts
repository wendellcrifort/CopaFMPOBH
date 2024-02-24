import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Time } from 'src/models/time';

@Injectable({
  providedIn: 'root'
})
export class RankingService {

  private apiUrl = 'https://copafmpobh.com.br/api/Jogador';

  constructor(private http: HttpClient) { }

  getTimes(): Observable<Time[]> {
    return this.http.get<Time[]>(`${this.apiUrl}/BuscarTimes`);
  }

}
