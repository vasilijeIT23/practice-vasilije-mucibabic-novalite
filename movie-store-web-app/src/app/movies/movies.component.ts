import { Component, OnInit } from '@angular/core';
//import { MatTableModule } from '@angular/material/table';
import { MovieClient, Movie, DeleteMovieCommand, CustomerClient} from '../api/api-reference';
import { ActivatedRoute, Router } from '@angular/router';
import { MsalService } from '@azure/msal-angular';

@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrls: ['./movies.component.css']
})
export class MoviesComponent implements OnInit{
  
  displayedColumns: string[] = ['id', 'title', 'licencingType', 'actions'];

  movies: Movie[] = [];

  constructor(private route: ActivatedRoute,
    private movieClient: MovieClient, 
    private customerClient: CustomerClient, 
    private router: Router, 
    private authService : MsalService,) { }

  ngOnInit() {
    this.movieClient.getAll().subscribe(result => {
      this.movies = result;
    });
  }

  onUpdate(movie: Movie) {
    this.router.navigate([`/movies/edit/${movie.id}`]);
  }

  onDelete(query: DeleteMovieCommand) {
    this.movieClient.delete(query).subscribe(_ => {
      this.movies = this.movies.filter(x => x.id !== query.id)
    })
  }
  onPurchase(movieId: string){
    this.router.navigate([`/movies/purchase`]);
    this.customerClient.purchaseMovie(movieId).subscribe()
    this.router.navigate([`/customers/promote`]);
    this.customerClient.promote()
  }

}