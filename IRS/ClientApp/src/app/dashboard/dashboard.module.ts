import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";

import { DashboardRoutingModule } from "./dashboard-routing.module";
import { ChartistModule } from 'ng-chartist';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { MatchHeightModule } from "../shared/directives/match-height.directive";

import { Dashboard1Component } from "./dashboard1/dashboard1.component";
import { Dashboard2Component } from "./dashboard2/dashboard2.component";
import { DashboardComponent } from './dashboard/dashboard.component';
import { ChartsModule } from 'ng2-charts';
import { IncidenceDashboardReportResolver } from 'app/shared/_resolvers/incidence-dashboard.resolver';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UserOrganizationsResolver } from 'app/shared/_resolvers/user-organization.resolver';
import { CountryListResolver } from 'app/shared/_resolvers/country-list.resolver';


@NgModule({
    imports: [
        CommonModule,
        DashboardRoutingModule,
        ChartistModule,
        NgbModule,
        MatchHeightModule,
        ChartsModule,
        BsDatepickerModule.forRoot(),
        FormsModule,
        ReactiveFormsModule,
    ],
    exports: [],
    declarations: [
        Dashboard1Component,
        Dashboard2Component,
        DashboardComponent
    ],
    providers: [
        IncidenceDashboardReportResolver,
        UserOrganizationsResolver,
        CountryListResolver
    ],
})
export class DashboardModule { }
