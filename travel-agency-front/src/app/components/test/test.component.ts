import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-test',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './test.component.html',
  styleUrl: './test.component.css'
})

export class TestComponent implements OnInit{
  ngOnInit(): void {
    console.log(this.test)
  }
  test = {
    date : Date.now(),
  }

  showDate(){
    console.log(this.test)
  }
}
