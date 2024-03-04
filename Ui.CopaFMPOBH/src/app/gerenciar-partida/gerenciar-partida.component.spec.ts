import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GerenciarPartidaComponent } from './gerenciar-partida.component';

describe('GerenciarPartidaComponent', () => {
  let component: GerenciarPartidaComponent;
  let fixture: ComponentFixture<GerenciarPartidaComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GerenciarPartidaComponent]
    });
    fixture = TestBed.createComponent(GerenciarPartidaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
