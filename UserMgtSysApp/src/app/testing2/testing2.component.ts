import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { Modal } from 'bootstrap';

@Component({
  selector: 'app-testing2',
  templateUrl: './testing2.component.html',
  styleUrls: ['./testing2.component.css']
})
export class Testing2Component implements AfterViewInit {

  title = 'UserMgtSysApp';
  @ViewChild('modal1') modal1!: ElementRef; // ViewChild for modal1
  @ViewChild('modal2') modal2!: ElementRef; // ViewChild for modal2
  modalDialogs: { [key: string]: Modal } = {};

  ngAfterViewInit() {
    this.modalDialogs['modal1'] = new Modal(this.modal1.nativeElement);
    this.modalDialogs['modal2'] = new Modal(this.modal2.nativeElement);
  }
  getAllUsers(modalRef: HTMLDivElement) {
    this.modalDialogs['modal2'].show();
  }
  showModal(modalRef: HTMLDivElement) {
    this.modalDialogs['modal1'].show();
  }
  closeModal(modalId: string) {
    if (modalId != null && modalId != "") {
      const modal = this.modalDialogs[modalId];
      if (modal) {
        modal.hide();
      }
    }
  }
}
