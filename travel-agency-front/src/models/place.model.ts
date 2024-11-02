export interface Place {
    id: number;
    name: string;
    country: string;
    description: string;
    imagesUrls: ImageUrl[];
}

interface ImageUrl {
    url: string;
    id : number;
    placeId : number;
}