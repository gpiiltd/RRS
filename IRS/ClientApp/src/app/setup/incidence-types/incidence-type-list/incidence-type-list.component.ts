import { IncidenceTypeService } from 'app/shared/services/incidence-type.service';
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
import { IncidenceTypeModalComponent } from '../incidence-type-modal/incidence-type-modal.component';

@Component({
  selector: 'app-incidence-type-list',
  templateUrl: './incidence-type-list.component.html',
  styleUrls: ['./incidence-type-list.component.scss']
})
export class IncidenceTypeListComponent implements OnInit {
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
    incidenceTypeData: any;
    departmentList: any[];
    organizationList: any[];
    isGlobalAdminUser = false;
    query: any = {
      pageSize: 50
    };

  constructor(private route: ActivatedRoute, private itService: IncidenceTypeService, private modalService: NgbModal,
    private renderer: Renderer, private router: Router, private toastr: ToastrService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.queryResult = data['incidenceTypeDetailListData'];
      console.log('incidence type list:');
      console.log(this.queryResult);
    });

    this.pageSettings = { pageSizes: true, pageSize: 10 };
    //this.data = this.queryResult.items;
    this.toolbarOptions = ['ExcelExport', 'PdfExport', 'Search', 'Print'];
    this.editSettings = { allowAdding: true, allowDeleting: true };
  }

  editRecord(args: any): void {
    this.incidenceTypeData = this.grid.getRowInfo(args.target);
    this.createEditIncidenceTypeModal(this.incidenceTypeData, 'edit')
  }

  viewRecord(args: any): void {
    this.incidenceTypeData = this.grid.getRowInfo(args.target);
    this.createEditIncidenceTypeModal(this.incidenceTypeData, 'view')
  }

  addRecord(): void {
    this.createEditIncidenceTypeModal(null, 'add')
  }

  deleteRecord(args: any): void {
    // console.log('del');
    this.incidenceTypeData = this.grid.getRowInfo(args.target);
    const recordId = this.incidenceTypeData.rowData.id;
    (Swal as any).fire({
      title: 'Are you sure',
      text: 'You want to delete this record?',
      type: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes'
    }).then((result) => {
      if (result.value) {
        const result$ = this.itService.deleteIncidenceType(recordId);
        result$.subscribe(() => {
          this.populateList();
          (Swal as any).fire(
            'Deleted!',
            'The record has been deleted successfully',
            'success'
          )
        }, error => {
          // console.log(error);
          // console.log(error.error);
          this.toastr.error(error.error.Error[0]);
        });
      }
    });
  }

  // grid page change event
  change(args: ChangeEventArgs) {
    this.initialPage = { currentPage: args.value };
  }

  toolbarClick(args: ClickEventArgs): void {
    if (args.item.id === 'incidenceTypeListGrid_excelexport') { // 'Grid_excelexport' -> Grid component id + _ + toolbar item name
        this.grid.excelExport();
    }
    if (args.item.id === 'incidenceTypeListGrid_pdfexport') { // 'Grid_pdfexport' -> Grid component id + _ + toolbar item name
      this.grid.pdfExport();
    }
  }

  private populateList() {
    this.itService.getIncidenceTypesList(this.query)
      .subscribe(result => {
        this.queryResult = result;
      });
  }

createEditIncidenceTypeModal(args: any, screenMode: string) {
  const modalRef = this.modalService.open(IncidenceTypeModalComponent, { size: 'lg' });
  let itDataFromList = {};
  if (args && args !== undefined) {
    this.incidenceTypeData = this.grid.getRowInfo(args.target);
    itDataFromList = this.incidenceTypeData.rowData;
  }
  modalRef.componentInstance.incidenceTypeData = itDataFromList;
  modalRef.componentInstance.screenMode = screenMode;
  modalRef.componentInstance.incidenceTypeListOutput.subscribe(($e) => {
    this.queryResult = $e;
  });
}
}
