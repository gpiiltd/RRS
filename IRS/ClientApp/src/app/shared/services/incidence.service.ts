import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { Incidence } from 'app/_models/incidence';
import { SaveIncidence } from 'app/_models/saveIncidence';
import { IncidenceDashboard } from 'app/_models/incidenceDashboard';

@Injectable({
  providedIn: 'root'
})

export class IncidenceService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

getIncidenceList(filter): Observable<Incidence[]> {
 return this.http.get<Incidence[]>(this.baseUrl + 'Incidence/getincidenceList' + '?' + this.toQueryString(filter));
}

getIncidence(id): Observable<SaveIncidence> {
  return this.http.get<SaveIncidence>(this.baseUrl + 'Incidence/' + id);
}

getIncidenceDashboardReportList(filter): Observable<IncidenceDashboard[]> {
  return this.http.get<IncidenceDashboard[]>(this.baseUrl + 'incidenceReport/getIncidenceMonthlyReportList'
   + '?' + this.toQueryString(filter));
 }

updateIncidence(incidence: Incidence) {
  return this.http.put(this.baseUrl + 'Incidence/' + incidence.id, incidence);
}

createIncidence(incidence) {
  return this.http.post(this.baseUrl + 'Incidence/createIncidence', incidence);
}

deleteIncidence(id) {
  return this.http.delete(this.baseUrl + 'Incidence/' + id);
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

