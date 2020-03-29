import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'app/shared/services/user.service';
import { AuthService } from 'app/shared/services/auth.service';
import { SaveUser } from 'app/_models/saveUser';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class UserViewResolver implements Resolve<SaveUser> {

    constructor(private userService: UserService, private router: Router,
        private toastr: ToastrService, private authService: AuthService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<SaveUser> {

        // resolve returns and auto subscribe to observables. the param is from the Activated Route route
        return this.userService.getUser(route.params['id']).pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/dashboard']);
                return of(null);
            })
        );
    }
}