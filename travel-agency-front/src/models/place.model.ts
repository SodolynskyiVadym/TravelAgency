import { ImageUrl } from "./imageUrl.model";

export interface Place {
    id: number;
    name: string;
    country: string;
    description: string;
    imagesUrls: ImageUrl[];
}