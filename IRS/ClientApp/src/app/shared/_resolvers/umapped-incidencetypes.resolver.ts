import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { KeyValuePair } from 'app/_models/keyValuePair';
import { IncidenceTypeDepartmentService } from '../services/incidencetype-department.service';

@Injectable()
export class UnmappedIncidenceTypesResolver implements Resolve<KeyValuePair[]> {
    constructor(private itdService: IncidenceTypeDepartmentService, private router: Router,
        private toastr: ToastrService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<KeyValuePair[]> {
        // resolve returns and auto subscribe to observables. the param is from the Activated Route route
        return this.itdService.getUnmappedIncidenceTypes().pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/dashboard']);
                return of(null);
            })
        );
    }
}