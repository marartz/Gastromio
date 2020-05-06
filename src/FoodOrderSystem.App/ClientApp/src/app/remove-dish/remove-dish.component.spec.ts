import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RemoveDishCategoryComponent } from './remove-dish-category.component';

describe('RemoveDishCategoryComponent', () => {
  let component: RemoveDishCategoryComponent;
  let fixture: ComponentFixture<RemoveDishCategoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RemoveDishCategoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RemoveDishCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
