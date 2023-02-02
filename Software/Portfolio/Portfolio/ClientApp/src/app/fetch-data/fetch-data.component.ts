import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[] = [];

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
  date: string | undefined;
  temperatureC: number | undefined;
  temperatureF: number | undefined;
  summary: string | undefined;
}
