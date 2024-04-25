import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MelhorJogadorComponent } from './melhor-jogador.component';

describe('MelhorJogadorComponent', () => {
  let component: MelhorJogadorComponent;
  let fixture: ComponentFixture<MelhorJogadorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MelhorJogadorComponent]
    });
    fixture = TestBed.createComponent(MelhorJogadorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
