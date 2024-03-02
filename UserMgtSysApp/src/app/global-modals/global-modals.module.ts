import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OkOnlyComponent } from './ok-only/ok-only.component';


// Creating new component inside Module
//npx ng g c Global-Modals/OkOnly
@NgModule({
  declarations: [
    OkOnlyComponent
  ],
  imports: [
    CommonModule
  ]
})
export class GlobalModalsModule { }
