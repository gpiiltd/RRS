import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { IncidenceStatusService } from '../services/incidence-status.service';
import { IncidenceStatus } from 'app/_models/incidenceStatus';

@Injectable()
export class IncidenceStatusDetailListResolver implements Resolve<IncidenceStatus[]> {
    filter: any = {page: 1, pageSize: 50};
    constructor(private isService: IncidenceStatusService, private router: Router,
        private toastr: ToastrService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<IncidenceStatus[]> {
        // resolve returns and auto subscribe to observables. the param is from the Activated Route route

        return this.isService.getIncidenceStatusList(this.filter).pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/dashboard']);
                return of(null);
            })
        );
    }
}