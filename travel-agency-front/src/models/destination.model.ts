import { Hotel } from './hotel.model';
import { Transport } from './transport.model';

export interface Destination {
    id: number;
    startDate: Date;
    endDate: Date;
    tourId: number;
    hotelId: number;
    hotel: Hotel;
    transportId: number;
    transport: Transport;
}