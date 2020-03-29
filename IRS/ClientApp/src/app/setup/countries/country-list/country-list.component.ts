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
import { CountryModalComponent } from '../country-modal/country-modal.component';

@Component({
  selector: 'app-country-list',
  templateUrl: './country-list.component.html',
  styleUrls: ['./country-list.component.scss']
})
export class CountryListComponent implements OnInit {
queryResult: any = {};
public pageSettings: PageSettingsModel;
@ViewChild('grid') public grid: GridComponent;
public initialPage: PageSettingsModel;
public toolbarOptions: ToolbarItems[];
public editSettings: EditSettingsModel;
public commands: CommandModel[];
countryData: any;
query: any = {
  pageSize: 50
};
constructor(private route: ActivatedRoute, private locService: LocationService, private modalService: NgbModal,
  private renderer: Renderer, private router: Router, private toastr: ToastrService) { }

ngOnInit() {
  this.route.data.subscribe(data => {
    this.queryResult = data['countryDetailListData'];
    console.log(this.queryResult);

    this.pageSettings = { pageSizes: true, pageSize: 10 };
    this.toolbarOptions = ['ExcelExport', 'PdfExport', 'Search', 'Print'];
    this.editSettings = { allowAdding: true, allowDeleting: true };
  });
}

private populateList() {
    this.locService.getCountryDetailList(this.query)
        .subscribe(result => {
            // console.log(result);
            this.queryResult = result
        });
}

editRecord(args: any): void {
  this.countryData = this.grid.getRowInfo(args.target);
  this.createEditCountryModal(this.countryData, 'editMode')
}

viewRecord(args: any): void {
  this.countryData = this.grid.getRowInfo(args.target);
  this.createEditCountryModal(this.countryData, 'viewMode')
}

addRecord(): void {
  this.createEditCountryModal(null, 'addMode')
}

deleteRecord(args: any): void {
  // console.log('del');
  this.countryData = this.grid.getRowInfo(args.target);
  const recordId = this.countryData.rowData.id;
  (Swal as any).fire({
    title: 'Are you sure',
    text: 'You want to delete this record?',
    type: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Yes'
  }).then((result) => {
    if (result.value) {
      const result$ = this.locService.deleteCountry(recordId);
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
  if (args.item.id === 'countryListGrid_excelexport') { // 'Grid_excelexport' -> Grid component id + _ + toolbar item name
      this.grid.excelExport();
  }
  if (args.item.id === 'countryListGrid_pdfexport') { // 'Grid_pdfexport' -> Grid component id + _ + toolbar item name
    this.grid.pdfExport();
  }
}

createEditCountryModal(args: any, screenMode: string) {
const modalRef = this.modalService.open(CountryModalComponent, { windowClass: 'countryModalClass'});
let countryDataFromList = {};
if (args && args !== undefined) {
  this.countryData = this.grid.getRowInfo(args.target);
  countryDataFromList = this.countryData.rowData;
}
modalRef.componentInstance.countryData = countryDataFromList;
modalRef.componentInstance.screenMode = screenMode;
modalRef.componentInstance.countryListOutput.subscribe(($e) => {
  this.queryResult = $e;
});
}
}
