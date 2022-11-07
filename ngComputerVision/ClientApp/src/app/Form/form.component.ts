import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';
import { Claim } from '../models/claim';
import { ComputervisionService } from '../services/computervision.service';
import { OcrResult } from '../models/ocrresult';
import { OcrComponent } from '../ocr/ocr.component';
import { MatSelectChange } from '@angular/material/select';
import { MouseHover } from '../models/mouseHover';
import { SearchValue } from '../models/SearchValue';



@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.scss']
})
export class FormComponent implements OnInit {
  contactForm!: FormGroup;
  PIIEntitiesResponse: any;
  searchForm: FormGroup;
  selectedData:any ;
  titleAlert: string = 'This field is required';
  post: any = '';
 
  @Input() text!: OcrResult;
  @Input() claimData!: Claim;
  public PIIResponseData: any = null;
  public healthResponseData: any = null;
  mouseHoverData !:MouseHover;
  value: any;
  claimId: string = '';
  person: string = '';
  getAllFormData: any;
  searchValue !: SearchValue;
  @Output() selected = new EventEmitter();
  @Output() searchFormEvent = new EventEmitter<string>();
  //@Output() submit = new EventEmitter();
  constructor(private fb: FormBuilder, private formBuilder: FormBuilder, private claimService: ComputervisionService) {
    this.mouseHoverData = new MouseHover();
    this.searchValue = new SearchValue();
    this.searchForm = fb.group({

      person: ['']


    });
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
 // @Output() submitFormClicked = new EventEmitter<any>();

  submitForm() {
    this.searchForm.patchValue({
      person: this.searchValue.person
    });
    this.claimService.addSearchDetails(this.searchForm.value).subscribe(data => {
      

        this.searchForm.reset();
      
    });

  }


  checkConfidenceScorePn(): boolean {
    let cs:boolean = false;
    for (var i = 0; i < this.claimData.piiEntitiesResponse.length; i++) {
      if (this.claimData.piiEntitiesResponse[i].category =="PhoneNumber" && this.claimData.piiEntitiesResponse[i].confidenceScore < 0.8) {
        
        cs = true;
        break;
      }

    }
    return cs;
  }
  checkConfidenceScorePerson(): boolean {
    let cs: boolean = false;
    for (var i = 0; i < this.claimData.piiEntitiesResponse.length; i++) {
      if (this.claimData.piiEntitiesResponse[i].category == "Person" && this.claimData.piiEntitiesResponse[i].confidenceScore < 0.8) {

        cs = true;
        break;
      }

    }
    return cs;
  }
  checkConfidenceScoreAdd(): boolean {
    let cs: boolean = false;
    for (var i = 0; i < this.claimData.piiEntitiesResponse.length; i++) {
      if (this.claimData.piiEntitiesResponse[i].category == "Address" && this.claimData.piiEntitiesResponse[i].confidenceScore < 0.8) {

        cs = true;
        break;
      }

    }
    return cs;
  }
  checkConfidenceScoreEm(): boolean {
    let cs: boolean = false;
    for (var i = 0; i < this.claimData.piiEntitiesResponse.length; i++) {
      if (this.claimData.piiEntitiesResponse[i].category == "Email" && this.claimData.piiEntitiesResponse[i].confidenceScore < 0.8) {

        cs = true;
        break;
      }

    }
    return cs;
  }
  checkConfidenceScoreDof(): boolean {
    let cs: boolean = false;
    for (var i = 0; i < this.claimData.piiEntitiesResponse.length; i++) {
      if (this.claimData.piiEntitiesResponse[i].category == "DateTime" && this.claimData.piiEntitiesResponse[i].confidenceScore < 0.8) {

        cs = true;
        break;
      }

    }
    return cs;
  }
  checkConfidenceScoreOrg(): boolean {
    let cs: boolean = false;
    for (var i = 0; i < this.claimData.piiEntitiesResponse.length; i++) {
      if (this.claimData.piiEntitiesResponse[i].category == "Organization" && this.claimData.piiEntitiesResponse[i].confidenceScore < 0.8) {

        cs = true;
        break;
      }

    }
    return cs;
  }
  checkConfidenceScoreTName(): boolean {
    let cs: boolean = false;
    for (var i = 0; i < this.claimData.healthEntitiesResponse.length; i++) {
      if (this.claimData.healthEntitiesResponse[i].category == "TreatmentName" && this.claimData.healthEntitiesResponse[i].confidenceScore < 0.8) {

        cs = true;
        break;
      }

    }
    return cs;
  }
  checkConfidenceScoreHealthProf(): boolean {
    let cs: boolean = false;
    for (var i = 0; i < this.claimData.healthEntitiesResponse.length; i++) {
      if (this.claimData.healthEntitiesResponse[i].category == "HealthcareProfession" && this.claimData.healthEntitiesResponse[i].confidenceScore < 0.8) {

        cs = true;
        break;
      }

    }
    return cs;
  }
  checkConfidenceScoreExamName(): boolean {
    let cs: boolean = false;
    for (var i = 0; i < this.claimData.healthEntitiesResponse.length; i++) {
      if (this.claimData.healthEntitiesResponse[i].category == "ExaminationName" && this.claimData.healthEntitiesResponse[i].confidenceScore < 0.8) {

        cs = true;
        break;
      }

    }
    return cs;
  }
  checkConfidenceScoreAdmnEvent(): boolean {
    let cs: boolean = false;
    for (var i = 0; i < this.claimData.healthEntitiesResponse.length; i++) {
      if (this.claimData.healthEntitiesResponse[i].category == "AdministrativeEvent" && this.claimData.healthEntitiesResponse[i].confidenceScore < 0.8) {

        cs = true;
        break;
      }

    }
    return cs;
  }
  checkConfidenceScoreCareEnv(): boolean {
    let cs: boolean = false;
    for (var i = 0; i < this.claimData.healthEntitiesResponse.length; i++) {
      if (this.claimData.healthEntitiesResponse[i].category == "CareEnvironment" && this.claimData.healthEntitiesResponse[i].confidenceScore < 0.8) {

        cs = true;
        break;
      }

    }
    return cs;
  }
  checkConfidenceScoreGen(): boolean {
    let cs: boolean = false;
    for (var i = 0; i < this.claimData.healthEntitiesResponse.length; i++) {
      if (this.claimData.healthEntitiesResponse[i].category == "Gender" && this.claimData.healthEntitiesResponse[i].confidenceScore < 0.8) {

        cs = true;
        break;
      }

    }
    return cs;
  }
  checkConfidenceScoreTreatmentDate(): boolean {
    let cs: boolean = false;
    for (var i = 0; i < this.claimData.healthEntitiesResponse.length; i++) {
      if (this.claimData.healthEntitiesResponse[i].category == "Date" && this.claimData.healthEntitiesResponse[i].confidenceScore < 0.8) {

        cs = true;
        break;
      }

    }
    return cs;
  }
  
  

  piiCheckBox(event: any) {
   if(event.target.checked)
      this.claimService.isCheckBoxTrue(true);
    else
     this.claimService.isCheckBoxTrue(false);
    

  }
  //sendSearch(event: any) {
  //  if (event.value != null) {
  //    this.person = event.value;
    
  //    this.claimService.sendSearchValue(this.person);
  //  } 
  //}
}
