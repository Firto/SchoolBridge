import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EndForgotPasswordComponent } from './end-forgot-password.component';

describe('EndForgotPasswordComponent', () => {
  let component: EndForgotPasswordComponent;
  let fixture: ComponentFixture<EndForgotPasswordComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EndForgotPasswordComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EndForgotPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
