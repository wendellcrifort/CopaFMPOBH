import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-modal-confimacao',
  templateUrl: './modal-confimacao.component.html',
  styleUrls: ['./modal-confimacao.component.css']
})
export class ModalConfimacaoComponent {
  @Input() titulo: string | null = null;
  @Output() cancelar = new EventEmitter();
  @Output() confirmar = new EventEmitter();

  public onCancelar(){
    this.cancelar.emit();
  }

  public onConfirmar(){
    this.confirmar.emit();
  }
}
