import { Component, ViewChild, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HouseService } from '../../services/house.service';
import { ApartmentService } from '../../services/apartment.service';
import { HouseWithApartments } from '../../models/houseWithApartments.model';
import { DeletionConfirmationComponent } from '../shared/deletion-confirmation/deletion-confirmation.component';
import { PopupComponent } from '../shared/popup/popup.component';

@Component({
    selector: 'app-house-detail',
    standalone: true,
    imports: [CommonModule, RouterModule, ReactiveFormsModule, DeletionConfirmationComponent, PopupComponent],
    templateUrl: './house-detail.component.html',
})
export class HouseDetailComponent {
    @ViewChild(DeletionConfirmationComponent) deleteModal!: DeletionConfirmationComponent;

    hosueId: number | null = null;
    house: HouseWithApartments | null = null;

    editHouseMessageIsPositive = true;
    editHouseMessage = '';
    editHouseIsLoading = false;
    editHouseForm = new FormGroup({
        number: new FormControl('', Validators.required),
        street: new FormControl('', Validators.required),
        city: new FormControl('', Validators.required),
        country: new FormControl('', Validators.required),
        postalCode: new FormControl('', Validators.required)
    });

    addApartmentIsVisible = false;
    addApartmentIsLoading = false;
    addApartmentErrorMessage = '';
    addApartmentForm = new FormGroup({
        number: new FormControl('', Validators.required),
        floor: new FormControl(null, Validators.required),
        roomCount: new FormControl(null, Validators.required),
        totalArea: new FormControl(null, Validators.required),
        livingArea: new FormControl(null, Validators.required)
    });

    constructor(
        private houseService: HouseService,
        private apartmentService: ApartmentService,
        private route: ActivatedRoute,
        private router: Router
    ) {}

    ngOnInit(): void {
        this.hosueId = Number(this.route.snapshot.paramMap.get('id'));

        if (!this.hosueId) {
            console.error('No ID provided');
            this.router.navigate(['/']);
            return;
        }
        
        this.houseService.get(this.hosueId).subscribe({
            next: (response) => {
                this.house = response
                this.editHouseForm.setValue({
                    number: response.number,
                    street: response.street,
                    city: response.city,
                    country: response.country,
                    postalCode: response.postalCode
                });
            },
            error: (err) => {
                console.error('Error fetching data', err)
                this.router.navigate(['/']);
            },
        });

        this.editHouseForm.valueChanges.subscribe(() => {
            this.editHouseMessage = '';
        });

        this.addApartmentForm.valueChanges.subscribe(() => {
            this.addApartmentErrorMessage = '';
        });
    }

    onHouseEdit() {
        if (this.editHouseForm.valid && this.house) {
            var values = this.editHouseForm.value;
            if(values.number === this.house.number && values.street === this.house.street && values.city === this.house.city && values.country === this.house.country && values.postalCode === this.house.postalCode) {
                this.editHouseMessageIsPositive = false;
                this.editHouseMessage = 'No changes were made.';
                return;
            }

            this.editHouseIsLoading = true;

            this.houseService.update(this.house.houseId, values.number!, values.street!, values.city!, values.country!, values.postalCode!).subscribe({
                next: () => {
                    this.house!.number = values.number!;
                    this.house!.street = values.street!;
                    this.house!.city = values.city!;
                    this.house!.country = values.country!;
                    this.house!.postalCode = values.postalCode!;
                    
                    this.editHouseMessageIsPositive = true;
                    this.editHouseMessage = 'House updated successfully.';
                },
                error: (err) => {
                    this.editHouseMessageIsPositive = false;
                    this.editHouseMessage = err.error;
                },
                complete: () => {
                    this.editHouseIsLoading = false;
                }
            });
        } else {
            this.editHouseMessageIsPositive = false;
            this.editHouseMessage = 'Please fill in all required fields correctly.';
        }
    }

    confirmHouseDeletion() {
        this.deleteModal.title = 'Delete house';
        this.deleteModal.item = 'house with ID: ' + this.house!.houseId;

        const deleteFunction = new EventEmitter<void>();
        deleteFunction.subscribe(() => this.onHouseDelete());
        
        this.deleteModal.deleteConfirmed = deleteFunction;
        
        this.deleteModal.show();
    }

    onHouseDelete() {
        this.houseService.delete(this.house!.houseId).subscribe({
            next: () => this.router.navigate(['/']),
            error: (err) => console.error('Error fetching data', err),
        });
    }

    showAddApartment() {
        this.addApartmentIsVisible = true;
    }
    
    closeAddApartment() {
        this.addApartmentIsVisible = false;
        this.addApartmentForm.reset();
    }
    
    onApartmentAdd() {
        if (this.addApartmentForm.valid) {
            this.addApartmentIsLoading = true;

            var values = this.addApartmentForm.value;
            this.apartmentService.add(values.number!, values.floor!, values.roomCount!, values.totalArea!, values.livingArea!, this.house!.houseId.toString()).subscribe({
                next: (response) => {
                    this.house?.apartments.push(response);
                    this.closeAddApartment();
                },
                error: (err) => this.addApartmentErrorMessage = err.error,
                complete: () => {
                    this.addApartmentIsLoading = false;
                }
            });
        } else {
            this.addApartmentErrorMessage = 'Please fill in all required fields correctly.';
        }
    }

    confirmApartmentDeletion(id: number) {
        this.deleteModal.title = 'Delete apartment';
        this.deleteModal.item = 'apartment with ID: ' + id;

        const deleteFunction = new EventEmitter<void>();
        deleteFunction.subscribe(() => this.onApartmentDelete(id));
        
        this.deleteModal.deleteConfirmed = deleteFunction;
        
        this.deleteModal.show();
    }

    onApartmentDelete(id: number) {
        this.apartmentService.delete(id).subscribe({
            next: () => {

                this.house!.apartments = this.house!.apartments.filter(apartment => apartment.apartmentId !== id);
            },
            error: (err) => console.error('Error fetching data', err),
        });
    }
}
