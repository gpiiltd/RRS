import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'app/shared/services/user.service';
import { AuthService } from 'app/shared/services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class UserRolesResolver implements Resolve<any[]> {
    decodedToken: any;
    jwtHelper = new JwtHelperService();
    userToken: any;

    constructor(private userService: UserService, private router: Router,
        private toastr: ToastrService, private authService: AuthService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<any[]> {
        this.userToken = localStorage.getItem('token');
        this.decodedToken = this.jwtHelper.decodeToken(this.userToken);
        // console.log("Token data:");
        // console.log(this.userToken);
        // console.log(this.decodedToken);

        // resolve returns and auto subscribe to observables. the param is from the Activated Route route
        // get user id from token in localStorage cos there is no id when navigating to user profile from navbar.
        // decode token here as service decodedToken is lost upon refresh
        return this.userService.getUserRoles(this.decodedToken.unique_name).pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/dashboard']);
                return of(null);
            })
        );
    }
}