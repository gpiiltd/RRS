import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { DepartmentService } from '../services/department.service';
import { Department } from 'app/_models/department';

@Injectable()
export class UserDepartmentsDetailsResolver implements Resolve<Department[]> {
    filter: any = {};
    constructor(private departmentService: DepartmentService, private router: Router,
        private toastr: ToastrService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Department[]> {
        // resolve returns and auto subscribe to observables. the param is from the Activated Route route
        return this.departmentService.getDepartmentDetailList(this.filter).pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/dashboard']);
                return of(null);
            })
        );
    }
}