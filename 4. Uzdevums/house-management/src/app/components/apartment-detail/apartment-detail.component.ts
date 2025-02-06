import { Component, ViewChild, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule, formatDate } from '@angular/common';
import { ApartmentWithResidents } from '../../models/apartmentWithResidents.model';
import { ApartmentService } from '../../services/apartment.service';
import { ResidentService } from '../../services/resident.service';
import { ApartmentResidentService } from '../../services/apartmentResident';
import { DeletionConfirmationComponent } from '../shared/deletion-confirmation/deletion-confirmation.component';
import { PopupComponent } from '../shared/popup/popup.component';
import { Resident } from '../../models/resident.model';
import { ResidentWithApartments } from '../../models/residentWithApartments.model';

@Component({
    selector: 'app-apartment-detail',
    standalone: true,
    imports: [CommonModule, RouterModule, ReactiveFormsModule, DeletionConfirmationComponent, PopupComponent],
    templateUrl: './apartment-detail.component.html',
})
export class ApartmentDetailComponent {
    @ViewChild(DeletionConfirmationComponent) deleteModal!: DeletionConfirmationComponent;

    readonly MENU_CLOSED = '';
    readonly MENU_ALL_RESIDENTS = 'allResidents';
    readonly MENU_ADD_RESIDENT = 'addResidents';
    readonly MENU_RESIDENT_DETAILS = 'editResidents';

    apartmentId: number | null = null;
    apartment: ApartmentWithResidents | null = null;
    allResidents: Resident[] = [];
    residentDetails: ResidentWithApartments | null = null;
    
    visibleMenu: string = this.MENU_CLOSED;

    editApartmentMessageIsPositive = true;
    editApartmentIsLoading = false;
    editApartmentMessage = '';
    editApartmentForm = new FormGroup({
        number: new FormControl('', Validators.required),
        floor: new FormControl(null, Validators.required),
        roomCount: new FormControl(null, Validators.required),
        totalArea: new FormControl(null, Validators.required),
        livingArea: new FormControl(null, Validators.required)
    });

    addResidentErrorMessage = '';
    addResidentIsLoading = false
    addResidentForm = new FormGroup({
        firstName: new FormControl('', Validators.required),
        lastName: new FormControl('', Validators.required),
        personalCode: new FormControl('', Validators.required),
        birthDate: new FormControl(null, Validators.required),
        phone: new FormControl('', Validators.required),
        email: new FormControl('', Validators.required)
    });
    
    editResidentMessage = '';
    editResidentIsLoading = false
    editResidentForm = new FormGroup({
        firstName: new FormControl('', Validators.required),
        lastName: new FormControl('', Validators.required),
        personalCode: new FormControl('', Validators.required),
        birthDate: new FormControl('', Validators.required),
        phone: new FormControl('', Validators.required),
        email: new FormControl('', Validators.required)
    });

    editResidentApartmentIsLoading = false;

    constructor(
        private apartmentService: ApartmentService,
        private residentService: ResidentService,
        private apartmentResidentService: ApartmentResidentService,
        private route: ActivatedRoute,
        private router: Router
    ) {}

    ngOnInit(): void {
        this.route.paramMap.subscribe(params => {
            this.apartmentId = Number(params.get('id'));

            if (!this.apartmentId) {
                console.error('No ID provided');
                this.router.navigate(['/']);
                return;
            }
            
            this.apartmentService.get(this.apartmentId).subscribe({
                next: (response) => {
                    this.apartment = response
                    this.editApartmentForm.setValue({
                        number: response.number,
                        floor: response.floor,
                        roomCount: response.roomCount,
                        totalArea: response.totalArea,
                        livingArea: response.livingArea
                    });
                },
                error: (err) => {
                    console.error('Error fetching data', err)
                    this.router.navigate(['/']);
                },
            });
        });

        this.editApartmentForm.valueChanges.subscribe(() => {
            this.editApartmentMessage = '';
        });

        this.addResidentForm.valueChanges.subscribe(() => {
            this.addResidentErrorMessage = '';
        });

        this.editResidentForm.valueChanges.subscribe(() => {
            this.editResidentMessage = '';
        });
    }

