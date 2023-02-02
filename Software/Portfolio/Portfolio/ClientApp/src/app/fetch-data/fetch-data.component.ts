import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  forecasts: WeatherForecast[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<WeatherForecast[]>(baseUrl + 'weatherforecast').subscribe(result => {
      this.loadForecasts(result);
    }, error => console.error(error));
  }
  
  loadForecasts(forecasts: WeatherForecast[]): void {
    this.forecasts = forecasts;
  }
}

export interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
