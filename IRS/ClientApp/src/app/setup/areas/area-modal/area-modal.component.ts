import { Component, OnInit, Input, ViewChild, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { AreaDetail } from 'app/_models/areadetail';
import { LocationService } from 'app/shared/services/location.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-area-modal',
  templateUrl: './area-modal.component.html',
  styleUrls: ['./area-modal.component.scss']
})
export class AreaModalComponent implements OnInit {
  // form data
  areaEditForm: FormGroup;

  // import the location data
  @Input() countryList: any[];
  @Input() stateList: any[];
  @Input() cityList: any[];

  // import the data for edit action
  @Input() areaData: AreaDetail;
  @Input() screenMode: string;
  saveAreaData: AreaDetail;

  // modal data
  ModalTitle = '';
  @Output() areaListOutput = new EventEmitter();
  areaListData: any[];
  query: any = {
    pageSize: 20
  };

  constructor(public activeModal: NgbActiveModal, private fb: FormBuilder, private locationService: LocationService,
     private toastr: ToastrService, private router: Router) { }

  ngOnInit() {
    console.log(this.areaData);
    this.createEditAreaForm(this.areaData);
    if (this.screenMode === 'viewMode') {
      this.areaEditForm.disable();
      this.ModalTitle = 'View Area';
    } else if (this.screenMode === 'addMode') {
      this.ModalTitle = 'Add Area';
    } else if (this.screenMode === 'editMode') {
      this.ModalTitle = 'Edit Area';
      this.populateStates(this.areaData.countryId); // filter states based on selected country
      this.populateCities(this.areaData.stateId); // filter cities based on selected State
      this.areaEditForm.controls['areaCode'].disable(); // make Code non editable in edit screen
    }
  }

  createEditAreaForm(item: AreaDetail) {
    if (!item) {
      item = {
        id: '',
        areaCode: '',
        areaName: '',
        description: '',
        countryId: '',
        stateId: '',
        cityId: '',
        cityName: '',
        stateName: '',
        countryName: '',
      }
    }
    this.areaEditForm = this.fb.group({
      id: [item.id || ''],
      areaCode: [item.areaCode || '', Validators.required],
      areaName: [item.areaName || '', Validators.required],
      description: [item.description || ''],
      countryId: [item.countryId || ''],
      stateId: [item.stateId || '', Validators.required],
      cityId: [item.cityId || '']
    });
  }

  private populateAreas() {
    this.locationService.getAreaDetailList(this.query)
      .subscribe(result => {
        // console.log('result..:');
        // console.log(result);
        this.areaListData = result
        this.areaListOutput.emit(this.areaListData);
      });
  }

  submit() {
    if (this.areaEditForm.valid) {
      console.log('saving area..');
      this.saveAreaData = Object.assign({}, this.areaEditForm.value);
      console.log('saving area now..');
      console.log(this.saveAreaData);
      const result$ = (this.saveAreaData.id) ? this.locationService.updateArea(this.saveAreaData)
       : this.locationService.createArea(this.saveAreaData);
        result$.subscribe(() => {
          this.toastr.success('Record saved successfully');
          this.populateAreas();
          this.activeModal.dismiss('Cross click')
        }, error => {
          this.toastr.error(error.error.Error[0]);
        });
    }
  }

  private populateCities(stateId: any) {
    const selectedState = this.stateList.find(m => m.id === stateId);
    this.cityList = selectedState ? selectedState.cities : [];
  };

  private populateStates(countryId: any) {
    const selectedCountry = this.countryList.find(m => m.id === countryId);
    this.stateList = selectedCountry ? selectedCountry.states : [];
  };

  onStateChange() {
    // removes the selected city and repopulates the list upon changing the state
    this.areaEditForm.patchValue({
      cityId: ''
    });
    this.populateCities(this.areaEditForm.controls['stateId'].value);
  }

  onCountryChange() {
    // removes the selected state and city and repopulates the state list upon changing the country
    this.areaEditForm.patchValue({
      stateId: '',
      cityId: ''
    });
    this.populateStates(this.areaEditForm.controls['countryId'].value);
    this.populateCities(null);
  }

}
