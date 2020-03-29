import { Component, OnInit, ViewChild, AfterViewInit, Renderer } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from 'app/shared/services/user.service';
import { RoleModalComponent } from '../role-modal/role-modal.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { User } from 'app/_models/user';
import { UsersRoles } from 'app/_models/usersRoles';

import { KeyValuePair } from 'app/_models/keyValuePair';

import { PageSettingsModel, GridComponent, ToolbarItems, CommandModel, EditSettingsModel,
  CommandClickEventArgs } from '@syncfusion/ej2-angular-grids';
import { ChangeEventArgs } from '@syncfusion/ej2-inputs';
import { ClickEventArgs } from '@syncfusion/ej2-angular-navigations';
import { IncidenceTypeDepartment } from 'app/_models/incidenceTypeDepartment';
import Swal from 'sweetalert2';
import { IncidenceTypeDepartmentService } from 'app/shared/services/incidencetype-department.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.scss']
})
export class UserManagementComponent implements OnInit {
  queryResult: any = {};
    public pageSettings: PageSettingsModel;
    @ViewChild('grid') public grid: GridComponent;
    public initialPage: PageSettingsModel;
    public toolbarOptions: ToolbarItems[];
    public editSettings: EditSettingsModel;
    public commands: CommandModel[];
    userRoleData: any;
    public data: object[];
    userListData: any[];
    query: any = {
      pageSize: 50
    };

  // check that this conforms with backend
  userPreferredContactList = [{'id': 0, name: 'Email'}, {'id': 1, name: 'SMS'}, {'id': 2, name: 'Phone'}]
  userGenderList = [{'id': 0, name: 'Male'}, {'id': 1, name: 'Female'}]

  constructor(private route: ActivatedRoute, private userService: UserService, private modalService: NgbModal, private renderer: Renderer,
     private router: Router, private toastr: ToastrService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      console.log('users init');
      this.queryResult = data['usersRolesListData']; // userProfileData from route.ts
      console.log(this.queryResult);
    });

    this.pageSettings = { pageSizes: true, pageSize: 10 };
    this.data = this.queryResult.items;
    this.toolbarOptions = ['ExcelExport', 'PdfExport', 'Search', 'Print'];
    this.editSettings = { allowAdding: true, allowDeleting: true };
    // this.populateUsers();
  }

  editRecord(args: any): void {
    this.userRoleData = this.grid.getRowInfo(args.target);
    this.createEditUserRoleModal(this.userRoleData, 'edit')
  }

  // grid page change event
  change(args: ChangeEventArgs) {
    this.initialPage = { currentPage: args.value };
  }

  viewRecord(args: any): void {
    this.userRoleData = this.grid.getRowInfo(args.target);
    this.createEditUserRoleModal(this.userRoleData, 'view')
  }

  toolbarClick(args: ClickEventArgs): void {
    if (args.item.id === 'userListGrid_excelexport') { // 'Grid_excelexport' -> Grid component id + _ + toolbar item name
        this.grid.excelExport();
    }
    if (args.item.id === 'userListGrid_pdfexport') { // 'Grid_pdfexport' -> Grid component id + _ + toolbar item name
      this.grid.pdfExport();
    }
  }

  createEditUserRoleModal(args: any, screenMode: string) {
    const modalRef = this.modalService.open(RoleModalComponent, { size: 'lg' });
    let userRoleDataFromList = {};
    if (args && args !== undefined) {
      this.userRoleData = this.grid.getRowInfo(args.target);
      userRoleDataFromList = this.userRoleData.rowData;
    }
    modalRef.componentInstance.userAndRoleData = userRoleDataFromList;
    modalRef.componentInstance.screenMode = screenMode;
    modalRef.componentInstance.userListOutput.subscribe(($e) => {
      this.queryResult = $e;
    });
  }
}
