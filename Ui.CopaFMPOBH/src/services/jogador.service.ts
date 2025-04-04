import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Time } from 'src/models/time';
import { JogadoresTime } from '../models/JogadoresTime';
import { JogadorForm } from 'src/models/jogadorForm';
import { environment } from 'src/environments/environment';

@Injectable({
	providedIn: 'root'
})
export class JogadorService {

	private apiUrl = `${environment.apiUrl}Jogador`;

	constructor(private http: HttpClient) { }

	getTimes(): Observable<Time[]> {
		return this.http.get<Time[]>(`${this.apiUrl}/BuscarTimes`);
	}

	getJogadores(idTime: number): Observable<JogadoresTime> {
		return this.http.get<JogadoresTime>(`${this.apiUrl}/BuscarJogadores/${idTime}`);
	}

	createJogadores(jogadores: JogadorForm[]): Observable<boolean> {
		return this.http.post<boolean>(`${this.apiUrl}/Jogador`, jogadores);
	}
}
