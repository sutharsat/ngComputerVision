import { EventEmitter, Injectable, Output } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { AvailableLanguage } from '../models/availablelanguage';
import { OcrResult } from '../models/ocrresult';
import { Claim } from '../models/claim';
import { MouseHover } from '../models/mouseHover';
import { SearchValue } from '../models/SearchValue';

@Injectable({
  providedIn: 'root'
})
export class ComputervisionService {

  baseURL: string;
  saveURL: string;
  claimURL: string;
  constructor(private http: HttpClient) {
    this.baseURL = '/api/OCR';
    this.claimURL = '/api/Claim/';
    this.saveURL= '/api/SaveClaim'
  }
  @Output() formHoverEvent = new EventEmitter<MouseHover>();
  @Output() formSearchValueSendEvent = new EventEmitter<string>();
  @Output() isCheckedEvent = new EventEmitter<Boolean>();
  private searchValueSubject = new Subject<any>();
  
  getAvailableLanguage(): Observable<AvailableLanguage[]> {
    return this.http.get<AvailableLanguage[]>(this.baseURL);
  }

  getTextFromImage(image: FormData): Observable<OcrResult> {
    return this.http.post<OcrResult>(this.baseURL, image);
  }
  getClaimData(id:string): Observable<Claim> {
   
    return this.http.get<Claim>(this.claimURL+id);
  }
  formHover(msg: MouseHover) {
    this.formHoverEvent.emit(msg);
  }
  isCheckBoxTrue(flag: boolean) {
    this.isCheckedEvent.emit(flag);
  }
  addSearchDetails(claimDetails: any) {
    
    return this.http.post(this.saveURL, claimDetails);
  }
  submitButton(submitting: boolean): void {
    console.log("service1");
    this.searchValueSubject.next(submitting);
  }
  onFormSubmit(): Observable<any> {
    console.log("service2");
    return this.searchValueSubject.asObservable();
  }

}
