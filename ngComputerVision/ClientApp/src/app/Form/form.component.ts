import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { Claim } from '../models/claim';
import { ComputervisionService } from '../services/computervision.service';
import { OcrResult } from '../models/ocrresult';
import { OcrComponent } from '../ocr/ocr.component';


@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.scss']
})
export class FormComponent implements OnInit {
  contactForm!: FormGroup;
  PIIEntitiesResponse: any;
  cities = {};
  selectedCountry: any;
 countries = [
   { id: 1, name: "United States", cities: ['Paris', 'Marseille', 'Nice'] },
   { id: 2, name: "Australia", cities: ['Paris', 'Marseille', 'Nice'] },
   { id: 3, name: "Canada", cities: ['Paris', 'Marseille', 'Nice'] },
   { id: 4, name: "Brazil", cities: ['Paris', 'Marseille', 'Nice'] },
   { id: 5, name: "England", cities: ['Paris', 'Marseille', 'Nice'] }
  ];
  //formGroup: any;
  titleAlert: string = 'This field is required';
  post: any = '';
  @Input() claimData!: Claim;
  public PIIResponseData: any = null;
  //PIIEntitiesResponse: any = null;
 claimId: string = '';

  constructor( private formBuilder: FormBuilder, private claimService: ComputervisionService) {
   
  }

  ngOnInit() {
    this.PIIResponseData = this.claimData.piiEntitiesResponse;
    this.cities = this.countries.filter(x => x.id == 1)[0].cities;

  }
  onChange(deviceValue:number) {
    this.cities = this.countries.filter(x => x.id == deviceValue)[0].cities;
  }

  getClaimsDetailsForForm() {
   }

 onSubmit(post:any) {
    this.post = post;
  }
  onEdit(post: any) {
    this.post = post;
  }

}
