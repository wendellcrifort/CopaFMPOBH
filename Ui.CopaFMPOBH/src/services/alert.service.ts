import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AlertService {
  private alertSubject = new Subject<any>();

  constructor() { }

  getAlerts() {
    return this.alertSubject.asObservable();
  }

  showAlertSuccess(mensagem: string) {
    this.showAlert('success', mensagem);
  }

  showAlertDanger(mensagem: string) {
    this.showAlert('danger', mensagem);
  }

  private showAlert(type: string, message: string) {
    this.alertSubject.next({ type, message });
  }
}