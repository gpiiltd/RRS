import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'app/shared/services/auth.service';
import { IncidenceService } from '../services/incidence.service';
import { SaveIncidence } from 'app/_models/saveIncidence';

@Injectable()
export class IncidenceViewResolver implements Resolve<SaveIncidence> {
    constructor(private iService: IncidenceService, private router: Router,
        private toastr: ToastrService, private authService: AuthService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<SaveIncidence> {
        // resolve returns and auto subscribe to observables. the param is from the Activated Route route
        return this.iService.getIncidence(route.params['id']).pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/dashboard']);
                return of(null);
            })
        );
    }
}