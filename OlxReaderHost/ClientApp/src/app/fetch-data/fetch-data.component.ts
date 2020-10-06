import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent implements OnInit {
  public olxdata: OlxDataDto[];
  city: string;
  dataType: number;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private actRoute: ActivatedRoute, private router: Router) {

    this.router.routeReuseStrategy.shouldReuseRoute = () => { return false; };
    this.dataType = this.actRoute.snapshot.params.dataType;

    http.get<OlxDataDto[]>(baseUrl + 'weatherforecast?dataType=' + this.dataType).subscribe(result => {
      this.olxdata = result;
    }, error => console.error(error));

    this.city = this.getDataTypeName(this.dataType);
  }

  ngOnInit() {
    console.log(1);
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
