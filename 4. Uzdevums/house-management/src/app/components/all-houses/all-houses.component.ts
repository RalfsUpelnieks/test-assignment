import { Component, ViewChild, EventEmitter } from '@angular/core';
import { FormControl, Validators, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HouseService } from '../../services/house.service';
import { House } from '../../models/house.model';
import { DeletionConfirmationComponent } from '../shared/deletion-confirmation/deletion-confirmation.component';
import { PopupComponent } from '../shared/popup/popup.component';

@Component({
    selector: 'app-all-houses',
    standalone: true,
    imports: [CommonModule, RouterModule, ReactiveFormsModule, DeletionConfirmationComponent, PopupComponent],
    templateUrl: './all-houses.component.html',
})
export class AllHousesComponent {
    @ViewChild(DeletionConfirmationComponent) deleteModal!: DeletionConfirmationComponent;

    houses: House[] = [];

    addHouseIsVisible = false;
    addHouseIsLoading = false;
    addHouseErrorMessage = '';
    addHouseForm = new FormGroup({
        number: new FormControl('', Validators.required),
        street: new FormControl('', Validators.required),
        city: new FormControl('', Validators.required),
        country: new FormControl('', Validators.required),
        postalCode: new FormControl('', Validators.required)
    });

    constructor(private houseService: HouseService) {}

    ngOnInit(): void {
        this.houseService.getAll().subscribe({
            next: (response) => this.houses = response,
            error: (err) => console.error('Error fetching data', err),
        });

        this.addHouseForm.valueChanges.subscribe(() => {
            this.addHouseErrorMessage = '';
        });
    }

    showAddHouse() {
        this.addHouseIsVisible = true;
    }
    
    closeAddHouse() {
        this.addHouseIsVisible = false;
        this.addHouseForm.reset();
    }
    
    onHouseAdd() {
        if (this.addHouseForm.valid) {
            this.addHouseIsLoading = true;

            var values = this.addHouseForm.value;
            this.houseService.add(values.number!, values.street!, values.city!, values.country!, values.postalCode!).subscribe({
                next: (response) => {
                    this.houses.push(response);
                    this.closeAddHouse();
                },
                error: (err) => this.addHouseErrorMessage = err.error,
                complete: () => {
                    this.addHouseIsLoading = false;
                }
            });
        } else {
            this.addHouseErrorMessage = 'Please fill in all required fields correctly.';
        }
    }

    confirmHouseDeletion(id: number) {
        this.deleteModal.item = 'house with ID: ' + id;

        const deleteFunction = new EventEmitter<void>();
        deleteFunction.subscribe(() => this.onHouseDelete(id));
        
        this.deleteModal.deleteConfirmed = deleteFunction;
        
        this.deleteModal.show();
    }

    onHouseDelete(id: number) {
        this.houseService.delete(id).subscribe({
            next: () => this.houses = this.houses.filter(house => house.houseId !== id),
            error: (err) => console.error('Error fetching data', err),
        });
    }
}
