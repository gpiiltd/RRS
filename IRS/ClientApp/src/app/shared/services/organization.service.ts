import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { KeyValuePair } from 'app/_models/keyValuePair';
import { Organization } from 'app/_models/organization';

@Injectable({
  providedIn: 'root'
})

export class OrganizationService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

getOrganizations(): Observable<any[]> {
   return this.http.get<any[]>(this.baseUrl + 'organization/getOrganizations');
}

 getOrganizationDetailList(filter): Observable<Organization[]> {
 return this.http.get<Organization[]>(this.baseUrl + 'organization/getorganizationDetailList' + '?' + this.toQueryString(filter));
}

getOrganizationDetail(filter): Observable<Organization> {
  return this.http.get<Organization>(this.baseUrl + 'organization/getLoggedInUserOrganization');
 }

updateOrganization(org: Organization) {
  return this.http.put(this.baseUrl + 'organization/' + org.id, org);
}

createOrganization(org) {
 return this.http.post(this.baseUrl + 'organization/createOrganization', org);
}

deleteOrganization(id) {
 return this.http.delete(this.baseUrl + 'organization/' + id);
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

