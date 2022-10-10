import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { Claim } from '../models/claim';
import { ComputervisionService } from '../services/computervision.service';
import { OcrResult } from '../models/ocrresult';
import { OcrComponent } from '../ocr/ocr.component';
import { MatSelectChange } from '@angular/material/select';


@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.scss']
})
export class FormComponent implements OnInit {
  contactForm!: FormGroup;
  PIIEntitiesResponse: any;
  
  selectedData:any ;
  titleAlert: string = 'This field is required';
  post: any = '';
  @Input() claimData!: Claim;
  @Input() text!: OcrResult;
  public PIIResponseData: any = null;
  public healthResponseData: any = null;
  
 claimId: string = '';

  constructor( private formBuilder: FormBuilder, private claimService: ComputervisionService) {
   
  }

  ngOnInit() {
    this.PIIResponseData = this.claimData.piiEntitiesResponse;
    this.healthResponseData = this.claimData.healthEntitiesResponse;
   

  }
  onChange(deviceValue:number) {
    
  }

  getClaimsDetailsForForm() {
   }

 onSubmit(post:any) {
    this.post = post;
  }
  onEdit(post: any) {
    this.post = post;
  }
  selectedValue(event: any) {
    this.selectedData = event.source.triggerValue;
     
     
      
    
  }

}
