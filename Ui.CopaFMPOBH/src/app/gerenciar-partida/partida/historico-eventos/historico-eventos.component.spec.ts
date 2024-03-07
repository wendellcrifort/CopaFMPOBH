import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HistoricoEventosComponent } from './historico-eventos.component';

describe('HistoricoEventosComponent', () => {
  let component: HistoricoEventosComponent;
  let fixture: ComponentFixture<HistoricoEventosComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HistoricoEventosComponent]
    });
    fixture = TestBed.createComponent(HistoricoEventosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
