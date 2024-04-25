import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemPartidaComponent } from './item-partida.component';

describe('ItemPartidaComponent', () => {
  let component: ItemPartidaComponent;
  let fixture: ComponentFixture<ItemPartidaComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ItemPartidaComponent]
    });
    fixture = TestBed.createComponent(ItemPartidaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
