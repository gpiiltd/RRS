import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { Hazard } from 'app/_models/hazard';
import { SaveHazard } from 'app/_models/saveHazard';
import { HazardDashboard } from 'app/_models/hazardDashboard';

@Injectable({
  providedIn: 'root'
})

export class HazardService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

getHazardList(filter): Observable<Hazard[]> {
 return this.http.get<Hazard[]>(this.baseUrl + 'Hazard/getHazardList' + '?' + this.toQueryString(filter));
}

getHazard(id): Observable<SaveHazard> {
  return this.http.get<SaveHazard>(this.baseUrl + 'Hazard/' + id);
}

getHazardDashboardReportList(filter): Observable<HazardDashboard[]> {
  return this.http.get<HazardDashboard[]>(this.baseUrl + 'hazardReport/getHazardMonthlyReportList'
   + '?' + this.toQueryString(filter));
 }

updateHazard(hazard: Hazard) {
  return this.http.put(this.baseUrl + 'Hazard/' + hazard.id, hazard);
}

createHazard(incidence) {
  return this.http.post(this.baseUrl + 'Hazard/createHazard', incidence);
}

deleteHazard(id) {
  return this.http.delete(this.baseUrl + 'Hazard/' + id);
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

