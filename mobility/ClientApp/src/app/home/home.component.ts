import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {IWeatherForecast} from '../interfaces/ICurrentWeather';
import {EndpointService} from '../services/endpoint.service';
//import { homedir } from 'os';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  
  public curweather: IWeatherForecast;
  public cityname : string;
  
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private weatherSrv : EndpointService ) {
  }

  public searchWeather ()
  {
    console.log("searchWeather clicked. " + this.cityname);
    if ( this.cityname != "")
    {
      
      this.weatherSrv.getCurrentWeather(this.cityname).subscribe(result => {
        this.curweather = result;
        
        console.log(result)

      }, error => console.error(error));
  
    }

  }
}
