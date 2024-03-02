import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Testing2Component } from './testing2.component';

describe('Testing2Component', () => {
  let component: Testing2Component;
  let fixture: ComponentFixture<Testing2Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [Testing2Component]
    });
    fixture = TestBed.createComponent(Testing2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
