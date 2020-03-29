import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserprofileRoutingModule } from './userprofile-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { UserViewResolver } from 'app/shared/_resolvers/user-view.resolver';
import { CountryListResolver } from 'app/shared/_resolvers/country-list.resolver';
import { StateListResolver } from 'app/shared/_resolvers/state-list.resolver';
import { AreaListResolver } from 'app/shared/_resolvers/area-list.resolver';
import { UserOrganizationDepartmentsResolver } from 'app/shared/_resolvers/user-department.resolver';
import { UserOrganizationsResolver } from 'app/shared/_resolvers/user-organization.resolver';
import { UserProfileEditComponent } from './user-profile-edit/user-profile-edit.component';
import { UserProfilePageComponent } from './user-profile/user-profile-page.component';
import { UserProfileEditResolver } from 'app/shared/_resolvers/user-profile-edit.resolver';
import { UserRolesResolver } from 'app/shared/_resolvers/user-roles.resolver';
import { NgxPasswordToggleModule } from 'ngx-password-toggle';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { CityListResolver } from 'app/shared/_resolvers/city-list.resolver';
import { UserProfilePhotoResolver } from 'app/shared/_resolvers/user-profile-photo.resolver';
import { PhotoService } from 'app/shared/services/photo.service';

@NgModule({
  imports: [
    CommonModule,
    UserprofileRoutingModule,
    FormsModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    NgxPasswordToggleModule,
    BsDatepickerModule.forRoot(),
  ],
  declarations: [
    UserProfilePageComponent ,
    UserProfileEditComponent
  ],
  providers: [
    UserProfileEditResolver,
    UserViewResolver,
    CountryListResolver,
    StateListResolver,
    CityListResolver,
    AreaListResolver,
    UserOrganizationDepartmentsResolver,
    UserOrganizationsResolver,
    UserRolesResolver,
    UserProfilePhotoResolver,
    PhotoService
  ]
})
export class UserprofileModule { }
