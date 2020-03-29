import { Component, OnInit, ViewChild, Renderer } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Organization } from 'app/_models/organization';
import { PageSettingsModel, GridComponent, ToolbarItems, CommandModel, EditSettingsModel,
  CommandClickEventArgs, columnSelectionComplete, GridModel, AddEventArgs, DialogEditEventArgs, SaveEventArgs } from '@syncfusion/ej2-angular-grids';
import { ChangeEventArgs } from '@syncfusion/ej2-inputs';
import { ClickEventArgs } from '@syncfusion/ej2-angular-navigations';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';
import { LocationService } from 'app/shared/services/location.service';
import { StateModalComponent } from '../state-modal/state-modal.component';
import { DataUtil } from '@syncfusion/ej2-data';
import { FormGroup, AbstractControl, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-state-list',
  templateUrl: './state-list.component.html',
  styleUrls: ['./state-list.component.scss']
})
export class StateListComponent implements OnInit {
queryResult: any = {};
public pageSettings: PageSettingsModel;
@ViewChild('grid') public grid: GridComponent;
public initialPage: PageSettingsModel;
public toolbarOptions: ToolbarItems[];
public editSettings: EditSettingsModel;
public commands: CommandModel[];
stateData: any;
query: any = {
  pageSize: 50
};
countryList: any[];
areaData: Object;
@ViewChild('orderForm')
    public orderForm: FormGroup;

public childGrid: GridModel = {
  queryString: 'stateId',
  toolbar: ['Add', 'Edit', 'Delete', 'Update', 'Cancel'],
  editSettings: { allowEditing: true, allowAdding: true, allowDeleting: true },
  columns: [
      { field: 'id', headerText: 'Area ID', textAlign: 'Right', width: 120 },
      { field: 'areaCode', headerText: 'areaCode', width: 150 },
      { field: 'areaName', headerText: 'areaName', width: 150 },
      { field: 'description', headerText: 'description', width: 150 },
      { field: 'stateId', headerText: 'stateId', width: 150 }
  ],
  load: function(){
    this.parentDetails.parentKeyFieldValue = this.parentDetails.parentRowData['id'];
   },
   actionComplete: function (args: SaveEventArgs): void {
    console.log('hi og');
    console.log(args.requestType);
    // if (args.requestType === 'beginEdit' || args.requestType === 'add') {
    //     this.areaData = Object.assign({}, args.rowData);
    // }
    // if (args.requestType === 'beginEdit' || args.requestType === 'add') {
    //   this.orderForm = this.createFormGroup(args.rowData);
    // }
    if (args.requestType === 'save') {
      this.areaData = Object.assign({}, args.rowData);
      console.log(this.areaData );
      console.log(args );
        // if (this.orderForm.valid) {
        //     args.data = this.areaData;
        // } else {
        //     args.cancel = true;
        // }
    }
  }
};

constructor(private route: ActivatedRoute, private locService: LocationService, private modalService: NgbModal,
  private renderer: Renderer, private router: Router, private toastr: ToastrService) { }

ngOnInit() {
  this.route.data.subscribe(data => {
    this.queryResult = data['stateDetailListData'];
    console.log(this.queryResult);
    // this.orgList = data['orgListData'];
    this.countryList = data['countryListData'];

    this.pageSettings = { pageSizes: true, pageSize: 10 };
    this.toolbarOptions = ['ExcelExport', 'PdfExport', 'Search', 'Print'];
    this.editSettings = { allowAdding: true, allowDeleting: true };
  });
}

private populateList() {
    this.locService.getStateDetailList(this.query)
        .subscribe(result => {
            // console.log(result);
            this.queryResult = result
        });
}

editRecord(args: any): void {
  this.stateData = this.grid.getRowInfo(args.target);
  this.createEditStateModal(this.stateData, 'editMode')
}

viewRecord(args: any): void {
  this.stateData = this.grid.getRowInfo(args.target);
  this.createEditStateModal(this.stateData, 'viewMode')
}

addRecord(): void {
  this.createEditStateModal(null, 'addMode')
}

deleteRecord(args: any): void {
  // console.log('del');
  this.stateData = this.grid.getRowInfo(args.target);
  const recordId = this.stateData.rowData.id;
  (Swal as any).fire({
    title: 'Are you sure',
    text: 'You want to delete this record?',
    type: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Yes'
  }).then((result) => {
    if (result.value) {
      const result$ = this.locService.deleteState(recordId);
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
    if (args.item.id === 'stateListGrid_excelexport') { // 'Grid_excelexport' -> Grid component id + _ + toolbar item name
        this.grid.excelExport();
    }
    if (args.item.id === 'stateListGrid_pdfexport') { // 'Grid_pdfexport' -> Grid component id + _ + toolbar item name
      this.grid.pdfExport();
    }
  }

  createEditStateModal(args: any, screenMode: string) {
  const modalRef = this.modalService.open(StateModalComponent, { windowClass: 'stateModalClass'});
  let stateDataFromList = {};
  if (args && args !== undefined) {
    this.stateData = this.grid.getRowInfo(args.target);
    stateDataFromList = this.stateData.rowData;
  }
  modalRef.componentInstance.stateData = stateDataFromList;
  modalRef.componentInstance.screenMode = screenMode;
  modalRef.componentInstance.countryList = this.countryList;
  modalRef.componentInstance.stateListOutput.subscribe(($e) => {
    this.queryResult = $e;
  });
  }

  createFormGroup(data: IAreaModel): FormGroup {
    return new FormGroup({
      areaCode: new FormControl(data.areaCode, Validators.required),
      areaName: new FormControl(data.areaName, Validators.required),
      description: new FormControl(data.description)
    });
  }

  actionBegin(args: SaveEventArgs): void {
    console.log('hi og');
    console.log(args.requestType);
    if (args.requestType === 'beginEdit' || args.requestType === 'add') {
        this.areaData = Object.assign({}, args.rowData);
    }
    if (args.requestType === 'save') {
        if (this.orderForm.valid) {
            args.data = this.areaData;
        } else {
            args.cancel = true;
        }
    }
  }

  actionComplete(args: DialogEditEventArgs): void {
    console.log('hi ogs');
    console.log(args.requestType);
    if (args.requestType === 'beginEdit' || args.requestType === 'add') {
        // Set initail Focus
        if (args.requestType === 'beginEdit') {
            (args.form.elements.namedItem('areaCode') as HTMLInputElement).focus();
        } else if (args.requestType === 'add') {
            (args.form.elements.namedItem('areaName') as HTMLInputElement).focus();
        }
    }
  }

  public focusIn(target: HTMLElement): void {
    target.parentElement.classList.add('e-input-focus');
  }

  public focusOut(target: HTMLElement): void {
    target.parentElement.classList.remove('e-input-focus');
  }

  get areaCode(): AbstractControl  { return this.orderForm.get('areaCode'); }

  get areaName(): AbstractControl { return this.orderForm.get('areaName'); }

  

  
}

export interface IAreaModel {
  areaCode: string;
  areaName: string;
  description: string;
}
