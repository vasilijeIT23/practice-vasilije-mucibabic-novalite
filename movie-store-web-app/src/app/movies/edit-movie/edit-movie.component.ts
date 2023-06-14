import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { UpdateMovieCommand, MovieClient, LicencingType, Movie, CreateMovieCommand } from 'src/app/api/api-reference';

@Component({
    selector: 'app-edit-movie',
    templateUrl: './edit-movie.component.html',
    styleUrls: ['./edit-movie.component.css']
})

export class EditMovieComponent implements OnInit {
    myMap = new Map<LicencingType, string>([
        [LicencingType.TwoDay, 'Two day'],
        [LicencingType.LifeLong, 'Lifelong']
    ]);
    movieId: string | undefined;
    formGroup = new FormGroup({
        id: new FormControl('', { nonNullable: true }),
        title: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
        licencingType: new FormControl<LicencingType>(LicencingType.TwoDay, { nonNullable: true }),
    });

    constructor(private route: ActivatedRoute,
        private client: MovieClient,
        private router: Router,
        private readonly snackBar: MatSnackBar) {
    }

    ngOnInit() {
        this.movieId = this.route.snapshot.paramMap.get('id') ?? undefined;
        if (this.movieId) {
            this.client.getById(this.movieId).subscribe(data => this.patchForm(data));
        }
    }

    onSubmit() {
        if (this.movieId) {
            this.client.update(new UpdateMovieCommand({
                id: this.formGroup.controls.id.value,
                title: this.formGroup.controls.title.value,
                licencingType: this.formGroup.controls.licencingType.value
            })).subscribe(_ => {
                this.snackBar.open('Movie updated');
                this.router.navigate([`/movies/`]);
            });
        } else {
            this.client.create(new CreateMovieCommand({
                title: this.formGroup.controls.title.value,
                licencingType: this.formGroup.controls.licencingType.value
            })).subscribe(_ => {
                this.snackBar.open('Movie created');
                this.router.navigate([`/movies/`]);
            });
        }
    }  

    private readonly patchForm = (movie: Movie) => {
        this.formGroup.controls.id.patchValue(movie.id!);
        this.formGroup.controls.title.patchValue(movie.title!);
        this.formGroup.controls.licencingType.patchValue(movie.licencingType!);
    }
}
