import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RegistrationComponent } from './registration/registration.component';
import { GlobalModalsModule } from './global-modals/global-modals.module';
import { TestingComponent } from './testing/testing.component';
import { Testing2Component } from './testing2/testing2.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegistrationComponent,
    TestingComponent,
    Testing2Component
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    GlobalModalsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
