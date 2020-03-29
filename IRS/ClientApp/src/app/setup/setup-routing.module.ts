import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserProfileEditResolver } from 'app/shared/_resolvers/user-profile-edit.resolver';
import { CountryListResolver } from 'app/shared/_resolvers/country-list.resolver';
import { StateListResolver } from 'app/shared/_resolvers/state-list.resolver';
import { AreaListResolver } from 'app/shared/_resolvers/area-list.resolver';
import { UserListResolver } from 'app/shared/_resolvers/user-list.resolver';
import { UserViewResolver } from 'app/shared/_resolvers/user-view.resolver';
import { UserListComponent } from './user/user-list/user-list.component';
import { UserEditComponent } from './user/user-edit/user-edit.component';
import { UserViewComponent } from './user/user-view/user-view.component';
import { AreaListComponent } from './areas/area-list/area-list.component';
import { CityListResolver } from 'app/shared/_resolvers/city-list.resolver';
import { AreaDetailListResolver } from 'app/shared/_resolvers/area-detail-list.resolver';
import { StateListComponent } from './states/state-list/state-list.component';
import { StateDetailListResolver } from 'app/shared/_resolvers/state-detail-list.resolver';
import { CountryListComponent } from './countries/country-list/country-list.component';
import { CountryDetailListResolver } from 'app/shared/_resolvers/country-detail-list.resolver';
import { CityListComponent } from './cities/city-list/city-list.component';
import { CityDetailListResolver } from 'app/shared/_resolvers/city-detail-list.resolver';
import { OrganizationListComponent } from './organization/organization-list/organization-list.component';
import { UserOrganizationsResolver } from 'app/shared/_resolvers/user-organization.resolver';
import { UserOrganizationDepartmentsResolver } from 'app/shared/_resolvers/user-department.resolver';
import { OrganizationDetailListResolver } from 'app/shared/_resolvers/organization-detail-list.resolver';
import { DepartmentDetailListResolver } from 'app/shared/_resolvers/department-detail-list.resolver';
import { DepartmentListComponent } from './department/department-list/department-list.component';
import { IncidenceTypeListComponent } from './incidence-types/incidence-type-list/incidence-type-list.component';
import { IncidenceTypeDetailListResolver } from 'app/shared/_resolvers/incidence-type-detail-list.resolver';
import { IncidenceStatusDetailListResolver } from 'app/shared/_resolvers/incidence-status-detail-list.resolver';
import { IncidenceStatusListComponent } from './incidence-statuses/incidence-status-list/incidence-status-list.component';
import { UsersRolesDetailListResolver } from 'app/shared/_resolvers/users-roles-detail-list.resolver';
import { UserManagementComponent } from './user-role-management/user-management/user-management.component';
import { OrganizationListResolver } from 'app/shared/_resolvers/organization-list.resolver';
import { SystemSettingsComponent } from './system-settings/system-settings.component';
import { SystemSettingsResolver } from 'app/shared/_resolvers/system-settings.resolver';
import { IncidenceTypeDepartmentListResolver } from 'app/shared/_resolvers/incidenceTypeDepartmentListResolver';
import { IncidenceTypeDepartmentListComponent } from './incidenceTypeDepartmentMapping/incidenceTypeDepartmentList/incidenceTypeDepartmentList.component';
import { UnmappedIncidenceTypesResolver } from 'app/shared/_resolvers/umapped-incidencetypes.resolver';
import { IncidenceTypeResolver } from 'app/shared/_resolvers/incidence-type.resolver';
import { UserRolesResolver } from 'app/shared/_resolvers/user-roles.resolver';
import { OrganizationEditComponent } from './organization/organization-edit/organization-edit.component';
import { OrganizationDetailResolver } from 'app/shared/_resolvers/organization-detail.resolver';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'users/new',
        component: UserEditComponent,
        resolve: {
          // userViewData: UserViewResolver,
          countryListData: CountryListResolver,
          stateListData: StateListResolver,
          areaListData: AreaListResolver,
          DeptListData: UserOrganizationDepartmentsResolver,
          OrgListData: UserOrganizationsResolver,
          UserRolesData: UserRolesResolver
        },
        data: {
          title: 'Edit Profile',
          roles: ['Admin']
        }
      },
      {
        path: 'users',
        component: UserListComponent,
        resolve: {
          userListData: UserListResolver,
          countryListData: CountryListResolver,
          stateListData: StateListResolver,
          areaListData: AreaListResolver,
          DeptListData: UserOrganizationDepartmentsResolver,
          OrgListData: UserOrganizationsResolver
        },
        data: {
          title: 'Users',
          roles: ['Admin']
        }
      },
      {
        path: 'users/edit/:id',
        component: UserEditComponent,
        resolve: {
          userViewData: UserViewResolver,
          countryListData: CountryListResolver,
          stateListData: StateListResolver,
          cityListData: CityListResolver,
          areaListData: AreaListResolver,
          DeptListData: UserOrganizationDepartmentsResolver,
          OrgListData: UserOrganizationsResolver,
          UserRolesData: UserRolesResolver
        },
        data: {
          title: 'Edit User'
        }
      },
      {
        path: 'users/:id',
        component: UserViewComponent,
        resolve: {
          userViewData: UserViewResolver,
          countryListData: CountryListResolver,
          stateListData: StateListResolver,
          cityListData: CityListResolver,
          areaListData: AreaListResolver,
          DeptListData: UserOrganizationDepartmentsResolver,
          OrgListData: UserOrganizationsResolver
        },
        data: {
          title: 'Edit Profile'
        }
      },
      {
        path: 'user-roles',
        component: UserManagementComponent,
        resolve: {
          usersRolesListData: UsersRolesDetailListResolver,
          // DeptListData: UserOrganizationDepartmentsResolver,
          // OrgListData: UserOrganizationsResolver,
        },
        data: {
          title: 'User Role Management',
          roles: ['Admin']
        }
      },
      {
        path: 'organizations',
        component: OrganizationListComponent,
        resolve: {
          countryListData: CountryListResolver,
          stateListData: StateListResolver,
          areaListData: AreaListResolver,
          cityListData: CityListResolver,
          orgDetailListData: OrganizationDetailListResolver,
          DeptListData: UserOrganizationDepartmentsResolver,
          orgListData: OrganizationListResolver,
        },
        data: {
          title: 'Organizations',
          roles: ['Admin']
        }
      },
      {
        path: 'organizations/edit',
        component: OrganizationEditComponent,
        resolve: {
          countryListData: CountryListResolver,
          stateListData: StateListResolver,
          areaListData: AreaListResolver,
          cityListData: CityListResolver,
          orgDetailData: OrganizationDetailResolver,
          DeptListData: UserOrganizationDepartmentsResolver,
        },
        data: {
          title: 'Organizations',
          roles: ['Admin']
        }
      },
      {
        path: 'departments',
        component: DepartmentListComponent,
        resolve: {
          deptDetailListData: DepartmentDetailListResolver,
          orgListData: OrganizationListResolver
        },
        data: {
          title: 'Departments',
          roles: ['Admin']
        }
      },
      {
        path: 'incidence-types',
        component: IncidenceTypeListComponent,
        resolve: {
          incidenceTypeDetailListData: IncidenceTypeDetailListResolver
        },
        data: {
          title: 'Incidence Types',
          roles: ['Admin']
        }
      },
      {
        path: 'incidence-statuses',
        component: IncidenceStatusListComponent,
        resolve: {
          incidenceStatusDetailListData: IncidenceStatusDetailListResolver
        },
        data: {
          title: 'Incidence Statuses',
          roles: ['Admin']
        }
      },
      {
        path: 'systemsettings/edit',
        component: SystemSettingsComponent,
        resolve: {
          systemSettingsData: SystemSettingsResolver,
        },
        data: {
          title: 'Edit System Settings'
        }
      },
      {
        path: 'incidenceTypeDepartmentMapping',
        component: IncidenceTypeDepartmentListComponent,
        resolve: {
          incidenceTypeDepartmentListData: IncidenceTypeDepartmentListResolver,
          incidenceTypesData: UnmappedIncidenceTypesResolver,
          DeptListData: UserOrganizationDepartmentsResolver,
          OrgListData: UserOrganizationsResolver
        },
        data: {
          title: 'IncidenceType-Department Mapping'
        }
      },
      {
        path: 'areas',
        component: AreaListComponent,
        resolve: {
          areaDetailListData: AreaDetailListResolver,
          countryListData: CountryListResolver,
          stateListData: StateListResolver,
          cityListData: CityListResolver,
        },
        data: {
          title: 'Areas'
        }
      },
      {
        path: 'cities',
        component: CityListComponent,
        resolve: {
          cityDetailListData: CityDetailListResolver,
          countryListData: CountryListResolver,
          stateListData: StateListResolver,
        },
        data: {
          title: 'Cities'
        }
      },
      {
        path: 'states',
        component: StateListComponent,
        resolve: {
          stateDetailListData: StateDetailListResolver,
          countryListData: CountryListResolver,
        },
        data: {
          title: 'States'
        }
      },
      {
        path: 'countries',
        component: CountryListComponent,
        resolve: {
          countryDetailListData: CountryDetailListResolver
        },
        data: {
          title: 'Countries'
        }
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SetupRoutingModule { }
