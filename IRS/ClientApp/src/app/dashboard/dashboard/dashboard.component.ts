import { Component, OnInit } from '@angular/core';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import { Label } from 'ng2-charts';
import { Router, ActivatedRoute } from '@angular/router';
import { BsDatepickerConfig, BsDatepickerViewMode } from 'ngx-bootstrap/datepicker';
import { KeyValuePair } from 'app/_models/keyValuePair';
import { IntKeyValuePair } from 'app/_models/intKeyValuePair';
import * as moment from 'moment'
import { IncidenceService } from 'app/shared/services/incidence.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  queryResultItems: any[] = [];
  queryResult: any;
  datas: any[] = [];
  barChartLabels: Label[] = [];
  barChartData: ChartDataSets[] = [];
  public barChartOptions: ChartOptions = {
    responsive: true,
    // We use these empty structures as placeholders for dynamic theming.
    scales: { xAxes: [{}], yAxes: [{}] },
  };
  bsValue: Date = new Date(2017, 7);
  minMode: BsDatepickerViewMode = 'year';
  bsConfig: Partial<BsDatepickerConfig>;

  public barChartType: ChartType = 'bar';
  public barChartLegend = true;
  yearList = Array<IntKeyValuePair>();
  // selectedYear: IntKeyValuePair;
  currentYear = moment().year();
  emptyRecords = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
  public chartColors: Array<any> = [
    { // first color
      backgroundColor: '#d9534f',
      borderColor: '#d9534f',
      pointBackgroundColor: '#d9534f',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#d9534f',
      pointHoverBorderColor: '#d9534f'
    },
    { // second color
      backgroundColor: '#5cb85c',
      borderColor: '#5cb85c',
      pointBackgroundColor: '#5cb85c',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#5cb85c',
      pointHoverBorderColor: '#5cb85c'
    },
    { // third color
      backgroundColor: '#f0ad4e',
      borderColor: '#f0ad4e',
      pointBackgroundColor: '#f0ad4e',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#f0ad4e',
      pointHoverBorderColor: '#f0ad4e'
    }];
    organizationList: any[];
    organizationId: "";
    isGlobalAdminUser = false;
    jwtHelper = new JwtHelperService();
    selectedYear: any = {
      id: this.currentYear,
      name: this.currentYear
    }

  constructor(private router: Router, private route: ActivatedRoute, private iService: IncidenceService) { }

  ngOnInit() {
    this.bsConfig = Object.assign({}, {
      minMode : this.minMode
    });
    this.route.data.subscribe(data => {
    this.queryResult = data['incidenceDashboardReportData'];
    this.queryResultItems = this.queryResult.items;

    this.datas[0] = this.getMonthlyChartDatasForAnIncidenceStatus(this.queryResultItems[0]);
    this.datas[1] = this.getMonthlyChartDatasForAnIncidenceStatus(this.queryResultItems[1]);
    this.datas[2] = this.getMonthlyChartDatasForAnIncidenceStatus(this.queryResultItems[2]);
    this.organizationList = data['OrgListData'];
console.log(data);
    this.generateChart(this.datas[0], this.datas[1], this.datas[2]);
    this.getYears();
    this.getToken();
  });
  console.log("her");
  console.log(moment().year());
  console.log(this.currentYear);
  console.log(this.selectedYear);
}

  // events
  public chartClicked({ event, active }: { event: MouseEvent, active: {}[] }): void {
    // console.log(event, active);
  }

  public chartHovered({ event, active }: { event: MouseEvent, active: {}[] }): void {
    // console.log(event, active);
  }

  public randomize(): void {
    this.barChartType = this.barChartType === 'bar' ? 'line' : 'bar';
  }

  generateChart(chartData:any, chartData1:any, chartData2:any) {
      this.barChartLabels = [
        "Jan",
        "Feb",
        "Mar",
        "Apr",
        "May",
        "Jun",
        "Jul",
        "Aug",
        "Sep",
        "Oct",
        "Nov",
        "Dec"
      ];
      this.barChartData = [
        { data: chartData, label: 'Open Incidences' },
        { data: chartData1, label: 'Closed Incidences' },
        { data: chartData2, label: 'Under Review Incidences' }
      ];

    }

    getYears() {
      this.yearList = [];
      console.log('beg');
      const intial = 2019;
        for (let i = 0; i < 100; i++) {
          this.yearList.push({
            id: intial + i,
            name: intial + i
          });
        }
      }

    GetDashboardReportDataOnYearChange() {
      const filter: any = {Year: this.selectedYear.id, OrganizationId: this.organizationId};
        this.iService.getIncidenceDashboardReportList(filter)
          .subscribe(result => {
            this.queryResult = result;
            console.log(this.queryResult);
            this.queryResultItems = this.queryResult.items;
            this.datas[0] = this.getMonthlyChartDatasForAnIncidenceStatus(this.queryResultItems[0]);
            this.datas[1] = this.getMonthlyChartDatasForAnIncidenceStatus(this.queryResultItems[1]);
            this.datas[2] = this.getMonthlyChartDatasForAnIncidenceStatus(this.queryResultItems[2]);
            this.generateChart(this.datas[0], this.datas[1], this.datas[2]);
        });
    }

    getMonthlyChartDatasForAnIncidenceStatus(barData: any[]) {
      let returnData: any;
      if (!barData) {
        returnData = this.emptyRecords;
      } else {
        returnData = Object.keys(barData).map(function(key) { return barData[key] });
      }
      return returnData;
    }

    resetYearToCurrent() {
      this.selectedYear = {
        id: this.currentYear,
        name: this.currentYear
      }
      this.organizationId = "";
      this.GetDashboardReportDataOnYearChange();
    }

    getToken() {
      const userToken = localStorage.getItem('token');
      const decodedToken = this.jwtHelper.decodeToken(userToken);
      console.log(decodedToken);
      if (decodedToken.unique_name === 'Admin') {
        this.isGlobalAdminUser = true;
      }
      console.log(this.isGlobalAdminUser);
    }
}

