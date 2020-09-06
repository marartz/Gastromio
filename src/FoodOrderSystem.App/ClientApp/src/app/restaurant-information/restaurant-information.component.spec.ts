import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RestaurantInformationComponent } from './restaurant-information.component';

describe('RestaurantInformationComponent', () => {
  let component: RestaurantInformationComponent;
  let fixture: ComponentFixture<RestaurantInformationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RestaurantInformationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RestaurantInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});