import { Injectable } from '@angular/core';
import { Stripe, loadStripe } from '@stripe/stripe-js';
import { environment } from '../environment/environment';

@Injectable({
  providedIn: 'root'
})

export class StripeService {
  private stripePromise: Promise<Stripe | null>;

  constructor() {
    this.stripePromise = loadStripe(environment.stripePublishKey);
  }

  getStripe() {
    return this.stripePromise;
  }
}
