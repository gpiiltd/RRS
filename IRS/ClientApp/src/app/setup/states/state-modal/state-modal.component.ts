import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { LocationService } from 'app/shared/services/location.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { StateDetail } from 'app/_models/statedetail';

@Component({
  selector: 'app-state-modal',
  templateUrl: './state-modal.component.html',
  styleUrls: ['./state-modal.component.scss']
})
export class StateModalComponent implements OnInit {
  // form data
  stateEditForm: FormGroup;

  // import the location data
  @Input() countryList: any[];

  // import the data for edit action
  @Input() stateData: StateDetail;
  @Input() screenMode: string;
  saveStateData: StateDetail;

  // modal data
  ModalTitle = '';
  @Output() stateListOutput = new EventEmitter();
  cityListData: any[];
  query: any = {
    pageSize: 20
  };

  constructor(public activeModal: NgbActiveModal, private fb: FormBuilder, private locationService: LocationService,
     private toastr: ToastrService, private router: Router) { }

  ngOnInit() {
    this.createEditStateForm(this.stateData);
    if(this.screenMode === 'viewMode') {
      this.stateEditForm.disable();
      this.ModalTitle = 'View State';
    } else if (this.screenMode === 'addMode') {
      this.ModalTitle = 'Add State';
    } else if (this.screenMode === 'editMode') {
      this.ModalTitle = 'Edit State';
      this.stateEditForm.controls['stateCode'].disable(); // make Code non editable in edit screen
    }
  }

  createEditStateForm(item: StateDetail) {
    if (!item) {
      item = {
        id: '',
        stateCode: '',
        stateName: '',
        description: '',
        countryId: '',
        countryName: '',
      }
    }
    this.stateEditForm = this.fb.group({
      id: [item.id || ''],
      stateCode: [item.stateCode || '', Validators.required],
      stateName: [item.stateName || '', Validators.required],
      description: [item.description || ''],
      countryId: [item.countryId || '', Validators.required]
    });
  }

  private populateStateList() {
    this.locationService.getStateDetailList(this.query)
      .subscribe(result => {
        console.log(result);
        this.stateListOutput.emit(result);
      });
  }

  submit() {
    if (this.stateEditForm.valid) {
      this.saveStateData = Object.assign({}, this.stateEditForm.value);
      const result$ = (this.saveStateData.id) ? this.locationService.updateState(this.saveStateData)
       : this.locationService.createState(this.saveStateData);
        result$.subscribe(() => {
          this.toastr.success('Record saved successfully');
          this.populateStateList();
          this.activeModal.dismiss('Cross click')
        }, error => {
          console.log(error);
          this.toastr.error(error.error.Error[0]);
        });
    }
  }
}
