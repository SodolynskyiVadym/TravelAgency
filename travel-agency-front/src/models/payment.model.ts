import { Tour } from "./tour.model";
import { User } from "./user.model";

export interface Payment {
    id: number;
    amount: number;
    isPaid: boolean;
    date: Date;
    stripeSession?: string;
    userId: number;
    user: User;
    tourId: number;
    tour: Tour;
}