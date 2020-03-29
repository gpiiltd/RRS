import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../../_models/user';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'app/shared/services/user.service';
import { AuthService } from 'app/shared/services/auth.service';
import { UsersRoles } from 'app/_models/usersRoles';

@Injectable()
export class UsersRolesDetailListResolver implements Resolve<UsersRoles[]> {
    filter: any = {page: 1, pageSize: 50};
    constructor(private userService: UserService, private router: Router,
        private toastr: ToastrService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<UsersRoles[]> {
        return this.userService.getUsersWithRoles(this.filter).pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/dashboard']);
                return of(null);
            })
        );
    }
}