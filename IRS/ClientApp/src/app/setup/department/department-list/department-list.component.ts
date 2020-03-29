import { DepartmentService } from 'app/shared/services/department.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DepartmentModalComponent } from '../department-modal/department-modal.component';
import { IncidenceTypeService } from 'app/shared/services/incidence-type.service';
import { Component, OnInit, ViewChild, Renderer } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { PageSettingsModel, GridComponent, ToolbarItems, CommandModel, EditSettingsModel,
  CommandClickEventArgs, columnSelectionComplete} from '@syncfusion/ej2-angular-grids';
import { ChangeEventArgs } from '@syncfusion/ej2-inputs';
import { ClickEventArgs } from '@syncfusion/ej2-angular-navigations';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-department-list',
  templateUrl: './department-list.component.html',
  styleUrls: ['./department-list.component.scss']
})
export class DepartmentListComponent implements OnInit {
  queryResult: any = {};
    public pageSettings: PageSettingsModel;
    @ViewChild('grid') public grid: GridComponent;
    public initialPage: PageSettingsModel;
    public toolbarOptions: ToolbarItems[];
    public editSettings: EditSettingsModel;
    public commands: CommandModel[];
    deptData: any;
    query: any = {
      pageSize: 50
    };
  orgList: any[];

  constructor(private route: ActivatedRoute, private deptService: DepartmentService, private modalService: NgbModal,
    private renderer: Renderer, private router: Router, private toastr: ToastrService) { }

  ngOnInit() {
    console.log('dept');
    this.route.data.subscribe(data => {
      this.queryResult = data['deptDetailListData'];
      console.log(this.queryResult);
      // this.orgList = data['orgListData'];

      this.pageSettings = { pageSizes: true, pageSize: 10 };
      this.toolbarOptions = ['ExcelExport', 'PdfExport', 'Search', 'Print'];
      this.editSettings = { allowAdding: true, allowDeleting: true };
    });
  }

  editRecord(args: any): void {
    this.deptData = this.grid.getRowInfo(args.target);
    this.createEditDepartmentModal(this.deptData, 'edit')
  }

  viewRecord(args: any): void {
    this.deptData = this.grid.getRowInfo(args.target);
    this.createEditDepartmentModal(this.deptData, 'view')
  }

  addRecord(): void {
    this.createEditDepartmentModal(null, 'add')
  }

  deleteRecord(args: any): void {
    // console.log('del');
    this.deptData = this.grid.getRowInfo(args.target);
    const recordId = this.deptData.rowData.id;
    (Swal as any).fire({
      title: 'Are you sure',
      text: 'You want to delete this record?',
      type: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes'
    }).then((result) => {
      if (result.value) {
        const result$ = this.deptService.deleteDepartment(recordId);
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

  // grid page change event
  change(args: ChangeEventArgs) {
    this.initialPage = { currentPage: args.value };
  }

  toolbarClick(args: ClickEventArgs): void {
    if (args.item.id === 'departmentListGrid_excelexport') { // 'Grid_excelexport' -> Grid component id + _ + toolbar item name
        this.grid.excelExport();
    }
    if (args.item.id === 'departmentListGrid_pdfexport') { // 'Grid_pdfexport' -> Grid component id + _ + toolbar item name
      this.grid.pdfExport();
    }
  }

  private populateList() {
    this.deptService.getDepartmentDetailList(this.query)
      .subscribe(result => {
        this.queryResult = result;
      });
  }

  createEditDepartmentModal(args: any, screenMode: string) {
  const modalRef = this.modalService.open(DepartmentModalComponent, { windowClass: 'deptModalClass'});
  let itDataFromList = {};
  if (args && args !== undefined) {
    this.deptData = this.grid.getRowInfo(args.target);
    itDataFromList = this.deptData.rowData;
  }
  modalRef.componentInstance.deptData = itDataFromList;
  modalRef.componentInstance.screenMode = screenMode;
  modalRef.componentInstance.deptListOutput.subscribe(($e) => {
    this.queryResult = $e;
  });
}
}
