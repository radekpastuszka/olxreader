import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { Constants } from '../model/constants';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public olxdata: OlxDataDto[];
  city: string;
  chartOptions: any;
  cityExist: boolean;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private actRoute: ActivatedRoute, private router: Router) {

    this.router.routeReuseStrategy.shouldReuseRoute = () => { return false; };
    this.city = this.actRoute.snapshot.params.city || "wszystkie";
    this.cityExist = Constants.Cities.map(function (x) {
      return x.toLowerCase();
    }).includes(this.city);

      this.chartOptions = {
        series: [{
          name: "ToSell",
          data: []
        }],
        chart: {
          height: 600,
          type: "area"
        },
        dataLabels: {
          enabled: false
        },
        title: {
          text: "Brak danych"
        },
        stroke: {
          width: 1,
          curve: "smooth"
        },
        xaxis: {
          type: 'datetime'
        }
    };

    if (this.cityExist) {

      http.get<OlxDataDto[]>(baseUrl + 'olxdata?city=' + this.city).subscribe(result => {
        this.olxdata = result;

        const toRent = this.olxdata.map(function (obj) {
          return { x: new Date(obj.date).toLocaleDateString(), y: obj.toRent };
        });

        const toSell = this.olxdata.map(function (obj) {
          return { x: new Date(obj.date).toLocaleDateString(), y: obj.toSell };
        });

        this.chartOptions.series = [{
          name: "Do wynajęcia",
          data: toRent
        },
        {
          name: "Do sprzedaży",
          data: toSell
        }];

        this.chartOptions.title = {
          text: this.city.charAt(0).toUpperCase() + this.city.slice(1)
        };
      }, error => console.error(error));
    }
  }
}

interface OlxDataDto {
  date: Date;
  toSell: number;
  toRent: number;
}
