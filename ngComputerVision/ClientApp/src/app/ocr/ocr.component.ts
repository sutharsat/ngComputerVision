import { Component, OnInit } from '@angular/core';
import { ComputervisionService } from '../services/computervision.service';
import { AvailableLanguage } from '../models/availablelanguage';
import { OcrResult } from '../models/ocrresult';
import { ViewChild } from '@angular/core';
import { FormComponent } from '../Form/form.component';
import { Claim } from '../models/claim';
@Component({
  selector: 'app-ocr',
  templateUrl: './ocr.component.html',
  styleUrls: ['./ocr.component.scss']
})
export class OcrComponent implements OnInit {

  loading = false;
  imageFile: any;
  imagePreview: any;
  imageData = new FormData();
  availableLanguage: AvailableLanguage[] = [];
  DetectedTextLanguage: string = '';
  ocrResult: OcrResult;
  DefaultStatus: string;
  status: string;
  maxFileSize: number;
  isValidFile = true;
  entityData!: Claim;
  clickIndex = 0;

  

  constructor(private computervisionService: ComputervisionService, private formComponent: FormComponent) {
    this.DefaultStatus = "Maximum size allowed for the image is 4 MB";
    this.status = this.DefaultStatus;
    this.maxFileSize = 4 * 1024 * 1024; // 4MB
    this.ocrResult = new OcrResult();
  }

  ngOnInit() {
    this.computervisionService.getAvailableLanguage()
      .subscribe((result: AvailableLanguage[]) =>
        this.availableLanguage = result
      );
  }

  uploadImage(event: any) {
    this.imageFile = event.target.files[0];
    if (this.imageFile.size > this.maxFileSize) {
      this.status = `The file size is ${this.imageFile.size} bytes, this is more than the allowed limit of ${this.maxFileSize} bytes.`;
      this.isValidFile = false;
    } else if (this.imageFile.type.indexOf('image') == -1) {
      this.status = "Please upload a valid image file";
      this.isValidFile = false;
    } else {
      const reader = new FileReader();
      reader.readAsDataURL(event.target.files[0]);
      reader.onload = () => {
        this.imagePreview = reader.result;
      };
      this.status = this.DefaultStatus;
      this.isValidFile = true;
    }
  }

  async GetText(index: number) {
    
    if (this.isValidFile) {

      this.loading = true;
      this.imageData.append('imageFile', this.imageFile);
     
      this.computervisionService.getTextFromImage(this.imageData)
        .subscribe((result: OcrResult) => {
          this.ocrResult = result;

          const availableLanguageDetails = this.availableLanguage.find(x => x.languageID === this.ocrResult.language);

          if (availableLanguageDetails) {
            this.DetectedTextLanguage = availableLanguageDetails.languageName;
          } else {
            this.DetectedTextLanguage = "unknown";
          }
          this.loading = false;
        });
      await this.delay(5000);
      
      
      this.computervisionService.getClaimData(this.ocrResult.generatedId).subscribe(data => {
        this.entityData = data;
     
      });
        this.clickIndex = index;
      

      
    }
  }
 
  @ViewChild('myInput')
  myInputVariable: any;

  ClearResults() {
    this.clickIndex = 0;
    this.ocrResult.detectedText = '';
    this.imagePreview = '';
    this.myInputVariable.nativeElement.value = "";
    
    this.entityData.piiEntitiesResponse = "";
    this.entityData.healthEntitiesResponse = "";
    
  }
   delay(ms: number) {
  return new Promise(resolve => setTimeout(resolve, ms));
}
}
