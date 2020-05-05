import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddDishCategoryComponent } from './add-dish-category.component';

describe('AddDishCategoryComponent', () => {
  let component: AddDishCategoryComponent;
  let fixture: ComponentFixture<AddDishCategoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddDishCategoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddDishCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
