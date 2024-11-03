import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { TransportApiService } from '../../../../services/api/transport-api.service';
import { Transport } from '../../../../models/transport.model';
import { BrowserStorageService } from '../../../../services/browser-storage-service.service';

@Component({
  selector: 'app-list-transports',
  standalone: true,
  imports: [RouterModule, CommonModule, FormsModule],
  templateUrl: './list-transports.component.html',
  styleUrls: [
    '../../../../assets/styles/style-table.css',
    '../../../../assets/styles/style-input-search.css',
    '../../../../assets/styles/style-button-create.css',
    './list-transports.component.css'
  ]
})

export class ListTransportsComponent implements OnInit {
  transports: Transport[] = [];
  filteredTransports: Transport[] = [];
  inputTransportName: string = '';
  usedTransportsIds: number[] = [];

  constructor(private transportApi: TransportApiService, private browserStorage: BrowserStorageService, private router: Router) { }

  ngOnInit(): void {
    this.transportApi.getTransports().subscribe(
      (response: Transport[]) => {
        this.transports = response;
        this.filteredTransports = this.transports;
      });

    this.transportApi.getUsedTransportsIds().subscribe({
      next: (response: number[]) => {
        this.usedTransportsIds = response;
      },
      error: (error) => {
        this.usedTransportsIds = this.transports.map(transport => transport.id);
      }
    });
  }

  searchTransport() {
    this.filteredTransports = this.transports.filter(transport => transport.name.toLowerCase().includes(this.inputTransportName.toLowerCase()));
  }

  deleteTransport(id: number) {
    let token = this.browserStorage.get('token');
    if (token == null) {
      this.router.navigate(['/login']);
      return;
    }
    this.transportApi.deleteTransport(id, token).subscribe({
      next: (response) => {
        this.transports = this.transports.filter(transport => transport.id != id);
        this.filteredTransports = this.filteredTransports.filter(transport => transport.id != id);
      },
      error: (error) => {
        alert('Error deleting transport');
      }
    });
  }
}
