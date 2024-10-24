import { Place } from "./place.model";

export interface Hotel {
    id: number;
    name: string;
    address: string;
    description: string;
    pricePerNight: number;
    imageUrl: string;
    placeId: number;
    place: Place;
}