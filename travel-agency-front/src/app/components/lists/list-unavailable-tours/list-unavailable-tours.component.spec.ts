import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListUnavailableToursComponent } from './list-unavailable-tours.component';

describe('ListUnavailableToursComponent', () => {
  let component: ListUnavailableToursComponent;
  let fixture: ComponentFixture<ListUnavailableToursComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListUnavailableToursComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListUnavailableToursComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
