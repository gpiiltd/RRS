import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { SystemSettings } from 'app/_models/systemSettings';

@Injectable({
  providedIn: 'root'
})

export class SystemSettingsService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

  // used by user-edit component
  getSystemSettings(): Observable<SystemSettings> {
    return this.http.get<SystemSettings>(this.baseUrl + 'systemsettings');
  }

  updateSystemSettings(settings: SystemSettings) {
    return this.http.put(this.baseUrl + 'systemsettings/edit/', settings);
  }

  createSystemSettings(settings) {
   return this.http.post(this.baseUrl + 'systemsettings/create', settings);
  }
}

