import { Resolve, Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { LocationService } from 'app/shared/services/location.service';
import { CountryDetail } from 'app/_models/countrydetail';

@Injectable()
export class CountryDetailListResolver implements Resolve<CountryDetail[]> {
    filter: any = {page: 1, pageSize: 10};
    constructor(private locationService: LocationService, private router: Router,
        private toastr: ToastrService) {}

    resolve(): Observable<CountryDetail[]> {
        // resolve returns and auto subscribe to observables. the param is from the Activated Route route
        return this.locationService.getCountryDetailList(this.filter).pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/dashboard']);
                return of(null);
            })
        );
    }
}
