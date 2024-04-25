import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArtilheirosComponent } from './artilheiros.component';

describe('ArtilheirosComponent', () => {
  let component: ArtilheirosComponent;
  let fixture: ComponentFixture<ArtilheirosComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ArtilheirosComponent]
    });
    fixture = TestBed.createComponent(ArtilheirosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
