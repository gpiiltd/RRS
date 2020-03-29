import { Component, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from 'app/shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-login-page',
    templateUrl: './login-page.component.html',
    styleUrls: ['./login-page.component.scss']
})

export class LoginPageComponent {
    model: any = {};
    logoUrl = 'assets/img/IRS.png';
    @ViewChild('f') loginForm: NgForm;

    constructor(private router: Router, private route: ActivatedRoute, public _authService: AuthService, private toastr: ToastrService) { }


    login() {
        // this.loginForm.reset();
        //console.log('loggin in');
        this._authService.signinUser(this.model).subscribe(next => {
            this.toastr.success('Successfully logged in');
          }, error => {
              //console.log(error.error);
            this.toastr.error(error.error.Error[0]);
            console.log(error.statusText);
          }, () => this.router.navigate(['dashboard']));
    }


    // On Forgot password link click
    onForgotPassword() {
        this.router.navigate(['forgotpassword'], { relativeTo: this.route.parent });
    }


    // On registration link click
    onRegister() {
        this.router.navigate(['register'], { relativeTo: this.route.parent });
    }
}