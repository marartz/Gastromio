import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CuisineSettingsComponent } from './cuisine-settings.component';

describe('CuisineSettingsComponent', () => {
  let component: CuisineSettingsComponent;
  let fixture: ComponentFixture<CuisineSettingsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CuisineSettingsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CuisineSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
