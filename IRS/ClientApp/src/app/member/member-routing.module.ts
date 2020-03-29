import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { IncidentListComponent } from './incidence/incident-list/incident-list.component';
import { IncidenceDetailListResolver } from 'app/shared/_resolvers/incidence-detail-list.resolver';
import { IncidenceEditComponent } from './incidence/incidence-edit/incidence-edit.component';
import { CountryListResolver } from 'app/shared/_resolvers/country-list.resolver';
import { StateListResolver } from 'app/shared/_resolvers/state-list.resolver';
import { AreaListResolver } from 'app/shared/_resolvers/area-list.resolver';
import { UserOrganizationDepartmentsResolver } from 'app/shared/_resolvers/user-department.resolver';
import { UserOrganizationsResolver } from 'app/shared/_resolvers/user-organization.resolver';
import { IncidenceViewResolver } from 'app/shared/_resolvers/incidence-detail.resolver';
import { UserKeyValueListResolver } from 'app/shared/_resolvers/user-key-value-list.resolver';
import { IncidenceTypeResolver } from 'app/shared/_resolvers/incidence-type.resolver';
import { IncidenceStatusResolver } from 'app/shared/_resolvers/incidence-status.resolver';
import { CityListResolver } from 'app/shared/_resolvers/city-list.resolver';
import { IncidencePhotosResolver } from 'app/shared/_resolvers/incidence-photos-resolver';
import { IncidenceDashboardReportResolver } from 'app/shared/_resolvers/incidence-dashboard.resolver';
import { UserRolesResolver } from 'app/shared/_resolvers/user-roles.resolver';
import { HazardListComponent } from './hazard/hazard-list/hazard-list.component';
import { HazardDetailListResolver } from 'app/shared/_resolvers/hazard-detail-list.resolver';
import { HazardEditComponent } from './hazard/hazard-edit/hazard-edit.component';
import { HazardPhotosResolver } from 'app/shared/_resolvers/hazard-photos-resolver';
import { HazardViewResolver } from 'app/shared/_resolvers/hazard-detail.resolver';
import { IncidenceViewComponent } from './incidence/incidence-view/incidence-view.component';
import { HazardViewComponent } from './hazard/hazard-view/hazard-view.component';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'hazards',
        component: HazardListComponent,
        resolve: {
          hazardDetailListData: HazardDetailListResolver,
          UserRolesData: UserRolesResolver
        },
        data: {
          title: 'Hazards'
        }
      },
      {
        path: 'hazard/edit/:id',
        component: HazardEditComponent,
        resolve: {
          hazardData: HazardViewResolver,
          countryListData: CountryListResolver,
          stateListData: StateListResolver,
          cityListData: CityListResolver,
          areaListData: AreaListResolver,
          DeptListData: UserOrganizationDepartmentsResolver,
          OrgListData: UserOrganizationsResolver,
          userListData: UserKeyValueListResolver,
          hazardStatusListData: IncidenceStatusResolver,
          hazardResolutionPhotosData: HazardPhotosResolver,
          UserRolesData: UserRolesResolver
        },
        data: {
          title: 'Edit Hazard'
        }
      },
      {
        path: 'hazard/view/:id',
        component: HazardViewComponent,
        resolve: {
          hazardData: HazardViewResolver,
          countryListData: CountryListResolver,
          stateListData: StateListResolver,
          cityListData: CityListResolver,
          areaListData: AreaListResolver,
          DeptListData: UserOrganizationDepartmentsResolver,
          OrgListData: UserOrganizationsResolver,
          userListData: UserKeyValueListResolver,
          hazardStatusListData: IncidenceStatusResolver,
          hazardResolutionPhotosData: HazardPhotosResolver,
          UserRolesData: UserRolesResolver
        },
        data: {
          title: 'View Hazard'
        }
      },
      {
        path: 'hazard/new',
        component: HazardEditComponent,
        resolve: {
          countryListData: CountryListResolver,
          stateListData: StateListResolver,
          cityListData: CityListResolver,
          areaListData: AreaListResolver,
          DeptListData: UserOrganizationDepartmentsResolver,
          OrgListData: UserOrganizationsResolver,
          userListData: UserKeyValueListResolver,
          hazardStatusListData: IncidenceStatusResolver
        },
        data: {
          title: 'Add Hazard'
        }
      },
      {
        path: 'incidences',
        component: IncidentListComponent,
        resolve: {
          incidenceDetailListData: IncidenceDetailListResolver,
          UserRolesData: UserRolesResolver
        },
        data: {
          title: 'Incidences'
        }
      },
      {
        path: 'incidence/edit/:id',
        component: IncidenceEditComponent,
        resolve: {
          incidenceData: IncidenceViewResolver,
          countryListData: CountryListResolver,
          stateListData: StateListResolver,
          cityListData: CityListResolver,
          areaListData: AreaListResolver,
          DeptListData: UserOrganizationDepartmentsResolver,
          OrgListData: UserOrganizationsResolver,
          userListData: UserKeyValueListResolver,
          incidenceTypeListData: IncidenceTypeResolver,
          incidenceStatusListData: IncidenceStatusResolver,
          incidenceResolutionPhotosData: IncidencePhotosResolver,
          UserRolesData: UserRolesResolver
        },
        data: {
          title: 'Edit Incidence'
        }
      },
      {
        path: 'incidence/view/:id',
        component: IncidenceViewComponent,
        resolve: {
          incidenceData: IncidenceViewResolver,
          countryListData: CountryListResolver,
          stateListData: StateListResolver,
          cityListData: CityListResolver,
          areaListData: AreaListResolver,
          DeptListData: UserOrganizationDepartmentsResolver,
          OrgListData: UserOrganizationsResolver,
          userListData: UserKeyValueListResolver,
          incidenceTypeListData: IncidenceTypeResolver,
          incidenceStatusListData: IncidenceStatusResolver,
          incidenceResolutionPhotosData: IncidencePhotosResolver,
          UserRolesData: UserRolesResolver
        },
        data: {
          title: 'View Incidence'
        }
      },
      {
        path: 'incidence/new',
        component: IncidenceEditComponent,
        resolve: {
          countryListData: CountryListResolver,
          stateListData: StateListResolver,
          cityListData: CityListResolver,
          areaListData: AreaListResolver,
          DeptListData: UserOrganizationDepartmentsResolver,
          OrgListData: UserOrganizationsResolver,
          userListData: UserKeyValueListResolver,
          incidenceTypeListData: IncidenceTypeResolver,
          incidenceStatusListData: IncidenceStatusResolver
        },
        data: {
          title: 'Add Incidence'
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MemberRoutingModule { }
