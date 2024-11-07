export interface TourBasicDto {
    id: number;
    name: string;
    description: string;
    placeStartId: number;
    price: number;
    startDate: Date;
    endDate: Date;
    formattedStartDate?: string;
    formattedEndDate?: string;
    imageUrl: string;
    placeIds: [number];
    transportTypes: [string];
}