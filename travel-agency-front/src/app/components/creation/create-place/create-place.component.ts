import { Component } from '@angular/core';
import { countries } from '../../../../services/constants/countries';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ValidatorService } from '../../../../services/validator.service';
import { PlaceApiService } from '../../../../services/api/place-api.service';
import { BrowserStorageService } from '../../../../services/browser-storage-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-place',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './create-place.component.html',
  styleUrls: [
    './create-place.component.css',
    '../../../../assets/styles/style-form-create.css'
  ]
})
export class CreatePlaceComponent {
  place: any = {
    name: '',
    country: '',
    description: '',
    imagesUrls: ['', '', '']
  }
  countries: string[] = countries;
  isImagesCorrect: boolean[] = [false, false, false];
  isCorrectInputs: boolean = false;
  isSending: boolean = false;

  constructor(private validator: ValidatorService, private placeApi: PlaceApiService, private broserStorage: BrowserStorageService, private router: Router) { }


  async checkImageUrl(index: number) {
    this.isImagesCorrect[index] = await this.validator.checkImageExists(this.place.imagesUrls[index]);
    this.checkInputs();
  }


  checkInputs() {
    this.isCorrectInputs = this.place.name != '' && this.place.country != '' && this.place.description != '' && this.isImagesCorrect.every(image => image);
  }

  createPlace() {
    this.isSending = true;
    let token = this.broserStorage.get('token');
    if (token) {
      this.placeApi.createPlace(this.place, token).subscribe({
        next: (response) => {
          this.isSending = false;
          this.router.navigate(['/places']);
        }, error: (error) => {
          this.isSending = false;
          alert('Error creating place, try again later');
        }
      });
    } else {
      this.isSending = false;
      this.router.navigate(['/login']);
    }
  }
}
