import { Component, ElementRef, ViewChild } from '@angular/core';
import { Modal } from 'bootstrap';

@Component({
  selector: 'app-testing',
  templateUrl: './testing.component.html',
  styleUrls: ['./testing.component.css']
})
export class TestingComponent {
  @ViewChild('btnGetAllUsers') btnGetAllUsers!: ElementRef;
  @ViewChild('modal2') modal2!: ElementRef;

  title = 'UserMgtSysApp';
  modal!: Modal;

  ngAfterViewInit() {
    // Adding event listener to the button element
    this.btnGetAllUsers.nativeElement.addEventListener('click', () => this.showModal(this.modal2.nativeElement));
    console.log("this.btnGetAllUsers", this.btnGetAllUsers.nativeElement.eventListeners());
  }
  getAllUsers(modalRef: HTMLDivElement){
    console.log("getAllUsers is called");
  }
  showModal(modalRef: HTMLDivElement) {
    console.log("showModal is called")
    this.modal = new Modal(modalRef);
    this.modal.show();
  }
  closeModal() {
    this.modal.hide();
  }
}
