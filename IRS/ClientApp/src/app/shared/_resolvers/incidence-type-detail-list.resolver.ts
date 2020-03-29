import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { IncidenceType } from 'app/_models/incidenceType';
import { IncidenceTypeService } from '../services/incidence-type.service';

@Injectable()
export class IncidenceTypeDetailListResolver implements Resolve<IncidenceType[]> {
    filter: any = {page: 1, pageSize: 50};
    constructor(private itService: IncidenceTypeService, private router: Router,
        private toastr: ToastrService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<IncidenceType[]> {
        // resolve returns and auto subscribe to observables. the param is from the Activated Route route

        return this.itService.getIncidenceTypesList(this.filter).pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/dashboard']);
                return of(null);
            })
        );
    }
}