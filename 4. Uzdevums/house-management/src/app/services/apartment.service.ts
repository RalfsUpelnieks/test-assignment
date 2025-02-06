import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class ApartmentService {
    constructor(private http: HttpClient) { }

    getAll(): Observable<any> {
        return this.http.get('/api/Apartments/GetAllApartments');
    }

    get(id: number): Observable<any> {
        return this.http.get(`/api/Apartments/GetApartment/${id}`);
    }

    add(number: string, floor: number, roomCount: number, totalArea: number, livingArea: number, houseId: string): Observable<any> {
        return this.http.post(
            '/api/Apartments/CreateApartment',
            {
                number,
                floor,
                roomCount,
                totalArea,
                livingArea,
                houseId
            }
        );
    }

    update(apartmentId: number, number: string, floor: number, roomCount: number, totalArea: number, livingArea: number, houseId: number): Observable<any> {
        return this.http.put(
            '/api/Apartments/EditApartment',
            {
                apartmentId,
                number,
                floor,
                roomCount,
                totalArea,
                livingArea,
                houseId
            }
        );
    }

    delete(id: number): Observable<any> {
        return this.http.delete(`/api/Apartments/RemoveApartment/${id}`);
    }
}
