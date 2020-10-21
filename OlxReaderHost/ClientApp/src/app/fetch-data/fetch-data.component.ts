import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public olxdata: OlxDataDto[];
  city: string;
  chartOptions: any;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private actRoute: ActivatedRoute, private router: Router) {

    this.router.routeReuseStrategy.shouldReuseRoute = () => { return false; };
    this.city = this.actRoute.snapshot.params.city || "";

    this.chartOptions = {
      series: [{
        name: "ToSell",
        data: []
      }],
      chart: {
        height: 700,
        type: "area"
      },
      dataLabels: {
        enabled: false
      },
      title: {
        text: "Wrocław"
      },
      stroke: {
        width: 1,
        curve: "smooth"
      },
      xaxis: {
        type: 'datetime'
      }
    };

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
      }, {
          name: "Do sprzedaży",
          data: toSell
        }];

      this.chartOptions.title = {
        text: this.city
      }
    }, error => console.error(error));
  }
}

interface OlxDataDto {
  date: Date;
  toSell: number;
  toRent: number;
}
