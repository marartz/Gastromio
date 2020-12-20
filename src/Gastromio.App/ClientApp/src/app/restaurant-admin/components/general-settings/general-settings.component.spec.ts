import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RestaurantSettingsGeneralComponent } from './restaurant-settings-general.component';

describe('RestaurantSettingsGeneralComponent', () => {
  let component: RestaurantSettingsGeneralComponent;
  let fixture: ComponentFixture<RestaurantSettingsGeneralComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RestaurantSettingsGeneralComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RestaurantSettingsGeneralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
