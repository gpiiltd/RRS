import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'app/shared/services/auth.service';
import { Photo } from 'app/_models/photo';
import { PhotoService } from '../services/photo.service';

@Injectable()
export class IncidencePhotosResolver implements Resolve<Photo[]> {
    constructor(private photoService: PhotoService, private router: Router,
        private toastr: ToastrService, private authService: AuthService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Photo[]> {
        // resolve returns and auto subscribe to observables. the param is from the Activated Route route
        return this.photoService.getIncidenceMedia(route.params['id']).pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/dashboard']);
                return of(null);
            })
        );
    }
}