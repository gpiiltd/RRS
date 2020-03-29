import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { LocationService } from 'app/shared/services/location.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { CountryDetail } from 'app/_models/countrydetail';

@Component({
  selector: 'app-country-modal',
  templateUrl: './country-modal.component.html',
  styleUrls: ['./country-modal.component.scss']
})
export class CountryModalComponent implements OnInit {
  // form data
  countryEditForm: FormGroup;

  // import the data for edit action
  @Input() countryData: CountryDetail;
  @Input() screenMode: string;
  saveCountryData: CountryDetail;

  // modal data
  ModalTitle = '';
  @Output() countryListOutput = new EventEmitter();
  query: any = {
    pageSize: 20
  };

  constructor(public activeModal: NgbActiveModal, private fb: FormBuilder, private locationService: LocationService,
     private toastr: ToastrService, private router: Router) { }

  ngOnInit() {
    this.createEditCountryForm(this.countryData);
    if (this.screenMode === 'viewMode') {
      this.countryEditForm.disable();
      this.ModalTitle = 'View Country';
    } else if (this.screenMode === 'addMode') {
      this.ModalTitle = 'Add Country';
    } else if (this.screenMode === 'editMode') {
      this.ModalTitle = 'Edit Country';
      this.countryEditForm.controls['code1'].disable(); // make Code non editable in edit screen
    }
  }

  createEditCountryForm(item: CountryDetail) {
    if (!item) {
      item = {
        id: '',
        code1: '',
        code2: '',
        name: '',
        description: ''
      }
    }
    this.countryEditForm = this.fb.group({
      id: [item.id || ''],
      code1: [item.code1 || '', Validators.required],
      code2: [item.code2 || ''],
      name: [item.name || '', Validators.required],
      description: [item.description || '']
    });
  }

  private populateCountryList() {
    this.locationService.getCountryDetailList(this.query)
      .subscribe(result => {
        this.countryListOutput.emit(result);
      });
  }

  submit() {
    if (this.countryEditForm.valid) {
      this.saveCountryData = Object.assign({}, this.countryEditForm.value);
      const result$ = (this.saveCountryData.id) ? this.locationService.updateCountry(this.saveCountryData)
       : this.locationService.createCountry(this.saveCountryData);
        result$.subscribe(() => {
          this.toastr.success('Record saved successfully');
          this.populateCountryList();
          this.activeModal.dismiss('Cross click')
        }, error => {
          console.log(error);
          this.toastr.error(error.error.Error[0]);
        });
    }
  }
}
