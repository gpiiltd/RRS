
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { IncidenceDashboard } from 'app/_models/incidenceDashboard';
import { IncidenceService } from '../services/incidence.service';

@Injectable()
export class IncidenceDashboardReportResolver implements Resolve<IncidenceDashboard[]> {
    filter: any = {Year: 2019};
    constructor(private iService: IncidenceService, private router: Router,
        private toastr: ToastrService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<IncidenceDashboard[]> {
        // resolve returns and auto subscribe to observables. the param is from the Activated Route route

        return this.iService.getIncidenceDashboardReportList(this.filter).pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/dashboard']);
                return of(null);
            })
        );
    }
}
