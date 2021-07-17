import { Injectable,Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {IWeatherForecast} from '../interfaces/ICurrentWeather';
import { Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EndpointService {


  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }
  
  getCurrentWeather(cityname:string) : Observable<IWeatherForecast>
  {
    return this.http.get<IWeatherForecast>(this.baseUrl + `weatherforecast/${cityname}` );
  }
  
}
