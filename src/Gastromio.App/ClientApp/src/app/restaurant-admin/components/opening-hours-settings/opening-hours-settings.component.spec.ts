import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OpeningHoursSettingsComponent } from './opening-hours-settings.component';

describe('OpeningHoursSettingsComponent', () => {
  let component: OpeningHoursSettingsComponent;
  let fixture: ComponentFixture<OpeningHoursSettingsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OpeningHoursSettingsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OpeningHoursSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
