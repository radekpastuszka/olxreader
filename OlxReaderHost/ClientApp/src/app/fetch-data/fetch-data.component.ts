import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public olxdata: CsvRow[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<CsvRow[]>(baseUrl + 'weatherforecast').subscribe(result => {
      this.olxdata = result;
    }, error => console.error(error));
  }
}

interface CsvRow {
  date: Date;
  wroclawToRent: number;
  wroclawToSell: number;
  gdanskToRent: number;
  gdanskToSell: number;
  warszawaToRent: number;
  warszawaToSell: number;
  krakowToRent: number;
  krakowToSell: number;
  poznanToRent: number;
  poznanToSell: number;
  lodzToRent: number;
  lodzToSell: number;
  totalToRent: number;
  totalToSell: number;
}
