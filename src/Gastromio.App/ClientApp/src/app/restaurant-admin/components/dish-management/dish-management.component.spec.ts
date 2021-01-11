import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DishManagementComponent } from './dish-management.component';

describe('DishManagementComponent', () => {
  let component: DishManagementComponent;
  let fixture: ComponentFixture<DishManagementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DishManagementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DishManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
