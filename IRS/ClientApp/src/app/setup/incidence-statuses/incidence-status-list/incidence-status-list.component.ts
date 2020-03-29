import { IncidenceTypeService } from 'app/shared/services/incidence-type.service';
import { IncidenceStatusService } from 'app/shared/services/incidence-status.service';
import { IncidenceStatus } from 'app/_models/incidenceStatus';
import { IncidenceStatusModalComponent } from '../incidence-status-modal/incidence-status-modal.component';
import { Component, OnInit, ViewChild, AfterViewInit, Renderer } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { PageSettingsModel, GridComponent, ToolbarItems, CommandModel, EditSettingsModel,
  CommandClickEventArgs, 
  columnSelectionComplete} from '@syncfusion/ej2-angular-grids';
import { ChangeEventArgs } from '@syncfusion/ej2-inputs';
import { ClickEventArgs } from '@syncfusion/ej2-angular-navigations';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'app/shared/services/user.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-incidence-status-list',
  templateUrl: './incidence-status-list.component.html',
  styleUrls: ['./incidence-status-list.component.scss']
})
export class IncidenceStatusListComponent implements OnInit {
  queryResult: any = {};
    public pageSettings: PageSettingsModel;
    @ViewChild('grid') public grid: GridComponent;
    public initialPage: PageSettingsModel;
    public toolbarOptions: ToolbarItems[];
    public editSettings: EditSettingsModel;
    public commands: CommandModel[];
    isNotPermitted = true;
    public columns: any;
    public data: object[];
    incidenceStatusData: any;
    departmentList: any[];
    organizationList: any[];
    isGlobalAdminUser = false;
    query: any = {
      pageSize: 50
    };
    jwtHelper = new JwtHelperService();

  constructor(private isService: IncidenceStatusService, private renderer: Renderer, private router: Router, private route: ActivatedRoute,
     private modalService: NgbModal, private toastr: ToastrService, private userService: UserService) { }

  ngOnInit() {
    console.log('incidence status list:');
    this.route.data.subscribe(data => {
      this.queryResult = data['incidenceStatusDetailListData'];
      this.getToken();
      // console.log(this.queryResult);
    });

    this.pageSettings = { pageSizes: true, pageSize: 10 };
    this.data = this.queryResult.items;
    this.toolbarOptions = ['ExcelExport', 'PdfExport', 'Search', 'Print'];
    this.editSettings = { allowAdding: true, allowDeleting: true };
  }

  editRecord(args: any): void {
    this.incidenceStatusData = this.grid.getRowInfo(args.target);
    this.createEditIncidenceStatusModal(this.incidenceStatusData, 'edit')
  }

  viewRecord(args: any): void {
    this.incidenceStatusData = this.grid.getRowInfo(args.target);
    this.createEditIncidenceStatusModal(this.incidenceStatusData, 'view')
  }

  addRecord(): void {
    this.createEditIncidenceStatusModal(null, 'add')
  }

  deleteRecord(args: any): void {
    // console.log('del');
    this.incidenceStatusData = this.grid.getRowInfo(args.target);
    const recordId = this.incidenceStatusData.rowData.id;
    console.log('del');
    console.log(recordId);
    (Swal as any).fire({
      title: 'Are you sure',
      text: 'You want to delete this record?',
      type: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes'
    }).then((result) => {
      if (result.value) {
        const result$ = this.isService.deleteIncidenceStatus(recordId);
        result$.subscribe(() => {
          this.populateList();
          (Swal as any).fire(
            'Deleted!',
            'The record has been deleted successfully',
            'success'
          )
        }, error => {
          this.toastr.error(error.error.Error[0]);
        });
      }
    });
  }

  getToken() {
    const userToken = localStorage.getItem('token');
    const decodedToken = this.jwtHelper.decodeToken(userToken);
    if (decodedToken.unique_name === 'Admin') {
      this.isGlobalAdminUser = true;
    }
    // console.log(this.isGlobalAdminUser);
  }

  // grid page change event
  change(args: ChangeEventArgs) {
    this.initialPage = { currentPage: args.value };
  }

  toolbarClick(args: ClickEventArgs): void {
    if (args.item.id === 'incidenceStatusListGrid_excelexport') { // 'Grid_excelexport' -> Grid component id + _ + toolbar item name
        this.grid.excelExport();
    }
    if (args.item.id === 'incidenceStatusListGrid_pdfexport') { // 'Grid_pdfexport' -> Grid component id + _ + toolbar item name
      this.grid.pdfExport();
    }
  }

  private populateList() {
    this.isService.getIncidenceStatusList(this.query)
      .subscribe(result => {
        this.queryResult = result;
      });
  }

  // redundant method: get type of Admin from token. Global Admin username = Admin
  // private checkIfGlobalAdmin() {
  //   this.userService.islobalAdminUser()
  //     .subscribe(result => {
  //       this.isGlobalAdminUser = result;
  //     });
  // }

  createEditIncidenceStatusModal(args: any, screenMode: string) {
    const modalRef = this.modalService.open(IncidenceStatusModalComponent, { size: 'lg' });
    let isDataFromList = {};
    if (args && args !== undefined) {
      this.incidenceStatusData = this.grid.getRowInfo(args.target);
      isDataFromList = this.incidenceStatusData.rowData;
    }
    modalRef.componentInstance.incidenceStatusData = isDataFromList;
    modalRef.componentInstance.screenMode = screenMode;
    modalRef.componentInstance.incidenceStatusListOutput.subscribe(($e) => {
      this.queryResult = $e;
    });
  }
}
