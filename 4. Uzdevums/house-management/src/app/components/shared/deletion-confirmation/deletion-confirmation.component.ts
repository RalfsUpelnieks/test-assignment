import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PopupComponent } from '../popup/popup.component';

@Component({
  selector: 'app-deletion-confirmation',
  standalone: true,
  imports: [CommonModule, PopupComponent],
  templateUrl: './deletion-confirmation.component.html',
  styleUrl: './deletion-confirmation.component.css'
})
export class DeletionConfirmationComponent {
    @Input() title: string = 'Delete Item';
    @Input() item: string = 'this item';
    @Output() deleteConfirmed = new EventEmitter<void>();
    isVisible = false;

    show() {
        this.isVisible = true;
    }
    
    close() {
        this.isVisible = false;
    }
    
    confirmDelete() {
        this.deleteConfirmed.emit();
        this.close();
    }
}
