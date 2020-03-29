import { Component, OnInit, Input, ViewChild, Output, EventEmitter  } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { LocationService } from 'app/shared/services/location.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { CityDetail } from 'app/_models/citydetail';

@Component({
  selector: 'app-city-modal',
  templateUrl: './city-modal.component.html',
  styleUrls: ['./city-modal.component.scss']
})
export class CityModalComponent implements OnInit {
  // form data
  cityEditForm: FormGroup;

  // import the location data
  @Input() countryList: any[];
  @Input() stateList: any[];

  // import the data for edit action
  @Input() cityData: CityDetail;
  @Input() screenMode: string;
  saveCityData: CityDetail;

  // modal data
  ModalTitle = '';
  @Output() cityListOutput = new EventEmitter();
  cityListData: any[];
  query: any = {
    pageSize: 20
  };

  constructor(public activeModal: NgbActiveModal, private fb: FormBuilder, private locationService: LocationService,
     private toastr: ToastrService, private router: Router) { }

  ngOnInit() {
    console.log('init');
    console.log(this.cityData);
    this.createCityAreaForm(this.cityData);
    if (this.screenMode === 'viewMode') {
      this.cityEditForm.disable();
      this.ModalTitle = 'View City';
    } else if (this.screenMode === 'addMode') {
      this.ModalTitle = 'Add City';
    } else if (this.screenMode === 'editMode') {
      this.ModalTitle = 'Edit City';
      this.populateStates(this.cityData.countryId); // filter states based on selected country
      this.cityEditForm.controls['cityCode'].disable(); // make Code non editable in edit screen
    }
  }

  createCityAreaForm(item: CityDetail) {
    if (!item) {
      item = {
        id: '',
        cityCode: '',
        cityName: '',
        description: '',
        stateId: '',
        countryId: '',
        stateName: '',
        countryName: '',
      }
    }
    this.cityEditForm = this.fb.group({
      id: [item.id || ''],
      cityCode: [item.cityCode || '', Validators.required],
      cityName: [item.cityName || '', Validators.required],
      description: [item.description || ''],
      stateId: [item.stateId || '', Validators.required],
      countryId: [item.countryId || '']
    });
  }

  private populateCityList() {
    this.locationService.getCityDetailList(this.query)
      .subscribe(result => {
        // console.log('result..:');
        // console.log(result);
        //this.cityListData = result
        this.cityListOutput.emit(result);
      });
  }

  submit() {
    if (this.cityEditForm.valid) {
      console.log('saving city..');
      this.saveCityData = Object.assign({}, this.cityEditForm.value);
      console.log('saving area now..');
      // console.log(this.saveAreaData);
      const result$ = (this.saveCityData.id) ? this.locationService.updateCity(this.saveCityData)
       : this.locationService.createCity(this.saveCityData);
        result$.subscribe(() => {
          this.toastr.success('Record saved successfully');
          this.populateCityList();
          this.activeModal.dismiss('Cross click')
        }, error => {
          this.toastr.error(error.error.Error[0]);
        });
    }
  }
  private populateStates(countryId: any) {
    const selectedCountry = this.countryList.find(m => m.id === countryId);
    this.stateList = selectedCountry ? selectedCountry.states : [];
  };

  onCountryChange() {
    // removes the selected state and city and repopulates the state list upon changing the country
    this.cityEditForm.patchValue({
      stateId: ''
    });
    this.populateStates(this.cityEditForm.controls['countryId'].value);
  }
}
