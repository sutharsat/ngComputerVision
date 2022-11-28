
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  domainUrl = 'http://localhost:4000';

  constructor(private http: HttpClient) { }
  search(person: any): Observable<Product[]> {
    return this.http.post<Product[]>(`${this.domainUrl}/api/product/search`, person);
}
//getAllProduct(){
//  return this.http.get(this.domainUrl + '/api/product');
//}

//addProduct(ptoduct: any){
//  return this.http.post(this.domainUrl + '/api/product', ptoduct);
//}

//getProductById(pid: any) {
//  return this.http.get(this.domainUrl + '/api/getproduct/' + pid);
//}
}

export interface Product {
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
