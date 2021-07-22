import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IWeatherForecast } from '../interfaces/ICurrentWeather';
import { EndpointService } from '../services/endpoint.service';
import { SearchHistoryService } from '../services/search-history/search-history.service';
//import { homedir } from 'os';


@Component({
	selector: 'app-home',
	templateUrl: './home.component.html',
	styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

	public curweather: IWeatherForecast;
	public cityname: string;
	public history: string[] = [];

	constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private weatherSrv: EndpointService, private historySrv: SearchHistoryService) {


	}
	ngOnInit(): void {
		this.history = this.historySrv.get();
	}
	public searchWeatherBykeyp(e)
	{
		if(e.keyCode === 13){
			this.searchWeather();
		 }
	}

	public searchWeather() {
		console.log("searchWeather clicked. " + this.cityname);

		if (this.cityname != "") {
			
			this.searchRequest(this.cityname);

		}
	}

	public hasData() {
		let yes: boolean = this.curweather != null && this.curweather.cod == 200;

		return yes;
	}
	public cityNotFound() {
		let yes: boolean = this.curweather != null && this.curweather.cod != 200;
		return yes;
	}
	
	public reSearch(item:string)
	{
		
		this.searchRequest(item,false);

		this.cityname = item;

	}

	searchRequest(city:string, addHistory:boolean = true) {

		if (city != "") {

			this.weatherSrv.getCurrentWeather(city).subscribe(result => {
				this.curweather = result;

				if ( addHistory )
					this.addHistory(city);

				console.log(result)

			}, error => console.error(error));

		}
	}	
	initLocalStorage() {
		let item: string = localStorage.getItem('searchHistory');
		if (item == null || item === "") {
			localStorage.setItem("searchHistory", JSON.stringify([]));
		}
		else {
			this.history = JSON.parse(item);
		}

	}

	addHistory(city) {

		this.historySrv.set(this.cityname);


	}


}
