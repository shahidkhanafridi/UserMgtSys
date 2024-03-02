import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OkOnlyComponent } from './ok-only.component';

describe('OkOnlyComponent', () => {
  let component: OkOnlyComponent;
  let fixture: ComponentFixture<OkOnlyComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OkOnlyComponent]
    });
    fixture = TestBed.createComponent(OkOnlyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
