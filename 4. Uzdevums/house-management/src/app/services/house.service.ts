import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class HouseService {
    constructor(private http: HttpClient) { }

    getAll(): Observable<any> {
        return this.http.get('/api/Houses/GetAllHouses');
    }

    get(id: number): Observable<any> {
        return this.http.get(`/api/Houses/GetHouse/${id}`);
    }

    add(number: string, street: string, city: string, country: string, postalCode: string): Observable<any> {
        return this.http.post(
            '/api/Houses/CreateHouse',
            {
                number,
                street,
                city,
                country,
                postalCode,
            }
        );
    }

    update(houseId: number, number: string, street: string, city: string, country: string, postalCode: string): Observable<any> {
        return this.http.put(
            '/api/Houses/EditHouse',
            {
                houseId,
                number,
                street,
                city,
                country,
                postalCode
            }
        );
    }

    delete(id: number): Observable<any> {
        return this.http.delete(`/api/Houses/RemoveHouse/${id}`);
    }
}
