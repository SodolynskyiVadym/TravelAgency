import { Component, OnInit } from '@angular/core';
import { Transport } from '../../../../models/transport.model';
import { TransportApiService } from '../../../../services/api/transport-api.service';
import { BrowserStorageService } from '../../../../services/browser-storage-service.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ValidatorService } from '../../../../services/validator.service';

@Component({
  selector: 'app-create-transport',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-transport.component.html',
  styleUrls: [
    './create-transport.component.css',
    '../../../../assets/styles/style-form-create.css'
  ]
})
export class CreateTransportComponent implements OnInit {
  transport = {} as Transport;
  isCorrectInputs: boolean = false;
  isCorrectImage: boolean = false;
  isSending: boolean = false;
  
  constructor(private transportApi : TransportApiService, 
    private browserStorage : BrowserStorageService, 
    private router : Router, 
    private validator : ValidatorService) { }

  ngOnInit(): void {
    
  }

  async checkImage(){
    this.isCorrectImage = await this.validator.checkImageExists(this.transport.imageUrl);
  }

  async checkInputs(){
    this.isCorrectInputs = this.transport.name != '' && this.transport.type != '' && this.transport.description != '' 
      && await this.validator.checkImageExists(this.transport.imageUrl);
  }

  createTransport() {
    this.isSending = true;
    let token = this.browserStorage.get("token");
    if (token == null) this.router.navigate(['/login']);
    else {
      this.transportApi.createTransport(this.transport, token).subscribe({
        next: () => {
          this.router.navigate(['/transports']);
        }, error: (err) => {
          alert("Transport wasn't created. Please try again.");
        }
      });
    }
    this.isSending = false;
  }

}