    onApartmentEdit() {
        if (this.editApartmentForm.valid && this.apartment) {
            var values = this.editApartmentForm.value;
            if(values.number === this.apartment.number && values.floor! === this.apartment.floor && values.roomCount! === this.apartment.roomCount && values.totalArea! === this.apartment.totalArea && values.livingArea! === this.apartment.livingArea) {
                this.editApartmentMessageIsPositive = false;
                this.editApartmentMessage = 'No changes were made.';
                return;
            }

            this.editApartmentIsLoading = true;

            this.apartmentService.update(this.apartment.apartmentId, values.number!, values.floor!, values.roomCount!, values.totalArea!, values.livingArea!, this.apartment.houseId).subscribe({
                next: () => {
                    this.apartment!.number = values.number!
                    this.apartment!.floor = values.floor!
                    this.apartment!.roomCount = values.roomCount!
                    this.apartment!.totalArea = values.totalArea!
                    this.apartment!.livingArea = values.livingArea!
                    
                    this.editApartmentMessageIsPositive = true;
                    this.editApartmentMessage = 'Apartment updated successfully.';
                },
                error: (err) => {
                    this.editApartmentMessageIsPositive = false;
                    this.editApartmentMessage = err.error;
                },
                complete: () => {
                    this.editApartmentIsLoading = false;
                }
            });
        } else {
            this.editApartmentMessageIsPositive = false;
            this.editApartmentMessage = 'Please fill in all required fields correctly.';
        }
    }

    confirmApartmentDeletion() {
        this.deleteModal.title = 'Delete apartment';
        this.deleteModal.item = 'apartment with ID: ' + this.apartment!.apartmentId;

        const deleteFunction = new EventEmitter<void>();
        deleteFunction.subscribe(() => this.onApartmentDelete());
        
        this.deleteModal.deleteConfirmed = deleteFunction;
        
        this.deleteModal.show();
    }

    onApartmentDelete() {
        this.apartmentService.delete(this.apartment!.apartmentId).subscribe({
            next: () => this.router.navigate(['/house', this.apartment!.houseId]),
            error: (err) => console.error('Error fetching data', err),
        });
    }

    showAllResidents() {
        this.residentService.getAll().subscribe({
            next: (response) => {
                this.allResidents = response
                this.visibleMenu = this.MENU_ALL_RESIDENTS;
            },
            error: (err) => {
                console.error('Error fetching data', err)
            },
        });
    }

    showAddResident() {
        this.addResidentForm.reset();
        this.visibleMenu = this.MENU_ADD_RESIDENT;
    }

    showResidentDetails(id: number) {
        this.residentService.get(id).subscribe({
            next: (response) => {
                this.residentDetails = response
                this.editResidentForm.setValue({
                    firstName: response.firstName,
                    lastName: response.lastName,
                    personalCode: response.personalCode,
                    birthDate: formatDate(new Date(response.birthDate), 'yyyy-MM-dd', 'en-US'),
                    phone: response.phone,
                    email: response.email
                });

                this.visibleMenu = this.MENU_RESIDENT_DETAILS;
            },
            error: (err) => console.error('Error fetching data', err),
        });
    }

    closeMenu() {
        this.visibleMenu = this.MENU_CLOSED;
    }

    onResidentAdd() {
        if (this.addResidentForm.valid) {
            this.addResidentIsLoading = true;

            var values = this.addResidentForm.value;
            this.residentService.add(values.firstName!, values.lastName!, values.personalCode!, new Date(values.birthDate!), values.phone!, values.email!).subscribe({
                next: () => {
                    this.showAllResidents()
                },
                error: (err) => this.addResidentErrorMessage = err.error,
                complete: () => {
                    this.addResidentIsLoading = false;
                }
            });
        } else {
            this.addResidentErrorMessage = 'Please fill in all required fields correctly.';
        }
    }

