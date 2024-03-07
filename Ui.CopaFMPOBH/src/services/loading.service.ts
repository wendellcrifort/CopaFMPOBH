import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  private loadingSubject = new BehaviorSubject<boolean>(false);

  constructor() { }

  getLoading() {
    return this.loadingSubject.asObservable();
  }

  setLoading(loading: boolean) {
    this.loadingSubject.next(loading);
  }
}
