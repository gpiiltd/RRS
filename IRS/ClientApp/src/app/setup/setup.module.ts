import { CountryListComponent } from './countries/country-list/country-list.component';
import { AreaDetailListResolver } from './../shared/_resolvers/area-detail-list.resolver';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

import { SetupRoutingModule } from './setup-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ChartistModule } from 'ng-chartist';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { UserListComponent } from './user/user-list/user-list.component';
import { UserViewComponent } from './user/user-view/user-view.component';
import { UserEditComponent } from './user/user-edit/user-edit.component';
import { UserViewResolver } from 'app/shared/_resolvers/user-view.resolver';
import { UserListResolver } from 'app/shared/_resolvers/user-list.resolver';
import { CountryListResolver } from 'app/shared/_resolvers/country-list.resolver';
import { StateListResolver } from 'app/shared/_resolvers/state-list.resolver';
import { AreaListResolver } from 'app/shared/_resolvers/area-list.resolver';
import { UserOrganizationDepartmentsResolver } from 'app/shared/_resolvers/user-department.resolver';
import { UserOrganizationsResolver } from 'app/shared/_resolvers/user-organization.resolver';
import { PaginationComponent } from 'app/shared/pagination.component';
import { AreaListComponent } from './areas/area-list/area-list.component';
import { StateListComponent } from './states/state-list/state-list.component';
import { StateDetailListResolver } from 'app/shared/_resolvers/state-detail-list.resolver';
import { CountryDetailListResolver } from 'app/shared/_resolvers/country-detail-list.resolver';
import { CityListComponent } from './cities/city-list/city-list.component';
import { CityDetailListResolver } from 'app/shared/_resolvers/city-detail-list.resolver';
import { OrganizationListComponent } from './organization/organization-list/organization-list.component';
import { CityListResolver } from 'app/shared/_resolvers/city-list.resolver';
import { OrganizationDetailListResolver } from 'app/shared/_resolvers/organization-detail-list.resolver';
import { DepartmentDetailListResolver } from 'app/shared/_resolvers/department-detail-list.resolver';
import { DepartmentListComponent } from './department/department-list/department-list.component';
import { IncidenceTypeListComponent } from './incidence-types/incidence-type-list/incidence-type-list.component';
import { IncidenceTypeDetailListResolver } from 'app/shared/_resolvers/incidence-type-detail-list.resolver';
import { IncidenceStatusListComponent } from './incidence-statuses/incidence-status-list/incidence-status-list.component';
import { IncidenceStatusDetailListResolver } from 'app/shared/_resolvers/incidence-status-detail-list.resolver';
import { UsersRolesDetailListResolver } from 'app/shared/_resolvers/users-roles-detail-list.resolver';
import { UserManagementComponent } from './user-role-management/user-management/user-management.component';
import { RoleModalComponent } from './user-role-management/role-modal/role-modal.component';
import { AreaModalComponent } from './areas/area-modal/area-modal.component';
import { CityModalComponent } from './cities/city-modal/city-modal.component';
import { StateModalComponent } from './states/state-modal/state-modal.component';
import { CountryModalComponent } from './countries/country-modal/country-modal.component';
import { IncidenceStatusModalComponent } from './incidence-statuses/incidence-status-modal/incidence-status-modal.component';
import { IncidenceTypeModalComponent } from './incidence-types/incidence-type-modal/incidence-type-modal.component';
import { ExportAsModule } from 'ngx-export-as';
import { DepartmentModalComponent } from './department/department-modal/department-modal.component';
import { OrganizationListResolver } from 'app/shared/_resolvers/organization-list.resolver';
import { OrganizationModalComponent } from './organization/organization-modal/organization-modal.component';
import { GridModule, PageService, SortService, SearchService, ToolbarService, FilterService, ExcelExportService,
  PdfExportService, EditService, CommandColumnService, ResizeService, DetailRowService } from '@syncfusion/ej2-angular-grids';
import { SystemSettingsComponent } from './system-settings/system-settings.component';
import { SystemSettingsResolver } from 'app/shared/_resolvers/system-settings.resolver';
import { IncidenceTypeDepartmentListResolver } from 'app/shared/_resolvers/incidenceTypeDepartmentListResolver';
import { IncidenceTypeDepartmentListComponent } from './incidenceTypeDepartmentMapping/incidenceTypeDepartmentList/incidenceTypeDepartmentList.component';
import { IncidencetypeDepartmentModalComponent } from './incidenceTypeDepartmentMapping/incidencetype-department-modal/incidencetype-department-modal.component';
import { UnmappedIncidenceTypesResolver } from 'app/shared/_resolvers/umapped-incidencetypes.resolver';
import { IncidenceTypeResolver } from 'app/shared/_resolvers/incidence-type.resolver';
import { UserRolesResolver } from 'app/shared/_resolvers/user-roles.resolver';
import { OrganizationEditComponent } from './organization/organization-edit/organization-edit.component';
import { OrganizationDetailResolver } from 'app/shared/_resolvers/organization-detail.resolver';

import { NumericTextBoxAllModule } from '@syncfusion/ej2-angular-inputs';

@NgModule({
  imports: [
    CommonModule,
    GridModule,
    SetupRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    ExportAsModule,
    BsDatepickerModule.forRoot(),
    NumericTextBoxAllModule
  ],
  declarations: [
    UserListComponent,
    UserViewComponent,
    UserEditComponent,
    PaginationComponent,
    CityListComponent,
    AreaListComponent,
    StateListComponent,
    CountryListComponent,
    OrganizationListComponent,
    DepartmentListComponent,
    IncidenceTypeListComponent,
    IncidenceStatusListComponent,
    UserManagementComponent,
    RoleModalComponent,
    AreaModalComponent,
    CityModalComponent,
    StateModalComponent,
    CountryModalComponent,
    IncidenceStatusModalComponent,
    IncidenceTypeModalComponent,
    DepartmentModalComponent,
    OrganizationModalComponent,
    SystemSettingsComponent,
    IncidenceTypeDepartmentListComponent,
    IncidencetypeDepartmentModalComponent,
    OrganizationEditComponent
],
entryComponents: [
  RoleModalComponent,
  AreaModalComponent,
  CityModalComponent,
  StateModalComponent,
  CountryModalComponent,
  IncidenceStatusModalComponent,
  IncidenceTypeModalComponent,
  DepartmentModalComponent,
  OrganizationModalComponent,
  IncidencetypeDepartmentModalComponent
],
providers: [
    ExcelExportService,
    PdfExportService,
    PageService,
    SortService,
    SearchService,
    ToolbarService,
    FilterService,
    EditService,
    CommandColumnService,
    UserViewResolver,
    UserListResolver,
    CountryListResolver,
    StateListResolver,
    AreaListResolver,
    CityListResolver,
    CityDetailListResolver,
    AreaDetailListResolver,
    StateDetailListResolver,
    UserOrganizationDepartmentsResolver,
    UserOrganizationsResolver,
    CountryDetailListResolver,
    OrganizationDetailListResolver,
    DepartmentDetailListResolver,
    IncidenceTypeDetailListResolver,
    IncidenceStatusDetailListResolver,
    UsersRolesDetailListResolver,
    OrganizationListResolver,
    SystemSettingsResolver,
    IncidenceTypeDepartmentListResolver,
    IncidenceTypeResolver,
    UserRolesResolver,
    OrganizationDetailResolver,
    ResizeService,
    UnmappedIncidenceTypesResolver,
    DetailRowService
  ]
})
export class SetupModule { }
