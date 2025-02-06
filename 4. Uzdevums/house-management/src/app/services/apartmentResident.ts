import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class ApartmentResidentService {
    constructor(private http: HttpClient) { }

    add(ApartmentId: number, ResidentId: number, IsOwner: boolean): Observable<any> {
        return this.http.post(
            '/api/ApartmentResident/CreateApartmentResident',
            {
                ApartmentId,
                ResidentId,
                IsOwner,
            }
        );
    }

    update(ApartmentId: number, ResidentId: number, IsOwner: boolean): Observable<any> {
        return this.http.put(
            '/api/ApartmentResident/EditApartmentResident',
            {
                ApartmentId,
                ResidentId,
                IsOwner,
            }
        );
    }

    delete(ApartmentId: number, ResidentId: number): Observable<any> {
        return this.http.delete(`/api/ApartmentResident/RemoveApartmentResident/${ApartmentId}/${ResidentId}`);
    }
}