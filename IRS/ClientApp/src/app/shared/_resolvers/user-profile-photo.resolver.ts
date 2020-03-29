import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'app/shared/services/auth.service';
import { Photo } from 'app/_models/photo';
import { PhotoService } from '../services/photo.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class UserProfilePhotoResolver implements Resolve<Photo> {
    decodedToken: any;
    jwtHelper = new JwtHelperService();
    userToken: any;

    constructor(private photoService: PhotoService, private router: Router,
        private toastr: ToastrService, private authService: AuthService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Photo> {
        this.userToken = localStorage.getItem('token');
        this.decodedToken = this.jwtHelper.decodeToken(this.userToken);

        // resolve returns and auto subscribe to observables. the param is from the Activated Route route
        return this.photoService.getUserProfilePhoto(this.decodedToken.nameid).pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/profile/edit/' + this.decodedToken.nameid]);
                return of(null);
            })
        );
    }
}