import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TransportApiService } from '../../../../services/transport/transport-api.service';
import { Transport } from '../../../../models/transport.model';

@Component({
  selector: 'app-list-transports',
  standalone: true,
  imports: [RouterModule, CommonModule, FormsModule],
  templateUrl: './list-transports.component.html',
  styleUrls: [
    '../../../../styles/style-table.css',
    '../../../../styles/style-input-search.css',
    '../../../../styles/style-button-create.css',
    './list-transports.component.css'
  ]
})

export class ListTransportsComponent implements OnInit {
  transports: Transport[] = [];
  filteredTransports: Transport[] = [];
  inputTransportName: string = '';

  constructor(private transportApi: TransportApiService) { }

  ngOnInit(): void {
    this.transportApi.getTransports().subscribe(
      (response: Transport[]) => {
        this.transports = response;
        this.filteredTransports = this.transports;
        console.log(this.filteredTransports);
      });
  }

  searchTransport() {
    this.filteredTransports = this.transports.filter(transport => transport.name.toLowerCase().includes(this.inputTransportName.toLowerCase()));
  }
}
