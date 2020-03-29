import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserProfilePageComponent } from './user-profile/user-profile-page.component';
import { UserProfileEditResolver } from 'app/shared/_resolvers/user-profile-edit.resolver';
import { CountryListResolver } from 'app/shared/_resolvers/country-list.resolver';
import { StateListResolver } from 'app/shared/_resolvers/state-list.resolver';
import { AreaListResolver } from 'app/shared/_resolvers/area-list.resolver';
import { UserOrganizationDepartmentsResolver } from 'app/shared/_resolvers/user-department.resolver';
import { UserOrganizationsResolver } from 'app/shared/_resolvers/user-organization.resolver';
import { UserProfileEditComponent } from './user-profile-edit/user-profile-edit.component';
import { UserViewResolver } from 'app/shared/_resolvers/user-view.resolver';
import { UserRolesResolver } from 'app/shared/_resolvers/user-roles.resolver';
import { CityListResolver } from 'app/shared/_resolvers/city-list.resolver';
import { UserProfilePhotoResolver } from 'app/shared/_resolvers/user-profile-photo.resolver';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'profile',
        component: UserProfilePageComponent,
        resolve: {
          userProfileData: UserProfileEditResolver,
          countryListData: CountryListResolver,
          stateListData: StateListResolver,
          cityListData: CityListResolver,
          areaListData: AreaListResolver,
          DeptListData: UserOrganizationDepartmentsResolver,
          OrgListData: UserOrganizationsResolver,
          UserProfilePhotoData: UserProfilePhotoResolver
        },
        data: {
          title: 'View Profile'
        }
      },
      {
        path: 'profile/edit/:id',
        component: UserProfileEditComponent,
        resolve: {
          userViewData: UserViewResolver,
          countryListData: CountryListResolver,
          stateListData: StateListResolver,
          cityListData: CityListResolver,
          areaListData: AreaListResolver,
          DeptListData: UserOrganizationDepartmentsResolver,
          OrgListData: UserOrganizationsResolver,
          UserRolesData: UserRolesResolver,
          UserProfilePhotoData: UserProfilePhotoResolver
        },
        data: {
          title: 'Edit Profile'
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserprofileRoutingModule { }
