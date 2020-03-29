import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { KeyValuePair } from 'app/_models/keyValuePair';
import { AreaDetail } from 'app/_models/areadetail';
import { StateDetail } from 'app/_models/statedetail';
import { CountryDetail } from 'app/_models/countrydetail';
import { CityDetail } from 'app/_models/citydetail';
import 'rxjs/add/operator/map';

@Injectable({
  providedIn: 'root'
})

export class LocationService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

 getCountries(): Observable<KeyValuePair[]> {
   return this.http.get<KeyValuePair[]>(this.baseUrl + 'country/getCountries');
 }

 getStates(): Observable<KeyValuePair[]> {
  return this.http.get<KeyValuePair[]>(this.baseUrl + 'state/getStates');
}

getCities(): Observable<KeyValuePair[]> {
  return this.http.get<KeyValuePair[]>(this.baseUrl + 'city/getCities');
}

getAreas(): Observable<KeyValuePair[]> {
  return this.http.get<KeyValuePair[]>(this.baseUrl + 'area/getAreas');
}

getAreaDetailList(filter): Observable<AreaDetail[]> {
  return this.http.get<AreaDetail[]>(this.baseUrl + 'area/getAreaList' + '?' + this.toQueryString(filter));
}

getCityDetailList(filter): Observable<CityDetail[]> {
  return this.http.get<CityDetail[]>(this.baseUrl + 'city/getCityList' + '?' + this.toQueryString(filter));
}

getStateDetailList(filter): Observable<StateDetail[]> {
  return this.http.get<StateDetail[]>(this.baseUrl + 'state/getStateList' + '?' + this.toQueryString(filter));
}

getCountryDetailList(filter): Observable<CountryDetail[]> {
  return this.http.get<CountryDetail[]>(this.baseUrl + 'country/getCountryList' + '?' + this.toQueryString(filter));
}

updateArea(area: AreaDetail) {
  return this.http.put(this.baseUrl + 'area/editArea/' + area.id, area);
}

createArea(area) {
  return this.http.post(this.baseUrl + 'area/createArea', area);
}

deleteArea(id) {
  return this.http.delete(this.baseUrl + 'area/' + id);
}

updateCity(city: CityDetail) {
  return this.http.put(this.baseUrl + 'city/editCity/' + city.id, city);
}

createCity(city) {
  return this.http.post(this.baseUrl + 'city/createCity', city);
}

deleteCity(id) {
  return this.http.delete(this.baseUrl + 'city/' + id);
}

updateState(state: StateDetail) {
  return this.http.put(this.baseUrl + 'state/editState/' + state.id, state);
}

createState(state) {
  return this.http.post(this.baseUrl + 'state/createState', state);
}

deleteState(id) {
  return this.http.delete(this.baseUrl + 'state/' + id);
}

updateCountry(country: CountryDetail) {
  return this.http.put(this.baseUrl + 'country/editCountry/' + country.id, country);
}

createCountry(country) {
  return this.http.post(this.baseUrl + 'country/createCountry', country);
}

deleteCountry(id) {
  return this.http.delete(this.baseUrl + 'country/' + id);
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

