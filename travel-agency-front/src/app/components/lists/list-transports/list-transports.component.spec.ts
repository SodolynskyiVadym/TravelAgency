import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListTransportsComponent } from './list-transports.component';

describe('ListTransportsComponent', () => {
  let component: ListTransportsComponent;
  let fixture: ComponentFixture<ListTransportsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListTransportsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListTransportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
