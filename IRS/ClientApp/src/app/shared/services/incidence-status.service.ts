import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { KeyValuePair } from 'app/_models/keyValuePair';
import { Organization } from 'app/_models/organization';
import { IncidenceType } from 'app/_models/incidenceType';
import { IncidenceStatus } from 'app/_models/incidenceStatus';

@Injectable({
  providedIn: 'root'
})

export class IncidenceStatusService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

// getIncidenceStatuses(): Observable<IncidenceStatus[]> {
//    return this.http.get<IncidenceStatus[]>(this.baseUrl + 'incidenceStatus');
//  }

getIncidenceStatusList(filter): Observable<IncidenceStatus[]> {
 return this.http.get<IncidenceType[]>(this.baseUrl + 'IncidenceStatus/getIncidenceStatusList' + '?' + this.toQueryString(filter));
}

getIncidenceStatuses(): Observable<KeyValuePair[]> {
  return this.http.get<KeyValuePair[]>(this.baseUrl + 'IncidenceStatus');
}

updateIncidenceStatus(incidenceStatus: IncidenceStatus) {
  return this.http.put(this.baseUrl + 'IncidenceStatus/' + incidenceStatus.id, incidenceStatus);
}

createIncidenceStatus(incidenceStatus) {
  return this.http.post(this.baseUrl + 'IncidenceStatus/createIncidenceStatus', incidenceStatus);
}

deleteIncidenceStatus(id) {
  return this.http.delete(this.baseUrl + 'IncidenceStatus/' + id);
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

