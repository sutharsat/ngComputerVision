import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { OcrComponent } from './ocr/ocr.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { SigninComponent } from './components/signin/signin.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AngularMaterialModule } from './angular-material.module';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { FormComponent } from './Form/form.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';

@NgModule({
  declarations: [
    AppComponent,
    OcrComponent,
    NavMenuComponent,
    HomeComponent,
    SigninComponent,
    FormComponent
  
    
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule, 
    AngularMaterialModule,
    FormsModule,
    MatAutocompleteModule,
    MDBBootstrapModule.forRoot(),

    
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'computer-vision-ocr', component: OcrComponent },
      { path: 'form', component: FormComponent },
      
    ], { relativeLinkResolution: 'legacy' }),
    BrowserAnimationsModule
  ],

  providers: [FormComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
