import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AvailableLanguage } from '../models/availablelanguage';
import { OcrResult } from '../models/ocrresult';
import { Claim } from '../models/claim';

@Injectable({
  providedIn: 'root'
})
export class ComputervisionService {

  baseURL: string;
  claimURL: string;
  constructor(private http: HttpClient) {
    this.baseURL = '/api/OCR';
    this.claimURL = '/api/Claim/';
  }

  getAvailableLanguage(): Observable<AvailableLanguage[]> {
    return this.http.get<AvailableLanguage[]>(this.baseURL);
  }

  getTextFromImage(image: FormData): Observable<OcrResult> {
    return this.http.post<OcrResult>(this.baseURL, image);
  }
  getClaimData(id:string): Observable<Claim> {
   /*let queryParams = new HttpParams();
    queryParams.append("id", "62f0edbcabd32e9e5086edc3");*/
    return this.http.get<Claim>(this.claimURL+id);
  }
}
