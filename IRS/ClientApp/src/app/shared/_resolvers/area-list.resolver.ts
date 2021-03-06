import { KeyValuePair } from 'app/_models/keyValuePair';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { LocationService } from 'app/shared/services/location.service';

@Injectable()
export class AreaListResolver implements Resolve<KeyValuePair[]> {
    constructor(private locationService: LocationService, private router: Router,
        private toastr: ToastrService) {}

    resolve(): Observable<KeyValuePair[]> {
        // resolve returns and auto subscribe to observables. the param is from the Activated Route route
        return this.locationService.getAreas().pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/dashboard']);
                return of(null);
            })
        );
    }
}