import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { SystemSettingsService } from 'app/shared/services/system-settings.service';
import { AuthService } from 'app/shared/services/auth.service';
import { SystemSettings } from 'app/_models/systemSettings';

@Injectable()
export class SystemSettingsResolver implements Resolve<SystemSettings> {
    constructor(private systemSettingsService: SystemSettingsService, private router: Router,
        private toastr: ToastrService, private authService: AuthService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<SystemSettings> {
        // resolve returns and auto subscribe to observables. the param is from the Activated Route route
        return this.systemSettingsService.getSystemSettings().pipe(
            catchError(error => {
                this.toastr.error('Problem retrieving your data');
                this.router.navigate(['/dashboard']);
                return of(null);
            })
        );
    }
}
