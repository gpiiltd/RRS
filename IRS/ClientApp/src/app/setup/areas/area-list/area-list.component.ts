import { Component, OnInit, ViewChild, Renderer } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Organization } from 'app/_models/organization';
import { PageSettingsModel, GridComponent, ToolbarItems, CommandModel, EditSettingsModel,
  CommandClickEventArgs, columnSelectionComplete} from '@syncfusion/ej2-angular-grids';
import { ChangeEventArgs } from '@syncfusion/ej2-inputs';
import { ClickEventArgs } from '@syncfusion/ej2-angular-navigations';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';
import { LocationService } from 'app/shared/services/location.service';
import { AreaModalComponent } from '../area-modal/area-modal.component';

@Component({
  selector: 'app-area-list',
  templateUrl: './area-list.component.html',
  styleUrls: ['./area-list.component.scss']
})
export class AreaListComponent implements OnInit {
  queryResult: any = {};
  public pageSettings: PageSettingsModel;
  @ViewChild('grid') public grid: GridComponent;
  public initialPage: PageSettingsModel;
  public toolbarOptions: ToolbarItems[];
  public editSettings: EditSettingsModel;
  public commands: CommandModel[];
  areaData: any;
  query: any = {
    pageSize: 50
  };
// orgList: any[];
  // private readonly PAGE_SIZE = 10;
  countryList: any[];
  stateList: any[];
  cityList: any[];
  constructor(private route: ActivatedRoute, private locService: LocationService, private modalService: NgbModal,
    private renderer: Renderer, private router: Router, private toastr: ToastrService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.queryResult = data['areaDetailListData'];
      console.log(this.queryResult);
      // this.orgList = data['orgListData'];
      this.countryList = data['countryListData'];
      this.stateList = data['stateListData'];
      this.cityList = data['cityListData'];

      this.pageSettings = { pageSizes: true, pageSize: 10 };
      this.toolbarOptions = ['ExcelExport', 'PdfExport', 'Search', 'Print'];
      this.editSettings = { allowAdding: true, allowDeleting: true };
    });
  }

  private populateList() {
      this.locService.getAreaDetailList(this.query)
          .subscribe(result => {
              // console.log(result);
              this.queryResult = result
          });
  }

  editRecord(args: any): void {
    this.areaData = this.grid.getRowInfo(args.target);
    this.createEditAreaModal(this.areaData, 'editMode')
  }

  viewRecord(args: any): void {
    this.areaData = this.grid.getRowInfo(args.target);
    this.createEditAreaModal(this.areaData, 'viewMode')
  }

  addRecord(): void {
    this.createEditAreaModal(null, 'addMode')
  }

  deleteRecord(args: any): void {
    // console.log('del');
    this.areaData = this.grid.getRowInfo(args.target);
    const recordId = this.areaData.rowData.id;
    (Swal as any).fire({
      title: 'Are you sure',
      text: 'You want to delete this record?',
      type: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes'
    }).then((result) => {
      if (result.value) {
        const result$ = this.locService.deleteArea(recordId);
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
    if (args.item.id === 'areaListGrid_excelexport') { // 'Grid_excelexport' -> Grid component id + _ + toolbar item name
        this.grid.excelExport();
    }
    if (args.item.id === 'areaListGrid_pdfexport') { // 'Grid_pdfexport' -> Grid component id + _ + toolbar item name
      this.grid.pdfExport();
    }
  }

  createEditAreaModal(args: any, screenMode: string) {
  const modalRef = this.modalService.open(AreaModalComponent, { windowClass: 'areaModalClass'});
  let areaDataFromList = {};
  if (args && args !== undefined) {
    this.areaData = this.grid.getRowInfo(args.target);
    areaDataFromList = this.areaData.rowData;
  }
  modalRef.componentInstance.areaData = areaDataFromList;
  modalRef.componentInstance.screenMode = screenMode;
  modalRef.componentInstance.countryList = this.countryList;
  modalRef.componentInstance.stateList = this.stateList;
  modalRef.componentInstance.cityList = this.cityList;
  modalRef.componentInstance.areaListOutput.subscribe(($e) => {
    this.queryResult = $e;
  });
}

}
