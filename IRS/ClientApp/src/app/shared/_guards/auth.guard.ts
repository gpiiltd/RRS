import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router, private toastr: ToastrService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    // we cant use next.data as the AuthGuard is protecting child route. we use next.firstChild which equals the activated route
    // check for the existence of roles as we are not adding roles to every route
    const roles = route.firstChild.data['roles'] as Array<string>;
    if (roles) {
      const match = this.authService.roleMatch(roles);
      if (match) {
        return true;
      } else {
        this.router.navigate(['dashboard']);
        this.toastr.error('You are not authorized to access this area');
      }
    }

    // we still check if the user is authenticated
    if (this.authService.isAuthenticated()) {
      return true;
    }

    this.toastr.success('Redirecting to login');
    this.router.navigate(['auth/login']);
    return false;
  }
}
