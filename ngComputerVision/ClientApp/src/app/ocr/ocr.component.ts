import { Component, ElementRef, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ComputervisionService } from '../services/computervision.service';
import { ProductService } from '../services/product.service';
import { AvailableLanguage } from '../models/availablelanguage';
import { OcrResult } from '../models/ocrresult';
import { ViewChild } from '@angular/core';
import { FormComponent } from '../Form/form.component';
import { FormControl,FormBuilder,  Validators } from '@angular/forms';
import { Observable  } from 'rxjs';
import { Claim } from '../models/claim';
import { MouseHover } from '../models/mouseHover';
import { SearchValue } from '../models/SearchValue';
import { FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';
import { debounceTime, map, startWith, switchMap, tap, finalize } from 'rxjs/operators';
import { MatSelect } from '@angular/material/select';
@Component({
  selector: 'app-ocr',
  templateUrl: './ocr.component.html',
  styleUrls: ['./ocr.component.scss']
})
export class OcrComponent implements OnInit {
 // hideRequiredMarker: boolean;
  ProductForm: FormGroup 
    searchForm: FormGroup ;
  myControl = new FormControl();
  productOptions: Product[]=[];
 isLoading: boolean;
  productObj: any;
  isProduct: boolean;
  loading = false;
  imageFile: any;
  imagePreview: any;
  Search: any;
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
 
  

 
  drawItems: any[] = []
  
  boundingBoxValues: any[] = []
  @Input('ImageHeight') ImageHeight = 0
  @Input('ImageWidth') ImageWidth = 0
  
  taggedItem = ""
  showInput: boolean = false;
  isMoving: boolean = false;
 
  public uniX: number | undefined;
  public uniY: number | undefined;
  public uniX2!: number;
  public uniY2!: number;
  public initX!: number;
  public initY!: number;
 
  public url!: string;
  public image: any;
  hoverName: string = '';
  isHovered: boolean = false;
  isChecked: boolean = false;
 
  @Input() formData!: SearchValue;
  person: string = '';
  submitServiceSubscription: Subscription = new Subscription;
  @ViewChild("layer1", { static: false }) layer1Canvas!: ElementRef;
  private context!: CanvasRenderingContext2D;
  private layer1CanvasElement: any;
    //searchForm: any;
    searchValue: any;



  constructor(private computervisionService: ComputervisionService, private formComponent: FormComponent, private productService: ProductService, private fb: FormBuilder) {
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
    this.computervisionService.formHoverEvent
      .subscribe((data: MouseHover) => {
        console.log('Event message from FormComponent : ' + data);
        this.hoverName = data.name;
        this.isHovered = data.isHover;
        
        this.showImage(this.entityData);
      });

    this.computervisionService.isCheckedEvent
      .subscribe((data: boolean) => {
        console.log('Event ststus from checkedBox : ' + data);
        this.isChecked = data;
        

        this.showImage(this.entityData);
      });
    
    //this.submitServiceSubscription = this.computervisionService.onFormSubmit().subscribe(
    //  (submitting) => {
    //    console.log("approve button is clicked");
    //    if (submitting) {
    //      console.log("data ready for DB call");

    //    }
    //  });
    //this.searchOptions = this.myControl.valueChanges
    //  .pipe(
    //    startWith(''),
    //    map((value: string) => this._filter(value))
    //  );
    
    this.searchForm = this.fb.group({
      serchInput: null
    });
    this.searchForm
      .get('serchInput')
      .valueChanges
      .pipe(
        debounceTime(300),
        tap(() => this.isLoading = true),
        switchMap(value => this.productService.search({ person: value })
          .pipe(
            finalize(() => this.isLoading = false),
          )
        )
      )
      .subscribe((product: any) => this.productOptions = product);
    this.ProductForm = this.fb.group({
      _id: [null],
      person: [''],
      phoneNumber: [''],
      email: ['']

      })
  }
  

  //private _filter(value: string): string[] {
  //  const filterValue = value.toLowerCase();

  //  return this.options.filter(option => option.toLowerCase().includes(filterValue));
  //}

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
        this.image = new Image();
        this.image.src = reader.result;
        this.imagePreview = reader.result;
        this.image.onload = () => {
         
          this.ImageWidth = this.image.width;
          this.ImageHeight = this.image.height;
          
          // this.showImage();
          this.layer1CanvasElement = this.layer1Canvas.nativeElement;
          this.context = this.layer1CanvasElement.getContext("2d");
          this.layer1CanvasElement.width = this.ImageWidth;
          this.layer1CanvasElement.height = this.ImageHeight;
          this.context.drawImage(this.image, 0, 0, this.ImageWidth, this.ImageHeight);

        };
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
    this.isHovered = false;
    this.isChecked = false;
    this.ocrResult.detectedText = '';
    this.imagePreview = '';
    this.myInputVariable.nativeElement.value = "";

    this.entityData.piiEntitiesResponse = "";
    this.entityData.healthEntitiesResponse = "";
    this.image = '';
    this.layer1Canvas.nativeElement.value = "";
    this.drawItems = [];
    this.context.clearRect(0, 0, this.layer1CanvasElement.width, this.layer1CanvasElement.height);
    
    this.searchForm
      .get('serchInput').setValue('');
  }
  delay(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
  showImage(entityData: Claim) {
   
    let parent = this;
    for (var i = 0; i < this.entityData.piiEntitiesResponse.length; i++) {
     
      if (this.entityData.piiEntitiesResponse[i].boundingBox != null) {
        
          this.initX = this.entityData.piiEntitiesResponse[i].boundingBox[0];
          this.initY = this.entityData.piiEntitiesResponse[i].boundingBox[1];
          this.uniX = this.entityData.piiEntitiesResponse[i].boundingBox[4] - this.entityData.piiEntitiesResponse[i].boundingBox[6];
          this.uniY = this.entityData.piiEntitiesResponse[i].boundingBox[5] - this.entityData.piiEntitiesResponse[i].boundingBox[3];
          this.isMoving = false
          this.showInput = true
          this.drawItems.push({
            name:this.entityData.piiEntitiesResponse[i].text,
            x0: this.initX,
            y0: this.initY,
            x1: this.uniX,
            y1: this.uniY
          });
          parent.drawRect("red", this.initX, this.initY, 0);
        }
       
    }
    for (var i = 0; i < this.entityData.healthEntitiesResponse.length; i++) {
      if (this.entityData.healthEntitiesResponse[i].boundingBox != null) {
        this.initX = this.entityData.healthEntitiesResponse[i].boundingBox[0];
        this.initY = this.entityData.healthEntitiesResponse[i].boundingBox[1];
        this.uniX = this.entityData.healthEntitiesResponse[i].boundingBox[4] - this.entityData.healthEntitiesResponse[i].boundingBox[6];
        this.uniY = this.entityData.healthEntitiesResponse[i].boundingBox[5] - this.entityData.healthEntitiesResponse[i].boundingBox[3];
        this.isMoving = false
        this.showInput = true
        this.drawItems.push({
          name: this.entityData.healthEntitiesResponse[i].text,
          x0: this.initX,
          y0: this.initY,
          x1: this.uniX,
          y1: this.uniY
        });
        parent.drawRect("red", this.initX, this.initY, 0);
      }
    }
    this.drawItems = [];

  }
  drawRect(color = "black", height: number, width: number, flag: number) {
    if (flag) {
      this.context.drawImage(this.image, 0, 0, this.ImageWidth, this.ImageHeight);
    }
    this.uniX = height
    this.uniY = width
    this.uniX2 = height
    this.uniY2 = width
    for (var i = 0; i < this.drawItems.length; i++) {
      if (this.drawItems[i].name === this.hoverName && this.isHovered) {
        this.context.beginPath();
        this.context.rect(
          this.drawItems[i].x0,
          this.drawItems[i].y0,
          this.drawItems[i].x1,
          this.drawItems[i].y1
        );
        this.context.lineWidth = 2;
        this.context.strokeStyle = color;
        this.context.stroke();

      }
      else if (this.drawItems[i].name === this.hoverName && !this.isHovered) {

        this.context.drawImage(this.image, 0, 0, this.ImageWidth, this.ImageHeight);


      }
    }
    for (var i = 0; i < this.drawItems.length; i++) {
       if (this.isChecked && !this.isHovered) {

        this.context.beginPath();
        this.context.rect(
          this.drawItems[i].x0,
          this.drawItems[i].y0,
          this.drawItems[i].x1,
          this.drawItems[i].y1
        );
        this.context.lineWidth = 2;
        this.context.strokeStyle = color;
        this.context.stroke();
        
      }
      else if (!this.isChecked && !this.isHovered) {
        this.context.drawImage(this.image, 0, 0, this.ImageWidth, this.ImageHeight);
        
      }
    }
   

    
  }
  
  setValue(event: any) {
    console.log(event.value);
    this.searchValue = event;
    console.log("parentForm");
    console.log(this.searchValue);
  }
  saveData() {
    //alert("dataSave");
    //this.searchValue.searchImageValue = this.imageData;
    const formData: FormData = new FormData();
    

    this.searchValue.claimId = this.ocrResult.generatedId;
    var obj = JSON.stringify(this.searchValue);
    formData.set("data", obj);
    formData.append('imageFile', this.imageFile);
      this.computervisionService.addSearchDetails(formData).subscribe(data => {


       // this.searchForm.reset();
        alert(data);
       
      });

    

    
  }
  getSelectedProduct(val: any) {
    
    console.log(val)
    this.computervisionService.getSearchImage(val)
      .subscribe((result) => {
       // const reader = new FileReader();
        let imageURL = 'data:image/png;base64,' + result;
        //reader.readAsDataURL(imageURL);
       // reader.readAsDataURL(new Blob([result], {type:'image/png'}));
       // reader.onload = () => { 
         // this.image = e.target.result;
          this.image = new Image();
          this.image.src = imageURL;
          this.imagePreview = imageURL;
          this.image.onload = () => {

            this.ImageWidth = this.image.width;
            this.ImageHeight = this.image.height;

            // this.showImage();
            this.layer1CanvasElement = this.layer1Canvas.nativeElement;
            this.context = this.layer1CanvasElement.getContext("2d");
            this.layer1CanvasElement.width = this.ImageWidth;
            this.layer1CanvasElement.height = this.ImageHeight;
            this.context.drawImage(this.image, 0, 0, this.ImageWidth, this.ImageHeight);

          };
       // };
        
        
      });
    this.status = this.DefaultStatus;
    this.isValidFile = true;
    this.computervisionService.getClaimData(val).subscribe(data => {
      this.entityData = data;
      this.ocrResult.detectedText = this.entityData.ocrText;


    });
    this.clickIndex = 2;
    //if (val) {
    //  this.productService.getProductById(val).subscribe(result => {
    //    console.log(result);
    //    this.productObj = result;
    //    this.isProduct = true;
    //    this.searchForm.reset();
    //  });
    //}
  }
}
export interface Product {
  _id: any;
  person: string;
  organization: string;
  address: string;
  phoneNumber: string;
  email: string;
  dateTime: string;
  //Image = string;
  claimId: string

}

