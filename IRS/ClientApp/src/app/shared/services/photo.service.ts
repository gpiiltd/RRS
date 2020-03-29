import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'environments/environment';
import { Photo } from 'app/_models/photo';
import { Observable } from 'rxjs';

@Injectable()
export class PhotoService {
    baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

  upload(incidenceId, photo, channelId): Observable<any> {
  const formData = new FormData();
    // public async Task<IActionResult> Upload(int e, IFormFile file)
    // 'file' param above in backend must be equal param name at the backend i.e IFormFile file
    formData.append('file', photo);
    return this.http.post(`${this.baseUrl}incidence/${incidenceId}/media/${channelId}`, formData);
  }

  uploadHazardPhoto(hazardId, photo, channelId): Observable<any> {
    const formData = new FormData();
      formData.append('file', photo);
      return this.http.post(`${this.baseUrl}hazard/${hazardId}/media/${channelId}`, formData);
    }

  uploadUserProfilePhoto(userId, photo): Observable<any> {
    const formData = new FormData();
    formData.append('file', photo);
    return this.http.post(`${this.baseUrl}users/${userId}/media`, formData);
  }

  getIncidenceMedia(incidenceId): Observable<Photo[]> {
    return this.http.get<Photo[]>(`${this.baseUrl}incidence/${incidenceId}/media`);
  }

  getHazardMedia(hazardId): Observable<Photo[]> {
    return this.http.get<Photo[]>(`${this.baseUrl}hazard/${hazardId}/media`);
  }

  getUserProfilePhoto(userId): Observable<Photo> {
    return this.http.get<Photo>(`${this.baseUrl}users/${userId}/media`);
  }

  deletePhotoVideo(id) {
    return this.http.delete(this.baseUrl + 'deletePhotoVideo/' + id);
  }
}
