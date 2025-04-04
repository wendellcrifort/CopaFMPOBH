import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class SignalRService {
    private hubConnection!: signalR.HubConnection;
    private placarAtualizado = new Subject<{ idPartida: number, golsMandante: number, golsVisitante: number }>();

    constructor() { }

    public iniciarConexao() {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl('https://api.ipbfutsal.com.br/placarHub', {
                skipNegotiation: true, 
                transport: signalR.HttpTransportType.WebSockets 
            })
            .withAutomaticReconnect()
            .build();

        this.hubConnection
            .start()
            .then(() => console.log('Conectado ao SignalR!'))
            .catch(err => console.error('Erro ao conectar SignalR:', err));

        this.hubConnection.on('ReceberAtualizacaoPlacar', (idPartida: number, golsMandante: number, golsVisitante: number) => {
            this.placarAtualizado.next({ idPartida, golsMandante, golsVisitante });
        });
    }

    public getPlacarAtualizado() {
        return this.placarAtualizado.asObservable();
    }
}
