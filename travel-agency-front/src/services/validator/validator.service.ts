import { Injectable } from '@angular/core';
import axios from 'axios';

@Injectable({
  providedIn: 'root'
})
export class ValidatorService {

  constructor() { }

  async checkImageExists(imageUrl: string): Promise<boolean> {
    try {
      const response = await axios.head(imageUrl);
      return response.status === 200;
    } catch (error) {
      return false;
    }
  }
}
