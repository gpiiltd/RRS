import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { KeyValuePair } from 'app/_models/keyValuePair';
import { IncidenceTypeService } from '../services/incidence-type.service';

@Injectable()
export class IncidenceTypeResolver implements Resolve<KeyValuePair[]> {
    constructor(private isService: IncidenceTypeService, private router: Router,
        private toastr: ToastrService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<KeyValuePair[]> {
        // resolve returns and auto subscribe to observables. the param is from the Activated Route route

        return this.isService.getIncidenceTypes().pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/dashboard']);
                return of(null);
            })
        );
    }
}