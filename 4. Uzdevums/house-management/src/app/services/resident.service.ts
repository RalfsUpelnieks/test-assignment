import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class ResidentService {
    constructor(private http: HttpClient) { }

    getAll(): Observable<any> {
        return this.http.get('/api/Residents/GetAllResidents');
    }

    get(id: number): Observable<any> {
        return this.http.get(`/api/Residents/GetResident/${id}`);
    }

    add(firstName: string, lastName: string, personalCode: string, birthDate: Date, phone: string, email: string): Observable<any> {
        return this.http.post(
            '/api/Residents/CreateResident',
            {
                firstName,
                lastName,
                personalCode,
                birthDate,
                phone,
                email
            }
        );
    }

    update(residentId: number, firstName: string, lastName: string, personalCode: string, birthDate: Date, phone: string, email: string): Observable<any> {
        return this.http.put(
            '/api/Residents/EditResident',
            {
                residentId,
                firstName,
                lastName,
                personalCode,
                birthDate,
                phone,
                email
            }
        );
    }

    delete(id: number): Observable<any> {
        return this.http.delete(`/api/Residents/RemoveResident/${id}`);
    }
}
