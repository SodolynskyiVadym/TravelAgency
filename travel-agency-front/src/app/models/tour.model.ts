import { Destination } from "./destination.model";
import { Place } from "./place.model";
import { Transport } from "./transport.model";

export interface Tour {
    id: number;
    name: string;
    description: string;
    startDate: Date;
    endDate: Date;
    price: number;
    quantitySeats: number;
    imageUrl: string;
    isAvailable: boolean;
    placeStartId: number;
    placeStart: Place;
    placeEndId: number;
    placeEnd: Place;
    transportToEndId: number;
    transportToEnd: Transport;
    destinations: Destination[];
  }