import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { MemberRoutingModule } from './member-routing.module';
import { IncidentListComponent } from './incidence/incident-list/incident-list.component';
import { GridModule, PageService, SortService, SearchService, ToolbarService, FilterService, ExcelExportService,
  PdfExportService, EditService, CommandColumnService, ResizeService } from '@syncfusion/ej2-angular-grids';
import { IncidenceDetailListResolver } from 'app/shared/_resolvers/incidence-detail-list.resolver';
import { IncidenceEditComponent } from './incidence/incidence-edit/incidence-edit.component';
import { IncidenceViewResolver } from 'app/shared/_resolvers/incidence-detail.resolver';
import { CountryListResolver } from 'app/shared/_resolvers/country-list.resolver';
import { StateListResolver } from 'app/shared/_resolvers/state-list.resolver';
import { AreaListResolver } from 'app/shared/_resolvers/area-list.resolver';
import { CityListResolver } from 'app/shared/_resolvers/city-list.resolver';
import { UserOrganizationDepartmentsResolver } from 'app/shared/_resolvers/user-department.resolver';
import { UserOrganizationsResolver } from 'app/shared/_resolvers/user-organization.resolver';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { UserKeyValueListResolver } from 'app/shared/_resolvers/user-key-value-list.resolver';
import { IncidenceTypeResolver } from 'app/shared/_resolvers/incidence-type.resolver';
import { IncidenceStatusResolver } from 'app/shared/_resolvers/incidence-status.resolver';
import { BrowserXhr } from '@angular/http';
import { BrowserXhrWithProgress, ProgressService } from 'app/shared/services/progress.service';
import { PhotoService } from 'app/shared/services/photo.service';
import { NgxGalleryModule } from 'ngx-gallery';
import { IncidencePhotosResolver } from 'app/shared/_resolvers/incidence-photos-resolver';
import { UiSwitchModule } from 'ngx-ui-switch';
import { VgCoreModule } from 'videogular2/core';
import { VgControlsModule } from 'videogular2/controls';
import { VgOverlayPlayModule } from 'videogular2/overlay-play';
import { VgBufferingModule } from 'videogular2/buffering';
import { NgImageSliderModule } from 'ng-image-slider';
import { ChartistModule } from 'ng-chartist';
import { IncidenceDashboardReportResolver } from 'app/shared/_resolvers/incidence-dashboard.resolver';
import { ChartsModule } from 'ng2-charts';
import { PhotoEditorComponent } from './photo-editor/photo-editor.component';
import { VideoEditorComponent } from './video-editor/video-editor.component';
import { UserRolesResolver } from 'app/shared/_resolvers/user-roles.resolver';
import { HazardListComponent } from './hazard/hazard-list/hazard-list.component';
import { HazardDetailListResolver } from 'app/shared/_resolvers/hazard-detail-list.resolver';
import { HazardPhotosResolver } from 'app/shared/_resolvers/hazard-photos-resolver';
import { HazardEditComponent } from './hazard/hazard-edit/hazard-edit.component';
import { HazardViewResolver } from 'app/shared/_resolvers/hazard-detail.resolver';
import { IncidenceViewComponent } from './incidence/incidence-view/incidence-view.component';
import { HazardViewComponent } from './hazard/hazard-view/hazard-view.component';

@NgModule({
  imports: [
    CommonModule,
    MemberRoutingModule,
    GridModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    UiSwitchModule,
    BsDatepickerModule.forRoot(),
    VgCoreModule,
    VgControlsModule,
    VgOverlayPlayModule,
    VgBufferingModule,
    NgImageSliderModule,
    ChartistModule,
    ChartsModule
  ],
  declarations: [
    IncidentListComponent,
    IncidenceEditComponent,
    PhotoEditorComponent,
    VideoEditorComponent,
    HazardListComponent,
    HazardEditComponent,
    IncidenceViewComponent,
    HazardViewComponent
  ],
  providers: [
    IncidenceDetailListResolver,
    ExcelExportService,
    PdfExportService,
    PageService,
    SortService,
    SearchService,
    ToolbarService,
    FilterService,
    ResizeService,
    EditService,
    CommandColumnService,
    IncidenceViewResolver,
    CountryListResolver,
    StateListResolver,
    CityListResolver,
    AreaListResolver,
    UserOrganizationDepartmentsResolver,
    UserOrganizationsResolver,
    UserKeyValueListResolver,
    IncidenceTypeResolver,
    IncidenceStatusResolver,
    IncidencePhotosResolver,
    BrowserXhrWithProgress,
    ProgressService,
    PhotoService,
    IncidenceDashboardReportResolver,
    UserRolesResolver,
    HazardDetailListResolver,
    HazardPhotosResolver,
    HazardViewResolver
  ]
})
export class MemberModule { }
