import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { KeyValuePair } from 'app/_models/keyValuePair';
import { IncidenceTypeDepartment } from 'app/_models/incidenceTypeDepartment';
import { SaveIncidenceTypeDepartment } from 'app/_models/saveIncidenceTypeDepartment';

@Injectable({
  providedIn: 'root'
})

export class IncidenceTypeDepartmentService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

  getIncidenceTypeDepartment(id): Observable<IncidenceTypeDepartment> {
    return this.http.get<IncidenceTypeDepartment>(this.baseUrl + 'incidenceTypeDepartment/' + id);
  }

  updateIncidenceTypeDepartment(incidenceTypeDept: SaveIncidenceTypeDepartment) {
    return this.http.put(this.baseUrl + 'incidenceTypeDepartment/updateIncidenceTypeDepartment/' + incidenceTypeDept.id, incidenceTypeDept);
  }

  createIncidenceTypeDepartment(incidenceTypeDept) {
   return this.http.post(this.baseUrl + 'incidenceTypeDepartment/createIncidenceTypeDepartment', incidenceTypeDept);
  }

  getIncidenceTypeDepartments(): Observable<KeyValuePair[]> {
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'incidenceTypeDepartment/getIncidenceTypeDepartments' );
  }

  getIncidenceTypeDepartmentList(filter): Observable<IncidenceTypeDepartment[]> {
    return this.http.get<IncidenceTypeDepartment[]>(this.baseUrl + 'incidenceTypeDepartment/getIncidenceTypesList' + '?' + this.toQueryString(filter));
  }

  getUnmappedIncidenceTypes(): Observable<KeyValuePair[]> {
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'incidenceTypeDepartment/getUnmappedIncidenceTypesForUser');
  }

  deleteIncidenceTypeDepartment(id) {
    return this.http.delete(this.baseUrl + 'incidenceTypeDepartment/' + id);
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

