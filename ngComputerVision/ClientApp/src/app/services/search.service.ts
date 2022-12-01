
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  domainUrl = 'http://localhost:4000';

  constructor(private http: HttpClient) { }
  search(person: any): Observable<Search[]> {
    return this.http.post<Search[]>(`${this.domainUrl}/api/search/search`, person);
}

}

export interface Search {
  _id:any;
  person: string;
  organization: string;
  address: string;
  phoneNumber: string;
  email: string;
  dateTime: string;
 //Image = string;
  claimId: string
}
