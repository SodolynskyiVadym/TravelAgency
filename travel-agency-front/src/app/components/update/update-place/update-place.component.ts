import { Component, OnInit } from '@angular/core';
import { Place } from '../../../../models/place.model';
import { ActivatedRoute, Router } from '@angular/router';
import { PlaceApiService } from '../../../../services/api/place-api.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ValidatorService } from '../../../../services/validator.service';
import { countries } from '../../../../services/constants/countries';
import { BrowserStorageService } from '../../../../services/browser-storage-service.service';
import { PlaceDto } from '../../../../models/placeDto.model';

@Component({
  selector: 'app-update-place',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './update-place.component.html',
  styleUrls: [
    './update-place.component.css',
    '../../../../assets/styles/style-form-create.css'
  ]
})
export class UpdatePlaceComponent implements OnInit {
  place: PlaceDto = {} as PlaceDto;
  countries = countries;
  isCorrectInputs: boolean = false;
  isImagesCorrect: boolean[] = [false, false, false];
  isSending: boolean = false;


  constructor(private placeApi: PlaceApiService, 
    private route: ActivatedRoute, 
    private router: Router, 
    private validator : ValidatorService,
     private browserStorage : BrowserStorageService) { }

  ngOnInit(): void {
    const id = Number.parseInt(this.route.snapshot.paramMap.get('id')?.toString() || '0');

    if (id > 0) {
      this.placeApi.getPlaceById(id).subscribe({
        next: async (response: PlaceDto | null) => {
          if (response === null) this.router.navigate(['/places']);
          else{
            this.place = response;
            for (let i = 0; i < this.place.imagesUrls.length; i++) {
              this.isImagesCorrect[i] = await this.validator.checkImageExists(this.place.imagesUrls[i]);
            }
            this.checkInputs();
          }
        },
        error: () => {
          this.router.navigate(['/']);
        }
      });
    } else {
      this.router.navigate(['/']);
    }
  }

  checkInputs(){
    this.isCorrectInputs = this.place.name != '' && this.place.country != '' && this.place.description != '' && this.isImagesCorrect.every(image => image);
  }

  async checkImageUrl(index: number) {
    this.isImagesCorrect[index] = await this.validator.checkImageExists(this.place.imagesUrls[index]);
    this.checkInputs();
  }

  updatePlace(){
    this.isSending = true;
    let token = this.browserStorage.get('token');
    if (token) {
      this.placeApi.updatePlace(this.place, token).subscribe({
        next: (response) => {
          this.isSending = false;
          this.isCorrectInputs = false;
        }, error: (error) => {
          this.isSending = false;
          alert('Error updating place, try again later');
        }
      });
    } else {
      this.isSending = false;
      this.router.navigate(['/login']);
    }
  }
}
