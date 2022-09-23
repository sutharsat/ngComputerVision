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
export class FormComponent {
  contactForm!: FormGroup;

 /* countries = [
    { id: 1, name: "United States" },
    { id: 2, name: "Australia" },
    { id: 3, name: "Canada" },
    { id: 4, name: "Brazil" },
    { id: 5, name: "England" }
  ];*/
  //formGroup: any;
  titleAlert: string = 'This field is required';
  post: any = '';
 @Input() claimData!: Claim;
 claimId: string = '';

  constructor( private formBuilder: FormBuilder, private claimService: ComputervisionService) {
   
  }

  ngOnInit() {
   

  }

  getClaimsDetailsForForm() {
   /* this.claimId = this.ocrComponent.ocrResult.generatedId;
    console.log(this.claimId);
     this.claimService.getClaimData(this.claimId).subscribe(data => {
       this.claimData = data;
    });*/
   // this.claimData = this.ocrComponent.entityData;

    
    
  }

 /* createForm() {
    this.formGroup = this.formBuilder.group({
      'email': [],
      'firstname': [null],
      'lastName': [],
      'medicareID': [],
      'dateOfBirth': ''
    });
  }*/

  

 /* get name() {
    return this.formGroup.get('name') as FormControl
  }*/
onSubmit(post:any) {
    this.post = post;
  }
  onEdit(post: any) {
    this.post = post;
  }

}
