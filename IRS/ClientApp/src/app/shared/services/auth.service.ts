import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'environments/environment';

@Injectable()
export class AuthService {
  token: string;
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private http: HttpClient) {}

  signupUser(email: string, password: string) {
    // your code for signing up the new user
  }

  signinUser(model: any) {
    // your code for checking credentials and getting tokens for for signing in user
    //console.log("model values...");
    //console.log(model);
    return this.http.post(this.baseUrl + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user)
        {
          localStorage.setItem('token', user.token);
          //console.log('user token value :' + user.token);
          // decode the token to fetch the username for usage in the view
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          //console.log(this.decodedToken);
        }
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
  }

  getToken() {
    // return this.token;
    return this.decodedToken;
  }

  isAuthenticated() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  roleMatch(allowedRoles): boolean {
    // this.decodedToken of AuthService is lost upon refresh. hence we decode token from localStorage here again
    const userToken = localStorage.getItem('token');
    this.decodedToken = this.jwtHelper.decodeToken(userToken);
    let isMatch = false;
    const userRoles = this.decodedToken.role as Array<string>;
    allowedRoles.forEach(element => {
      if (userRoles && userRoles.includes(element)) {
        isMatch = true;
        return;
      }
    });
    return isMatch;
  }
}
