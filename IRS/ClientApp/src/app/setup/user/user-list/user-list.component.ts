import { KeyValuePair } from 'app/_models/keyValuePair';
import { UserService } from 'app/shared/services/user.service';
import { Component, OnInit, ViewChild, AfterViewInit, Renderer } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { PageSettingsModel, GridComponent, ToolbarItems, CommandModel, EditSettingsModel,
  CommandClickEventArgs } from '@syncfusion/ej2-angular-grids';
import { ChangeEventArgs } from '@syncfusion/ej2-inputs';
import { ClickEventArgs } from '@syncfusion/ej2-angular-navigations';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  // private readonly PAGE_SIZE = 50;
  queryResult: any = {};
  public pageSettings: PageSettingsModel;
  @ViewChild('grid') public grid: GridComponent;
  public initialPage: PageSettingsModel;
  public toolbarOptions: ToolbarItems[];
  public editSettings: EditSettingsModel;
  public commands: CommandModel[];
  isNotPermitted = true;
  userData: any;
  public columns: any;
  query: any = {
    pageSize: 50
  };

  public data: object[];

  // check that this conforms with backend
  userPreferredContactList = [{'id': 0, name: 'Email'}, {'id': 1, name: 'SMS'}, {'id': 2, name: 'Phone'}]
  userGenderList = [{'id': 0, name: 'Male'}, {'id': 1, name: 'Female'}]

  constructor(private renderer: Renderer, private router: Router, private route: ActivatedRoute, private userService: UserService,
    private toastr: ToastrService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.queryResult = data['userListData'];
    });
    console.log('tada');
    console.log(this.queryResult);
    this.pageSettings = { pageSizes: true, pageSize: 10 };
    this.data = this.queryResult.items;
    console.log(this.data);
    this.toolbarOptions = ['ExcelExport', 'PdfExport', 'Search', 'Print'];
    this.editSettings = { allowAdding: true, allowDeleting: true };
  }

  editRecord(args: any): void {
    this.userData = this.grid.getRowInfo(args.target);
    console.log(this.userData.rowData.id);
    // console.log(ret.);
    this.router.navigate(['setup/users/edit/' + this.userData.rowData.id]);
  }

  viewRecord(args: any): void {
    this.userData = this.grid.getRowInfo(args.target);
    console.log(this.userData.rowData.id);
    // console.log(ret.);
    this.router.navigate(['setup/users/' + this.userData.rowData.id]);
  }

  createRecord(): void {
    this.router.navigate(['setup/users/new']);
  }

  // deleteRecord(args: any): void {
  //   console.log('del');
  //   alert(JSON.stringify(args.rowData));
  // }

  getToken() {
    console.log('startup token :' + localStorage.getItem('token') )
    return localStorage.getItem('token');
  }

  // grid page change event
  change(args: ChangeEventArgs) {
    this.initialPage = { currentPage: args.value };
  }

  toolbarClick(args: ClickEventArgs): void {
    if (args.item.id === 'userListGrid_excelexport') { // 'Grid_excelexport' -> Grid component id + _ + toolbar item name
        this.grid.excelExport();
    }
    if (args.item.id === 'userListGrid_pdfexport') { // 'Grid_pdfexport' -> Grid component id + _ + toolbar item name
      this.grid.pdfExport();
    }
  }

  private populateList() {
    this.userService.getUserList(this.query)
      .subscribe(result => {
        this.queryResult = result;
      });
  }

  deleteRecord(args: any): void {
    // console.log('del');
    this.userData = this.grid.getRowInfo(args.target);
    const recordId = this.userData.rowData.id;
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
        const result$ = this.userService.deleteUser(recordId);
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

}
