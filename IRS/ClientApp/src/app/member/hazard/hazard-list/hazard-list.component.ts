import { Component, OnInit, ViewChild, AfterViewInit, Renderer } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { PageSettingsModel, GridComponent, ToolbarItems, CommandModel, EditSettingsModel,
  CommandClickEventArgs } from '@syncfusion/ej2-angular-grids';
import { ChangeEventArgs } from '@syncfusion/ej2-inputs';
import { ClickEventArgs } from '@syncfusion/ej2-angular-navigations';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { HazardService } from 'app/shared/services/hazard.services';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-hazard-list',
  templateUrl: './hazard-list.component.html',
  styleUrls: ['./hazard-list.component.scss']
})
export class HazardListComponent implements OnInit {
  query: any = {
    pageSize: 50
  };
  countryList: any[];
  stateList: any[];
  cityList: any[];
  areaId: string;
  isGlobalAdminUser = false;
  jwtHelper = new JwtHelperService();
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
  hazardData: any;
  isOrgAdminUser = false;

  public data: object[];

  constructor(private renderer: Renderer, private router: Router, private route: ActivatedRoute, private modalService: NgbModal,
    private toastr: ToastrService, private hazardService: HazardService) { }

  ngOnInit() {
    // const jwt = this.getToken();
    this.route.data.subscribe(data => {
      this.queryResult = data['hazardDetailListData'];

      this.loggedInUserRoles = data['UserRolesData'];
        console.log(this.loggedInUserRoles);
        if (this.loggedInUserRoles) {
          this.isOrgAdminUser = this.loggedInUserRoles.indexOf('Organization Admin') > -1;
        }
    });
    this.pageSettings = { pageSizes: true, pageSize: 10 };
    this.data = this.queryResult.items;
    this.getToken();
    console.log('tada');
    console.log(this.data);
    this.toolbarOptions = ['ExcelExport', 'PdfExport', 'Search', 'Print'];
    this.editSettings = { allowAdding: true, allowDeleting: true };
  }

  editRecord(args: any): void {
    this.hazardData = this.grid.getRowInfo(args.target);
    console.log(this.hazardData.rowData.id);
    // console.log(ret.);
    this.router.navigate(['hazard/edit/' + this.hazardData.rowData.id]);
  }

  viewRecord(args: any): void {
    this.hazardData = this.grid.getRowInfo(args.target);
    console.log(this.hazardData.rowData.id);
    // console.log(ret.);
    this.router.navigate(['hazard/view/' + this.hazardData.rowData.id]);
  }

  createRecord(): void {
    this.router.navigate(['hazard/new']);
  }

  // grid page change event
  change(args: ChangeEventArgs) {
    this.initialPage = { currentPage: args.value };
  }

  toolbarClick(args: ClickEventArgs): void {
    if (args.item.id === 'hazardListGrid_excelexport') { // 'Grid_excelexport' -> Grid component id + _ + toolbar item name
        this.grid.excelExport();
    }
    if (args.item.id === 'hazardListGrid_pdfexport') { // 'Grid_pdfexport' -> Grid component id + _ + toolbar item name
      this.grid.pdfExport();
    }
  }

  private populateList() {
    this.hazardService.getHazardList(this.query)
      .subscribe(result => {
        this.queryResult = result;
      });
  }

  deleteRecord(args: any): void {
    // console.log('del');
    this.hazardData = this.grid.getRowInfo(args.target);
    const recordId = this.hazardData.rowData.id;
    (Swal as any).fire({
      title: 'Are you sure',
      text: 'You want to delete this record?',
      type: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes'
    }).then((result) => {
      if (result.value) {
        const result$ = this.hazardService.deleteHazard(recordId);
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

  getToken() {
    // console.log('startup token :' + localStorage.getItem('token') )
    const userToken = localStorage.getItem('token');
    const decodedToken = this.jwtHelper.decodeToken(userToken);
    if (decodedToken.unique_name === 'Admin') {
      this.isGlobalAdminUser = true;
    }
    return localStorage.getItem('token');
  }
}
