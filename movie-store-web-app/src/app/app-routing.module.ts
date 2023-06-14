import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CustomersComponent } from './customers/customers.component';
import { MoviesComponent } from './movies/movies.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { EditMovieComponent } from './movies/edit-movie/edit-movie.component';
import { EditCustomerComponent } from './customers/edit-customer/edit-customer.component';
import { AuthGuard } from './auth/auth.guard';
import { AuthService } from './auth/auth.service';

const appRoutes: Routes = [
  { path: 'customers', component: CustomersComponent, canActivate:[AuthGuard] },
  { path: 'movies', component: MoviesComponent },
  { path: 'movies/edit/:id', component: EditMovieComponent, canActivate:[AuthGuard] },
  { path: 'movies/create', component: EditMovieComponent, canActivate:[AuthGuard] },
  { path: 'customers/edit/:id', component: EditCustomerComponent, canActivate:[AuthGuard] },
  { path: 'movies/purchase', component: MoviesComponent},
  { path: 'customers/promote', component: CustomersComponent},
  { path: '', redirectTo: '/movies', pathMatch: 'full' },
  { path: '**', component: PageNotFoundComponent },
]

@NgModule({
  imports: [
    RouterModule.forRoot(
      appRoutes
    )
  ],
  exports: [RouterModule],
  providers: [AuthGuard, AuthService]
})
export class AppRoutingModule { }
