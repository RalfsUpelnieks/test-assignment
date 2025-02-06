import { Apartment } from "./apartment.model";

export type HouseWithApartments = {
    houseId: number,
    number: string,
    street: string,
    city: string,
    country: string,
    postalCode: string,
    apartments: Apartment[]
};