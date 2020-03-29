import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { IncidenceStatusService } from '../services/incidence-status.service';
import { KeyValuePair } from 'app/_models/keyValuePair';

@Injectable()
export class IncidenceStatusResolver implements Resolve<KeyValuePair[]> {
    constructor(private isService: IncidenceStatusService, private router: Router,
        private toastr: ToastrService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<KeyValuePair[]> {
        // resolve returns and auto subscribe to observables. the param is from the Activated Route route

        return this.isService.getIncidenceStatuses().pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/dashboard']);
                return of(null);
            })
        );
    }
}