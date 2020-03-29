import { Component, OnInit, ViewChild, AfterViewInit, Renderer } from '@angular/core';
import { KeyValuePair } from 'app/_models/keyValuePair';
import { UserService } from 'app/shared/services/user.service';
import { Router, ActivatedRoute } from '@angular/router';
import { PageSettingsModel, GridComponent, ToolbarItems, CommandModel, EditSettingsModel,
  CommandClickEventArgs } from '@syncfusion/ej2-angular-grids';
import { ChangeEventArgs } from '@syncfusion/ej2-inputs';
import { ClickEventArgs } from '@syncfusion/ej2-angular-navigations';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { IncidenceTypeDepartment } from 'app/_models/incidenceTypeDepartment';
import { IncidencetypeDepartmentModalComponent } from '../incidencetype-department-modal/incidencetype-department-modal.component';
import Swal from 'sweetalert2';
import { IncidenceTypeDepartmentService } from 'app/shared/services/incidencetype-department.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-incidenceTypeDepartmentList',
  templateUrl: './incidenceTypeDepartmentList.component.html',
  styleUrls: ['./incidenceTypeDepartmentList.component.scss']
})
export class IncidenceTypeDepartmentListComponent implements OnInit {
    queryResult: any = {};
    public pageSettings: PageSettingsModel;
    @ViewChild('grid') public grid: GridComponent;
    public initialPage: PageSettingsModel;
    public toolbarOptions: ToolbarItems[];
    public editSettings: EditSettingsModel;
    public commands: CommandModel[];
    isNotPermitted = true;
    incidenceTypeDeptData: any;
    public columns: any;
    public data: object[];
    incidenceTypesListData: any[];
    departmentList: any[];
    organizationList: any[];
    query: any = {
      pageSize: 50
    };


  constructor(private renderer: Renderer, private router: Router, private route: ActivatedRoute, private modalService: NgbModal,
    private itdService: IncidenceTypeDepartmentService, private toastr: ToastrService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.queryResult = data['incidenceTypeDepartmentListData'];
      this.incidenceTypesListData = data['incidenceTypesData'];
      this.departmentList = data['DeptListData'];
      this.organizationList = data['OrgListData'];
    });
    this.pageSettings = { pageSizes: true, pageSize: 10 };
    this.data = this.queryResult.items;
    this.toolbarOptions = ['ExcelExport', 'PdfExport', 'Search', 'Print'];
    this.editSettings = { allowAdding: true, allowDeleting: true };
  }

  deleteRecord(args: any): void {
    // console.log('del');
    this.incidenceTypeDeptData = this.grid.getRowInfo(args.target);
    const recordId = this.incidenceTypeDeptData.rowData.id;
    (Swal as any).fire({
      title: 'Are you sure?',
      text: 'You want to delete this record',
      type: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes'
    }).then((result) => {
      if (result.value) {
        const result$ = this.itdService.deleteIncidenceTypeDepartment(recordId);
        result$.subscribe(() => {
          this.populateList();
          (Swal as any).fire(
            'Deleted!',
            'The record has been deleted successfully',
            'success'
          )
        }, error => {
          this.toastr.error('Error deleting record');
        });
      }
    });
  }

  // grid page change event
  change(args: ChangeEventArgs) {
    this.initialPage = { currentPage: args.value };
  }

  toolbarClick(args: ClickEventArgs): void {
    if (args.item.id === 'incidenceTypeDeptListGrid_excelexport') { // 'Grid_excelexport' -> Grid component id + _ + toolbar item name
        this.grid.excelExport();
    }
    if (args.item.id === 'incidenceTypeDeptListGrid_pdfexport') { // 'Grid_pdfexport' -> Grid component id + _ + toolbar item name
      this.grid.pdfExport();
    }
  }

  private populateList() {
    this.itdService.getIncidenceTypeDepartmentList(this.query)
      .subscribe(result => {
        this.queryResult = result;
      });
  }

  createEditIncidenceTypeDepartmentMappingModal(args: any) {
    const modalRef = this.modalService.open(IncidencetypeDepartmentModalComponent, { size: 'lg' });
    let itdDataFromList = {};
    if (args && args !== undefined) {
      this.incidenceTypeDeptData = this.grid.getRowInfo(args.target);
      itdDataFromList = this.incidenceTypeDeptData.rowData;
    }

    let mode = '';
    if (itdDataFromList && itdDataFromList !== undefined) {
      mode = 'edit';
    } else {
      mode = 'view';
    }
    // areainfo is the Area row passed from the table
    modalRef.componentInstance.incidenceTypesList = this.incidenceTypesListData;
    modalRef.componentInstance.departmentList = this.departmentList;
    // console.log('hi tt');
    // console.log(this.departmentList);
    modalRef.componentInstance.organizationList = this.organizationList;
    modalRef.componentInstance.itdData = itdDataFromList;
    modalRef.componentInstance.screenMode = mode;
    modalRef.componentInstance.itdOutput.subscribe(($e) => {
      this.queryResult = $e;
    });
  }
}