    onResidentEdit() {
        if (this.editResidentForm.valid && this.residentDetails) {
            var values = this.editResidentForm.value;
            const newBirthday = new Date(values.birthDate!);
            const oldBirthday = new Date(this.residentDetails.birthDate);

            if (values.firstName === this.residentDetails.firstName && values.lastName! === this.residentDetails.lastName && values.personalCode! === this.residentDetails.personalCode && values.phone! === this.residentDetails.phone && values.email! === this.residentDetails.email && newBirthday.getDay() === oldBirthday.getDay() && newBirthday.getMonth() === oldBirthday.getMonth() && newBirthday.getFullYear() === oldBirthday.getFullYear()) {
                this.editResidentMessage = 'No changes were made.';
                return;
            }

            this.editResidentIsLoading = true;

            this.residentService.update(this.residentDetails.residentId, values.firstName!, values.lastName!, values.personalCode!, newBirthday, values.phone!, values.email!).subscribe({
                next: () => {
                    this.closeMenu()
                },
                error: (err) => {
                    this.editApartmentMessageIsPositive = false;
                    this.editApartmentMessage = err.error;
                },
                complete: () => {
                    this.editApartmentIsLoading = false;
                }
            });
        } else {
            this.editResidentMessage = 'Please fill in all required fields correctly.';
        }
    }

    confirmResidentDeletion() {
        this.closeMenu();

        this.deleteModal.title = 'Delete resident';
        this.deleteModal.item = 'resident with ID: ' + this.residentDetails!.residentId;

        const deleteFunction = new EventEmitter<void>();
        deleteFunction.subscribe(() => this.onResidentDelete());
        
        this.deleteModal.deleteConfirmed = deleteFunction;
        
        this.deleteModal.show();
    }

    onResidentDelete() {
        this.residentService.delete(this.residentDetails!.residentId).subscribe({
            next: () => {
                if(this.isResidentOfApartment(this.residentDetails!.residentId)){
                    this.apartment!.residents = this.apartment!.residents.filter(r => r.residentId !== this.residentDetails!.residentId);
                    this.apartment!.residentCount -= 1;
                }

                this.showAllResidents()
            },
            error: (err) => console.error('Error fetching data', err),
        });
    }

    onResidentApartmentAdd(residentId: number) {
        this.apartmentResidentService.add(this.apartment!.apartmentId, residentId, false).subscribe({
            next: (response) => {
                this.residentService.get(response.residentId).subscribe({
                    next: (response) => {
                        this.apartment?.residents.push(response);
                        this.apartment!.residentCount += 1;
                    },
                    error: (err) => {
                        console.error('Error fetching data', err)
                    },
                });
            },
            error: (err) => console.error('Error fetching data', err)
        });
    }

    onResidentApartmentDelete(residentId: number) {
        this.apartmentResidentService.delete(this.apartment!.apartmentId, residentId).subscribe({
            next: () => {
                this.apartment!.residents = this.apartment!.residents.filter(r => r.residentId !== residentId);
                this.apartment!.residentCount -= 1;
            },
            error: (err) => console.error('Error fetching data', err),
        });
    }

    toggleOwner(event: Event, residentId: number) {
        event.preventDefault();

        if (this.apartment?.ownerResidentId === residentId) {
            this.apartmentResidentService.update(this.apartment!.apartmentId, residentId, false).subscribe({
                next: () => {
                    this.apartment!.ownerResidentId = null;
                },
                error: (err) => console.error('Error fetching data', err),
            });
        } else {
            this.apartmentResidentService.update(this.apartment!.apartmentId, residentId, true).subscribe({
                next: () => {
                    this.apartment!.ownerResidentId = residentId;
                },
                error: (err) => console.error('Error fetching data', err),
            });
        }
    }

    isResidentOfApartment(residentId: number): boolean {
        return this.apartment!.residents.some(r => r.residentId === residentId);
    }
}