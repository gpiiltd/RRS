import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Hazard } from 'app/_models/hazard';
import { HazardService } from '../services/hazard.services';

@Injectable()
export class HazardDetailListResolver implements Resolve<Hazard[]> {
    filter: any = {page: 1, pageSize: 50};
    constructor(private hazardService: HazardService, private router: Router,
        private toastr: ToastrService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Hazard[]> {
        // resolve returns and auto subscribe to observables. the param is from the Activated Route route

        return this.hazardService.getHazardList(this.filter).pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/dashboard']);
                return of(null);
            })
        );
    }
}