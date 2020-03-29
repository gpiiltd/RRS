import { Component, OnInit, ViewChild, AfterViewInit, Renderer } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { PageSettingsModel, GridComponent, ToolbarItems, CommandModel, EditSettingsModel,
  CommandClickEventArgs } from '@syncfusion/ej2-angular-grids';
import { ChangeEventArgs } from '@syncfusion/ej2-inputs';
import { ClickEventArgs } from '@syncfusion/ej2-angular-navigations';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { IncidenceService } from 'app/shared/services/incidence.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-incident-list',
  templateUrl: './incident-list.component.html',
  styleUrls: ['./incident-list.component.scss']
})
export class IncidentListComponent implements OnInit {
  query: any = {
    pageSize: 50
  };
  countryList: any[];
  stateList: any[];
  cityList: any[];
  areaId: string;
  loggedInUserRoles: any[];
  // private readonly PAGE_SIZE = 20;
  queryResult: any = {};
  // query: any = {
  //   pageSize: this.PAGE_SIZE
  // };
  public pageSettings: PageSettingsModel;
  @ViewChild('grid') public grid: GridComponent;
  public initialPage: PageSettingsModel;
  public toolbarOptions: ToolbarItems[];
  public editSettings: EditSettingsModel;
  public commands: CommandModel[];
  isNotPermitted = true;
  incidenceData: any;
  isGlobalAdminUser = false;
  isOrgAdminUser = false;
  jwtHelper = new JwtHelperService();

  public data: object[];

  constructor(private renderer: Renderer, private router: Router, private route: ActivatedRoute, private modalService: NgbModal,
    private toastr: ToastrService, private iService: IncidenceService) { }

  ngOnInit() {
    // const jwt = this.getToken();
    this.route.data.subscribe(data => {
      this.queryResult = data['incidenceDetailListData'];

      this.loggedInUserRoles = data['UserRolesData'];
        console.log(this.loggedInUserRoles);
        if (this.loggedInUserRoles) {
          this.isOrgAdminUser = this.loggedInUserRoles.indexOf('Organization Admin') > -1;
        }
    });
    this.getToken();
    this.pageSettings = { pageSizes: true, pageSize: 10 };
    this.data = this.queryResult.items;
    console.log('tada');
    console.log(this.data);
    this.toolbarOptions = ['ExcelExport', 'PdfExport', 'Search', 'Print'];
    this.editSettings = { allowAdding: true, allowDeleting: true };
  }

  // commandClick(args: CommandClickEventArgs): void {
  //   console.log('damn');
  //   alert(JSON.stringify(args.rowData));
  // }

  editRecord(args: any): void {
    this.incidenceData = this.grid.getRowInfo(args.target);
    console.log(this.incidenceData.rowData.id);
    // console.log(ret.);
    this.router.navigate(['incidence/edit/' + this.incidenceData.rowData.id]);
  }

  viewRecord(args: any): void {
    this.incidenceData = this.grid.getRowInfo(args.target);
    console.log(this.incidenceData.rowData.id);
    // console.log(ret.);
    this.router.navigate(['incidence/view/' + this.incidenceData.rowData.id]);
  }

  createRecord(): void {
    this.router.navigate(['incidence/new']);
  }

  getToken() {
    // console.log('startup token :' + localStorage.getItem('token') )
    const userToken = localStorage.getItem('token');
    const decodedToken = this.jwtHelper.decodeToken(userToken);
    if (decodedToken.unique_name === 'Admin') {
      this.isGlobalAdminUser = true;
    }
    return localStorage.getItem('token');
  }

  // grid page change event
  change(args: ChangeEventArgs) {
    this.initialPage = { currentPage: args.value };
  }

  toolbarClick(args: ClickEventArgs): void {
    if (args.item.id === 'incidenceListGrid_excelexport') { // 'Grid_excelexport' -> Grid component id + _ + toolbar item name
        this.grid.excelExport();
    }
    if (args.item.id === 'incidenceListGrid_pdfexport') { // 'Grid_pdfexport' -> Grid component id + _ + toolbar item name
      this.grid.pdfExport();
    }
  }

  private populateList() {
    this.iService.getIncidenceList(this.query)
      .subscribe(result => {
        this.queryResult = result;
      });
  }

  deleteRecord(args: any): void {
    // console.log('del');
    this.incidenceData = this.grid.getRowInfo(args.target);
    const recordId = this.incidenceData.rowData.id;
    (Swal as any).fire({
      title: 'Are you sure',
      text: 'You want to delete this record?',
      type: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes'
    }).then((result) => {
      if (result.value) {
        const result$ = this.iService.deleteIncidence(recordId);
        result$.subscribe(() => {
          this.populateList();
          (Swal as any).fire(
            'Deleted!',
            'The record has been deleted successfully',
            'success'
          )
        }, error => {
          console.log(error);
          console.log(error.error);
          this.toastr.error(error.error);
        });
      }
    });
  }
}
