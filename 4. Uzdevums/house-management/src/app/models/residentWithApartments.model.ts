import { Apartment } from "./apartment.model";

export type ResidentWithApartments = {
    residentId: number,
    firstName: string,
    lastName: string,
    personalCode: string,
    birthDate: Date,
    phone: string,
    email: string,
    apartments: Apartment[]
};