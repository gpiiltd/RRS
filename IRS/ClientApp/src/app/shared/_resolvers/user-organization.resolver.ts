import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { KeyValuePair } from 'app/_models/keyValuePair';
import { DepartmentService } from '../services/department.service';
import { OrganizationService } from '../services/organization.service';

@Injectable()
export class UserOrganizationsResolver implements Resolve<any[]> {
    constructor(private organizationService: OrganizationService, private router: Router,
        private toastr: ToastrService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<any[]> {
        // resolve returns and auto subscribe to observables. the param is from the Activated Route route
        return this.organizationService.getOrganizations().pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/dashboard']);
                return of(null);
            })
        );
    }
}