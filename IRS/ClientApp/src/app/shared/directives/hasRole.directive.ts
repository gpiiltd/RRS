import { Directive, ViewContainerRef, TemplateRef, Input } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective {
  @Input() appHasRole: string[];
  isVisible = false;
  decodedToken: any;
  jwtHelper = new JwtHelperService();
  userToken: any;

  constructor(
    private viewContainerRef: ViewContainerRef,
    private templateRef: TemplateRef<any>,
    private authService: AuthService) { }

  ngOnInit() {
    this.userToken = localStorage.getItem('token');
    this.decodedToken = this.jwtHelper.decodeToken(this.userToken);
    const userRoles = this.decodedToken.role as Array<string>;
    // if no roles clear the view container ref
    // hide any element we have the appHasRole directive on if the user has no roles at all
    if (!userRoles) {
      this.viewContainerRef.clear();
    }

    // if user has role needed then render the element
    // this.templateRef refers to anything we are applying the structural directive to
    // console.log('appHasRole value..');
    // console.log(this.appHasRole);
    if (this.authService.roleMatch(this.appHasRole)) {
      if (!this.isVisible) {
        this.isVisible = true;
        this.viewContainerRef.createEmbeddedView(this.templateRef);
      } else {
        // this.isVisible = false;
        this.viewContainerRef.clear();
      }
    }
  }

}