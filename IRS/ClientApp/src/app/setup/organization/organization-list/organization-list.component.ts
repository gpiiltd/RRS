import { Component, OnInit, ViewChild, Renderer } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrganizationService } from 'app/shared/services/organization.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Organization } from 'app/_models/organization';
import { OrganizationModalComponent } from '../organization-modal/organization-modal.component';
import { PageSettingsModel, GridComponent, ToolbarItems, CommandModel, EditSettingsModel,
  CommandClickEventArgs, columnSelectionComplete} from '@syncfusion/ej2-angular-grids';
import { ChangeEventArgs } from '@syncfusion/ej2-inputs';
import { ClickEventArgs } from '@syncfusion/ej2-angular-navigations';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-organization-list',
  templateUrl: './organization-list.component.html',
  styleUrls: ['./organization-list.component.scss']
})
export class OrganizationListComponent implements OnInit {
  queryResult: any = {};
  public pageSettings: PageSettingsModel;
  @ViewChild('grid') public grid: GridComponent;
  public initialPage: PageSettingsModel;
  public toolbarOptions: ToolbarItems[];
  public editSettings: EditSettingsModel;
  public commands: CommandModel[];
  orgData: any;
  query: any = {
    pageSize: 50
  };
// orgList: any[];
  // private readonly PAGE_SIZE = 10;
  countryList: any[];
  stateList: any[];
  cityList: any[];
  areaList: any[];
  departmentList: any[];
  orgList: any[];
  constructor(private route: ActivatedRoute, private orgService: OrganizationService, private modalService: NgbModal,
    private renderer: Renderer, private router: Router, private toastr: ToastrService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.queryResult = data['orgDetailListData'];
      console.log(this.queryResult);
      this.countryList = data['countryListData'];
      this.stateList = data['stateListData'];
      this.cityList = data['cityListData'];
      this.areaList = data['areaListData'];
      this.departmentList = data['DeptListData'];
      this.orgList = data['orgListData'];
      this.pageSettings = { pageSizes: true, pageSize: 10 };
      this.toolbarOptions = ['ExcelExport', 'PdfExport', 'Search', 'Print'];
      this.editSettings = { allowAdding: true, allowDeleting: true };
    });
  }

  private populateList() {
      this.orgService.getOrganizationDetailList(this.query)
          .subscribe(result => {
              // console.log(result);
              this.queryResult = result
          });
  }

  editRecord(args: any): void {
    this.orgData = this.grid.getRowInfo(args.target);
    this.createEditOrganizationModal(this.orgData, 'edit')
  }

  viewRecord(args: any): void {
    this.orgData = this.grid.getRowInfo(args.target);
    this.createEditOrganizationModal(this.orgData, 'view')
  }

  addRecord(): void {
    this.createEditOrganizationModal(null, 'add')
  }

  deleteRecord(args: any): void {
    // console.log('del');
    this.orgData = this.grid.getRowInfo(args.target);
    const recordId = this.orgData.rowData.id;
    (Swal as any).fire({
      title: 'Are you sure',
      text: 'You want to delete this record?',
      type: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes'
    }).then((result) => {
      if (result.value) {
        const result$ = this.orgService.deleteOrganization(recordId);
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
    if (args.item.id === 'organizationListGrid_excelexport') { // 'Grid_excelexport' -> Grid component id + _ + toolbar item name
        this.grid.excelExport();
    }
    if (args.item.id === 'organizationListGrid_pdfexport') { // 'Grid_pdfexport' -> Grid component id + _ + toolbar item name
      this.grid.pdfExport();
    }
  }

  createEditOrganizationModal(args: any, screenMode: string) {
  const modalRef = this.modalService.open(OrganizationModalComponent, { windowClass: 'orgModalClass'});
  let orgDataFromList = {};
  if (args && args !== undefined) {
    this.orgData = this.grid.getRowInfo(args.target);
    orgDataFromList = this.orgData.rowData;
  }
  modalRef.componentInstance.orgData = orgDataFromList;
  modalRef.componentInstance.screenMode = screenMode;
  modalRef.componentInstance.countryList = this.countryList;
  modalRef.componentInstance.stateList = this.stateList;
  modalRef.componentInstance.cityList = this.cityList;
  modalRef.componentInstance.areaList = this.areaList;
  modalRef.componentInstance.departmentList = this.departmentList;
  modalRef.componentInstance.orgList = this.orgList;
  modalRef.componentInstance.orgListOutput.subscribe(($e) => {
    this.queryResult = $e;
  });
}
}
