import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { Constants } from '../model/constants';
import { ApexAxisChartSeries, ApexChart, ApexDataLabels, ApexTitleSubtitle, ApexTooltip, ApexStroke, ApexAnnotations } from 'ng-apexcharts';

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  dataLabels: ApexDataLabels;
  title: ApexTitleSubtitle;
  tooltip: ApexTooltip;
  stroke: ApexStroke;
  annotations: ApexAnnotations;
  colors: any;
  toolbar: any;
};

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})

export class FetchDataComponent {
  public olxdata: OlxDataDto[];
  city: string;
  chartOptions: any;
  chartOptions2: any;
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
        }];

        this.chartOptions.title = {
          text: this.city.charAt(0).toUpperCase() + this.city.slice(1) + " - do wynajęcia"
        };

        const sortedToRent = [...toRent].sort(function (a, b) {
          return a.y - b.y;
        });

        const sortedToSell = [...toSell].sort(function (a, b) {
          return a.y - b.y;
        });

        this.chartOptions.annotations = {
          points: [
            {
              x: sortedToRent[sortedToRent.length - 1].x,
              y: sortedToRent[sortedToRent.length - 1].y,
              marker: {
                size: 2,
                fillColor: "#fff",
                strokeColor: "red",
                radius: 1,
                cssClass: "apexcharts-custom-class"
              },
              label: {
                borderColor: "#FF4560",
                offsetY: 0,
                style: {
                  color: "#fff",
                  background: "#FF4560"
                },

                text: "Max wynajem"
              }
            }
          ]
        }

        this.chartOptions2 = { ...this.chartOptions };

        this.chartOptions2.series = [{
          name: "Do sprzedaży",
          data: toSell
        }];

        this.chartOptions2.title = {
          text: this.city.charAt(0).toUpperCase() + this.city.slice(1) + " - do sprzedaży"
        };

        this.chartOptions2.annotations = {
          points: [
            {
              x: sortedToSell[sortedToSell.length - 1].x,
              y: sortedToSell[sortedToSell.length - 1].y,
              marker: {
                size: 2,
                fillColor: "#fff",
                strokeColor: "red",
                radius: 1,
                cssClass: "apexcharts-custom-class"
              },
              label: {
                borderColor: "#FF4560",
                offsetY: 0,
                style: {
                  color: "#fff",
                  background: "#FF4560"
                },

                text: "Max sprzedaż"
              }
            }
          ]
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
