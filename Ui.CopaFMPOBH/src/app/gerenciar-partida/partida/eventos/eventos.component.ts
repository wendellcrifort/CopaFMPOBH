import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Jogador } from 'src/models/jogador';
import { Time } from 'src/models/time';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent {
  @Input() jogador: Jogador | null = null;
  @Input() adversario: Time | null = null;
  @Input() time: string | null = null;
  public mostrarModal = false;
  
  public gol(){
    
  }

  public amarelo(){

  }

  public vermelho(){

  }

  public golContra(){

  }

  public abrirModal(){
    this.mostrarModal = true;
  }

  public fechaModal(){
    this.mostrarModal = false;
  }
}
