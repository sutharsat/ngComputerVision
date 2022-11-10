export class SearchValue {
  person: string;
  organization: string;
  address: string;
  phoneNumber: string;
  email: string;
  dateTime: string;
  searchImageValue = new FormData();
  claimId: string
  

  constructor() {
    this.person = '';
    this.organization = '';
    this.address = '';
    this.phoneNumber = '';
    this.email = '';
    this.dateTime = '';
    this.claimId = '';
    
    
  }
 
}
