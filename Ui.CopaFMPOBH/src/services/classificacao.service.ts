import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { classificacao } from '../models/classificacao';
import { Jogador } from '../models/jogador';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root'
})
export class ClassificacaoService {

    private apiUrl = `${environment.apiUrl}Ranking`;    

    constructor(private http: HttpClient) { }

    getClassificacao(): Observable<classificacao> {
        return this.http.get<classificacao>(`${this.apiUrl}/BuscarClassificacao`);
    }

    getMelhorGoleiro(): Observable<Jogador[]> {
        return this.http.get<Jogador[]>(`${this.apiUrl}/BuscarMelhoresGoleiros`);
    }

    getArtilheiros(): Observable<Jogador[]> {
        return this.http.get<Jogador[]>(`${this.apiUrl}/BuscarArtilheiros`);
    }

    getMelhorJogador(): Observable<Jogador[]> {
        return this.http.get<Jogador[]>(`${this.apiUrl}/BuscarMelhoresJogadores`);
    }
}
