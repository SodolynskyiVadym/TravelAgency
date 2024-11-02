import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Transport } from '../../../../models/transport.model';
import { TransportApiService } from '../../../../services/api/transport-api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BrowserStorageService } from '../../../../services/browser-storage-service.service';
import { ValidatorService } from '../../../../services/validator.service';

@Component({
  selector: 'app-update-transport',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './update-transport.component.html',
  styleUrls: [
    './update-transport.component.css',
    '../../../../assets/styles/style-form-create.css'
  ]
})
export class UpdateTransportComponent implements OnInit {
  transport = {} as Transport;
  isSending = false;
  isCorrectInputs = true;
  isCorrectImage = true;

  constructor(private transportApi: TransportApiService,
    private router: Router,
    private browserStorage: BrowserStorageService,
    private route: ActivatedRoute,
    private validator: ValidatorService) { }

  ngOnInit(): void {
    const id = Number.parseInt(this.route.snapshot.paramMap.get('id')?.toString() || '0');
    if (id > 0) {
      this.transportApi.getTransportById(id).subscribe({
        next: (response) => {
          this.transport = response;
          this.checkImage();
          this.checkInputs();
        },
        error: () => {
          this.router.navigate(['/transports']);
        }
      });
    } else {
      this.router.navigate(['/transports']);
    }
  }

  async checkImage() {
    this.isCorrectImage = await this.validator.checkImageExists(this.transport.imageUrl);
  }

  checkInputs() {
    this.isCorrectInputs = this.transport.name != '' && this.transport.description != '' && this.isCorrectImage && this.transport.type != '';
  }

  updateTransport() {
    this.isSending = true;
    let token = this.browserStorage.get("token");
    if (token == null) this.router.navigate(['/login']);
    else {
      this.transportApi.updateTransport(this.transport, token).subscribe({
        next: () => {
          this.router.navigate(['/transports']);
        },
        error: () => {
          alert("Error while updating transport");
        }
      });
    }
    this.isSending = false;
  }
}