import { Hotel } from './hotel.model';
import { Transport } from './transport.model';

export interface Destination {
    id: number;
    startDate: Date;
    formattedStartDate: string;
    endDate: Date;
    formattedEndDate: string;
    tourId: number;
    hotelId: number;
    hotel: Hotel;
    transportId: number;
    transport: Transport;
}