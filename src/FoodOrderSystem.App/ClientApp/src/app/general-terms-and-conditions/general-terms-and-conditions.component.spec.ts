import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneralTermsAndConditionsComponent } from './general-terms-and-conditions.component';

describe('GeneralTermsAndConditionsComponent', () => {
  let component: GeneralTermsAndConditionsComponent;
  let fixture: ComponentFixture<GeneralTermsAndConditionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GeneralTermsAndConditionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GeneralTermsAndConditionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
