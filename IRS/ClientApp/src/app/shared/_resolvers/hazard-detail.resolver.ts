import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'app/shared/services/auth.service';
import { SaveHazard } from 'app/_models/saveHazard';
import { HazardService } from '../services/hazard.services';

@Injectable()
export class HazardViewResolver implements Resolve<SaveHazard> {
    constructor(private hazardService: HazardService, private router: Router,
        private toastr: ToastrService, private authService: AuthService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<SaveHazard> {
        // resolve returns and auto subscribe to observables. the param is from the Activated Route route
        return this.hazardService.getHazard(route.params['id']).pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/dashboard']);
                return of(null);
            })
        );
    }
}