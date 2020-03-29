import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { KeyValuePair } from 'app/_models/keyValuePair';
import { Organization } from 'app/_models/organization';
import { IncidenceType } from 'app/_models/incidenceType';

@Injectable({
  providedIn: 'root'
})

export class IncidenceTypeService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

getIncidenceTypes(): Observable<KeyValuePair[]> {
   return this.http.get<KeyValuePair[]>(this.baseUrl + 'incidenceType');
 }

getIncidenceTypesList(filter): Observable<IncidenceType[]> {
 return this.http.get<IncidenceType[]>(this.baseUrl + 'IncidenceType/getincidenceTypesList' + '?' + this.toQueryString(filter));
}

updateIncidenceType(incidenceType: IncidenceType) {
  return this.http.put(this.baseUrl + 'IncidenceType/' + incidenceType.id, incidenceType);
}

createIncidenceType(incidenceType) {
  return this.http.post(this.baseUrl + 'IncidenceType/createIncidenceType', incidenceType);
}

deleteIncidenceType(id) {
  return this.http.delete(this.baseUrl + 'IncidenceType/' + id);
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

