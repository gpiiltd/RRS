import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { KeyValuePair } from 'app/_models/keyValuePair';
import { Department } from 'app/_models/department';
import { SaveDepartmentDetail } from 'app/_models/saveDepartmentDetail';

@Injectable({
  providedIn: 'root'
})

export class DepartmentService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

getDepartment(id): Observable<KeyValuePair> {
  return this.http.get<KeyValuePair>(this.baseUrl + 'department/' + id);
}

 getDepartments(): Observable<KeyValuePair[]> {
   return this.http.get<KeyValuePair[]>(this.baseUrl + 'department/GetDepartments');
 }

 getDepartmentDetailList(filter): Observable<Department[]> {
  return this.http.get<Department[]>(this.baseUrl + 'department/getDepartmentList' + '?' + this.toQueryString(filter));
 }

 updateDepartment(dept: SaveDepartmentDetail) {
   return this.http.put(this.baseUrl + 'department/' + dept.id, dept);
 }

createDepartment(dept) {
  return this.http.post(this.baseUrl + 'department/createDepartment', dept);
}

deleteDepartment(id) {
  return this.http.delete(this.baseUrl + 'department/' + id);
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

