import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { Dashboard1Component } from "./dashboard1/dashboard1.component";
import { Dashboard2Component } from "./dashboard2/dashboard2.component";
import { IncidenceDashboardReportResolver } from 'app/shared/_resolvers/incidence-dashboard.resolver';
import { DashboardComponent } from './dashboard/dashboard.component';
import { UserOrganizationsResolver } from 'app/shared/_resolvers/user-organization.resolver';
import { CountryListResolver } from 'app/shared/_resolvers/country-list.resolver';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: DashboardComponent,
        resolve: {
          incidenceDashboardReportData: IncidenceDashboardReportResolver,
          OrgListData: UserOrganizationsResolver,
          countryListData: CountryListResolver,
        },
        data: {
          title: 'Dashboard'
        }
      },
      {
        path: 'dashboard1',
        component: Dashboard1Component,
        data: {
          title: 'Dashboard 1'
        }
      },
      {
        path: 'dashboard2',
        component: Dashboard2Component,
        data: {
          title: 'Dashboard2'
        }
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DashboardRoutingModule { }
