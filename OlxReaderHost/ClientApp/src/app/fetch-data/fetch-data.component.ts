import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';


import {
  ChartComponent,
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexTitleSubtitle
} from "ng-apexcharts";


@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent implements OnInit {
  public olxdata: OlxDataDto[];
  city: string;
  dataType: number;
  chartOptions: any;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private actRoute: ActivatedRoute, private router: Router) {

    this.router.routeReuseStrategy.shouldReuseRoute = () => { return false; };
    this.dataType = this.actRoute.snapshot.params.dataType;

    this.chartOptions = {
      series: [{
        name: "ToSell",
        data: []
      }],
      chart: {
        height: 350,
        type: "line"
      },
      title: {
        text: "Wrocław"
      },
      xaxis: {
        type: 'datetime',
        labels: {
          format: 'dd/MM',
        }
      }
    };

    

    http.get<OlxDataDto[]>(baseUrl + 'weatherforecast?dataType=' + this.dataType).subscribe(result => {
      this.olxdata = result;

      const toRent = this.olxdata.map(function (obj) {
        return { x: new Date(obj.date).toLocaleDateString(), y: obj.toRent };
      });

      const toSell = this.olxdata.map(function (obj) {
        return { x: new Date(obj.date).toLocaleDateString(), y: obj.toSell };
      });

      this.chartOptions.title.text = this.city;

      this.chartOptions.series = [{
        name: "Do wynajęcia",
        data: toRent
        }]
    }, error => console.error(error));

    this.city = this.getDataTypeName(this.dataType);
  }

  ngOnInit() {
    
  }

  getDataTypeName(dataType: number): string {
    let toReturn = "";
    switch (dataType) {
      case 0:
        toReturn = "Wszystkie";
        break;
      case 1:
        toReturn = "Wrocław";
        break;
      case 2:
        toReturn = "Gdańsk";
        break;
      case 3:
        toReturn = "Warzawa";
        break;
      case 4:
        toReturn = "Kraków";
        break;
      case 5:
        toReturn = "Poznań";
        break;
      case 6:
        toReturn = "Łódź";
    }
    return toReturn;
  }
}

interface OlxDataDto {
  date: Date;
  toSell: number;
  toRent: number;
}
