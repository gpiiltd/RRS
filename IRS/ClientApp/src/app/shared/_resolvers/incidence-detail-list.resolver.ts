import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Incidence } from 'app/_models/incidence';
import { IncidenceService } from '../services/incidence.service';

@Injectable()
export class IncidenceDetailListResolver implements Resolve<Incidence[]> {
    filter: any = {page: 1, pageSize: 50};
    constructor(private iService: IncidenceService, private router: Router,
        private toastr: ToastrService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Incidence[]> {
        // resolve returns and auto subscribe to observables. the param is from the Activated Route route

        return this.iService.getIncidenceList(this.filter).pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/dashboard']);
                return of(null);
            })
        );
    }
}