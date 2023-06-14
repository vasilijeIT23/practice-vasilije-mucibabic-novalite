import { Injectable } from '@angular/core';
import { MsalService } from '@azure/msal-angular';
import { Customer, CustomerClient, Role } from '../api/api-reference';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  isAuth = false;
  email: string | undefined;
  role: Role | undefined;
  customers: Customer[] = [];
  customer: Customer | undefined;

  constructor(private authService : MsalService, private client: CustomerClient) { }

  isAuthorized() {
    const activateAccounts = this.authService.instance.getAllAccounts();
    this.email = activateAccounts[0].idTokenClaims?.preferred_username;

    this.createUser();
    
    if(this.customer != null)
    {
      this.role = this.customer.role;
      if(this.role == 2)
      {
        this.isAuth = true;
        return this.isAuth;
      }
    }
    return this.isAuth;
  }

  createUser(){
    this.client.create().subscribe(data => {
      this.customer = data;
    })
  }
}
