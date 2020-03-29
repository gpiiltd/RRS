import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { User } from 'app/_models/user';
import { UsersRoles } from 'app/_models/usersRoles';
import { SaveUser } from 'app/_models/saveUser';
import { KeyValuePair } from 'app/_models/keyValuePair';

@Injectable({
  providedIn: 'root'
})

export class UserService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

  // used by user-edit component
  getUser(id): Observable<SaveUser> {
    return this.http.get<SaveUser>(this.baseUrl + 'users/' + id);
  }

  islobalAdminUser(): Observable<boolean> {
    return this.http.get<boolean>(this.baseUrl + 'users/IsGlobalAdminUserCheck');
  }

  // updateUser(id: any, user: User) {
  //   return this.http.put(this.baseUrl + 'users/' + id, user);
  // }

  updateUser(user: SaveUser) {
    return this.http.put(this.baseUrl + 'Users/' + user.id, user);
  }

  createUser(user) {
   return this.http.post(this.baseUrl + 'Users/createUser', user);
  }

  getUsers(): Observable<KeyValuePair[]> {
    // console.log("calling user service...");
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'Users/getUsers' );
  }

  getUserList(filter): Observable<User[]> {
    // console.log("calling user service...");
    return this.http.get<User[]>(this.baseUrl + 'Users/getUserList' + '?' + this.toQueryString(filter));
  }

  getUsersWithRoles(filter): Observable<UsersRoles[]> {
    // console.log("calling user service...");
  return this.http.get<UsersRoles[]>(this.baseUrl + 'users/GetUsersWithRoles' + '?' + this.toQueryString(filter));
  }

  getUserRoles(username): Observable<any[]> {
    // console.log("calling user service...");
  return this.http.get<any[]>(this.baseUrl + 'users/GetUserRoles/' + username);
  }

  updateUserRoles(user: UsersRoles, roles: {}) {
    return this.http.post(this.baseUrl + 'users/editroles/' + user.userName, roles);
  }

  deleteUser(id) {
    return this.http.delete(this.baseUrl + 'users/' + id);
  }

  toQueryString(obj) {
    var parts: string[] = [];
    // tslint:disable-next-line: forin
    for (var property in obj) {
        var value = obj[property];
        if (value != null && value != undefined)
            parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(value));
    }

  return parts.join('&');
}
}

