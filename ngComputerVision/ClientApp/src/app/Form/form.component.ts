import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { Claim } from '../models/claim';
import { ComputervisionService } from '../services/computervision.service';
import { OcrResult } from '../models/ocrresult';
import { OcrComponent } from '../ocr/ocr.component';
import { MatSelectChange } from '@angular/material/select';
import { MouseHover } from '../models/mouseHover';



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
  mouseHoverData !:MouseHover;
  
 claimId: string = '';

  constructor( private formBuilder: FormBuilder, private claimService: ComputervisionService) {
    this.mouseHoverData = new MouseHover();
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
  function1(name: string, flag: boolean) {
    this.mouseHoverData.name = name;
    this.mouseHoverData.isHover = flag;
    this.claimService.formHover(this.mouseHoverData);
    
  }
  

  
}
