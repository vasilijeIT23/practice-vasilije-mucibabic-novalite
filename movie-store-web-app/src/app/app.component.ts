import { Component } from '@angular/core';
import { MsalService } from '@azure/msal-angular';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HttpClient } from '@angular/common/http';
import { Customer, CustomerClient} from './api/api-reference';
import { Router } from '@angular/router';
import { AuthGuard } from './auth/auth.guard';
import { AuthService } from './auth/auth.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'movie-store-web-app';
  loginDisplay = false;
  customerEmail? = '';
  apiResponse = '';

  constructor(
    private authService : MsalService,
    private snackBar: MatSnackBar,
    private client: CustomerClient,
    private router: Router,
    private service: AuthService
    ) { }


  isAdmin(): boolean{
    if(this.service.isAuthorized())
    {
      return true
    }
    return false;
  }

  login() {
    this.authService.loginPopup()
    .subscribe({
      next: () => {
        this.setLoginDisplay();
        this.createUser();
      },
      error: (error) => console.log(error)
    });
  }

  setLoginDisplay() {
    const activateAccounts = this.authService.instance.getAllAccounts();
    this.loginDisplay = activateAccounts.length > 0;
    if(this.loginDisplay){
      this.authService.instance.setActiveAccount(activateAccounts[0]);
    }
    this.customerEmail = activateAccounts[0].username;
  }

  logout(){
    this.authService.logoutPopup({
      mainWindowRedirectUri: "/movies/",
    }).subscribe(_ => this.setLoginDisplay());
  }

  createUser(){
    this.client.create().subscribe();
  }
  createMovie() {
    this.router.navigate([`/movies/create/`]);
}  
 }

