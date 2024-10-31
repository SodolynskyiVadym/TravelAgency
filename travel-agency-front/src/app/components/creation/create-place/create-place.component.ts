import { Component } from '@angular/core';
import { countries } from '../../../../services/constants/countries';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ValidatorService } from '../../../../services/validator.service';

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
export class CreatePlaceComponent{
  place : any = {
    name: '',
    country: '',
    description: '',
    imagesUrls: ['', '', '']
  }
  countries : string[] = countries;
  isImagesCorrect : boolean[] = [false, false, false];
  isCorrectInputs : boolean = false;
  isSending : boolean = false;

  constructor(private validator : ValidatorService) { }
  

  async checkImageUrl(index: number){
    this.isImagesCorrect[index] = await this.validator.checkImageExists(this.place.imagesUrls[index]);
    this.checkInputs();
  }


  checkInputs(){
    this.isCorrectInputs = this.place.name != '' && this.place.country != '' && this.place.description != '' && this.isImagesCorrect.every(image => image);
  }

  createPlace(){

  }

}
