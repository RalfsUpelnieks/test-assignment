import { Resident } from "./resident.model";

export type ApartmentWithResidents = {
    apartmentId: number,
    number: string,
    floor: number,
    roomCount: number,
    residentCount: number,
    totalArea: number,
    livingArea: number,
    houseId: number,
    ownerResidentId: number | null,
    residents: Resident[]
};