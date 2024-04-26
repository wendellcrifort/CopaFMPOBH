import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalConfimacaoComponent } from './modal-confimacao.component';

describe('ModalConfimacaoComponent', () => {
  let component: ModalConfimacaoComponent;
  let fixture: ComponentFixture<ModalConfimacaoComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ModalConfimacaoComponent]
    });
    fixture = TestBed.createComponent(ModalConfimacaoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
