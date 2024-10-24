import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateTransportComponent } from './create-transport.component';

describe('CreateTransportComponent', () => {
  let component: CreateTransportComponent;
  let fixture: ComponentFixture<CreateTransportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateTransportComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateTransportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
