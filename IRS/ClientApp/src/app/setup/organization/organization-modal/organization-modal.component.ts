import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { Organization } from 'app/_models/organization';
import { OrganizationService } from 'app/shared/services/organization.service';

@Component({
  selector: 'app-organization-modal',
  templateUrl: './organization-modal.component.html',
  styleUrls: ['./organization-modal.component.scss']
})
export class OrganizationModalComponent implements OnInit {
  currentJustify = 'start';
  currentPage = 'About';
  // form data
  orgEditForm: FormGroup;
  isPasswordHidden = true;

  // import the data for edit action
  @Input() orgData: Organization;
  @Input() screenMode: string;
  @Input() countryList: any[];
  @Input() stateList: any[];
  @Input() cityList: any[];
  @Input() areaList: any[];
  @Input() departmentList: any[];
  @Input() orgList: any[];
  saveOrgData: Organization;
  smsServiceProviderList = [{'id': 0, name: 'Twilio'}];

  // modal data
  ModalTitle = '';
  @Output() orgListOutput = new EventEmitter();
  query: any = {
    pageSize: 20
  };

  constructor(public activeModal: NgbActiveModal, private fb: FormBuilder, private orgService: OrganizationService,
     private toastr: ToastrService, private router: Router) { }

  ngOnInit() {
    this.createEditOrganizationForm(this.orgData);
    this.setEmailSettingsValidator();
    this.setSMSSettingsValidator();
    this.setFileSettingsValidator();
    if (this.screenMode === 'view') {
      this.orgEditForm.disable();
      this.ModalTitle = 'View Organization';
    } else if (this.screenMode === 'add') {
      this.ModalTitle = 'Add Organization';
      this.orgEditForm.controls['hazardDefaultDepartmentId'].disable();
    } else if (this.screenMode === 'edit') {
      this.ModalTitle = 'Edit Organization';
      this.orgEditForm.controls['code'].disable(); // make Code non editable in edit screen
    }
    // filter the dept based on org being edited
    const orgName = this.orgEditForm.controls['companyName'].value;
    if (orgName) {
      this.populateDepartmentsByOrgName(orgName);
    } else {
      this.populateDepartmentsByOrgName(null);
    }
    console.log(this.orgList);
  }

  createEditOrganizationForm(item: Organization) {
    if (!item) {
      item = {
        id: '',
        companyName: '',
        registrationNo: '',
        businessCategory: '',
        code: '',
        contactFirstName: '',
        contactMiddleName: '',
        contactLastName: '',
        phone1: '',
        phone2: '',
        officeAddress: '',
        brandLogo: '',
        dateofEst: '',
        comment: '',
        areaId: '',
        cityId: '',
        stateId: '',
        countryId: '',
        enableBranding: '',
        brandTitle: '',
        brandIcon: '',
        useSsl: false,
        hostName: '',
        port: 0,
        maxImageFileSize: 0,
        maxVideoFileSize: 0,
        pageSize: 0,
        email1: '',
        email2: '',
        acceptedVideoFileTypes: '',
        acceptedImageFileTypes: '',
        smsServiceProvider: '',
        entityName: '',
        sendSMS: false,
        activateEmailSenderSettings: false,
        activateFileSettings: false,
        emailSendersEmail: '',
        emailSenderName: '',
        emailSenderPassword: '',
        emailRecipientAddresses: '',
        smsRecipientNumbers: '',
        hazardDefaultDepartmentId: ''
      }
    }
    this.orgEditForm = this.fb.group({
      id: [item.id ],
      code: [item.code , Validators.required],
      companyName: [item.companyName , Validators.required],
      registrationNo: [item.registrationNo],
      businessCategory: [item.businessCategory],
      brandLogo: [item.brandLogo],
      dateofEst: [item.dateofEst],
      email1: [item.email1, [Validators.required, Validators.email]],
      email2: [item.email2, Validators.email],
      phone1: [item.phone1, Validators.required],
      phone2: [item.phone2],
      officeAddress: [item.officeAddress],
      contactFirstName: [item.contactFirstName],
      contactMiddleName: [item.contactMiddleName],
      contactLastName: [item.contactLastName],
      comment: [item.comment],
      areaId: [item.areaId],
      cityId: [item.cityId],
      stateId: [item.stateId],
      countryId: [item.countryId],
      enableBranding: [item.enableBranding],
      brandTitle: [item.brandTitle],
      brandIcon: [item.brandIcon],
      useSsl: [item.useSsl],
      hostName: [item.hostName],
      port: [item.port],
      maxImageFileSize: [item.maxImageFileSize],
      acceptedImageFileTypes: [item.acceptedImageFileTypes],
      pageSize: [item.pageSize],
      maxVideoFileSize: [item.maxVideoFileSize],
      acceptedVideoFileTypes: [item.acceptedVideoFileTypes],
      sendSMS: [item.sendSMS],
      smsServiceProvider: [item.smsServiceProvider],
      entityName: [item.entityName],
      emailSendersEmail: [item.emailSendersEmail],
      emailSenderName:  [item.emailSenderName],
      emailSenderPassword:  [item.emailSenderPassword],
      activateEmailSenderSettings:  [item.activateEmailSenderSettings],
      activateFileSettings:  [item.activateFileSettings],
      emailRecipientAddresses:  [item.emailRecipientAddresses],
      smsRecipientNumbers: [item.smsRecipientNumbers],
      hazardDefaultDepartmentId: [item.hazardDefaultDepartmentId]
    });
  }

  private populateOrganizationList() {
    this.orgService.getOrganizationDetailList(this.query)
      .subscribe(result => {
        this.orgListOutput.emit(result);
      });
  }

  submit() {
    const estDate = this.orgEditForm.controls['dateofEst'].value;
    if (estDate) {
      const dd = new Date(this.orgEditForm.controls['dateofEst'].value);
      dd.setHours(dd.getHours() - dd.getTimezoneOffset() / 60);
      this.orgEditForm.patchValue({
        dateofEst: dd
      });
    }
    if (this.orgEditForm.valid) {
      this.saveOrgData = Object.assign({}, this.orgEditForm.value);
      console.log('submit org');
      console.log(this.saveOrgData);
      const result$ = (this.saveOrgData.id) ? this.orgService.updateOrganization(this.saveOrgData)
       : this.orgService.createOrganization(this.saveOrgData);
        result$.subscribe(() => {
          this.toastr.success('Record saved successfully');
          this.populateOrganizationList();
          this.activeModal.dismiss('Cross click')
        }, error => {
          this.toastr.error(error.error.Error[0]);
        });
    }
  }

  onStateChange() {
    // removes the selected city and repopulates the list upon changing the state
    this.orgEditForm.patchValue({
      cityId: ''
    });
    this.populateCities(this.orgEditForm.controls['stateId'].value);
  }

  onCountryChange() {
    // removes the selected state and city and repopulates the state list upon changing the country
    this.orgEditForm.patchValue({
      stateId: '',
      cityId: ''
    });
    this.populateStates(this.orgEditForm.controls['countryId'].value);
    this.populateCities(null);
  }

  private populateCities(stateId: any) {
    const selectedState = this.stateList.find(m => m.id === stateId);
    this.cityList = selectedState ? selectedState.cities : [];
  };

  private populateStates(countryId: any) {
    const selectedCountry = this.countryList.find(m => m.id === countryId);
    this.stateList = selectedCountry ? selectedCountry.states : [];
  };

   // set file settings fields as compulsory when file settings is activated and vice versa
   setEmailSettingsValidator() {
    const emailSendersEmailControl = this.orgEditForm.get('emailSendersEmail');
    const emailSenderNameControl = this.orgEditForm.get('emailSenderName');
    const emailSenderPasswordControl = this.orgEditForm.get('emailSenderPassword');
    const hostNameControl = this.orgEditForm.get('hostName');
    const portControl = this.orgEditForm.get('port');

    this.orgEditForm.get('activateEmailSenderSettings').valueChanges
      .subscribe(settings => {

        if (settings === true) {
          emailSendersEmailControl.setValidators([Validators.required]);
          emailSenderNameControl.setValidators([Validators.required]);
          emailSenderPasswordControl.setValidators([Validators.required]);
          hostNameControl.setValidators([Validators.required]);
          portControl.setValidators([Validators.required]);
        }
        if (settings === false) {
          emailSendersEmailControl.setValidators(null);
          emailSenderNameControl.setValidators(null);
          emailSenderPasswordControl.setValidators(null);
          hostNameControl.setValidators(null);
          portControl.setValidators(null);
        }

        emailSendersEmailControl.updateValueAndValidity();
        emailSenderNameControl.updateValueAndValidity();
        emailSenderPasswordControl.updateValueAndValidity();
        hostNameControl.updateValueAndValidity();
        portControl.updateValueAndValidity();
      });
  }

  // set email settings fields as compulsory when email settings is activated and vice versa
  setFileSettingsValidator() {
    const maxImageFileSizeControl = this.orgEditForm.get('maxImageFileSize');
    const maxVideoFileSizeControl = this.orgEditForm.get('maxVideoFileSize');
    const acceptedVideoFileTypesControl = this.orgEditForm.get('acceptedVideoFileTypes');
    const acceptedImageFileTypesControl = this.orgEditForm.get('acceptedImageFileTypes');

    this.orgEditForm.get('activateFileSettings').valueChanges
      .subscribe(activateFileSettings => {

        if (activateFileSettings === true) {
          maxImageFileSizeControl.setValidators([Validators.required]);
          maxVideoFileSizeControl.setValidators([Validators.required]);
          acceptedVideoFileTypesControl.setValidators([Validators.required]);
          acceptedImageFileTypesControl.setValidators([Validators.required]);
        }
        if (activateFileSettings === false) {
          maxImageFileSizeControl.setValidators(null);
          maxVideoFileSizeControl.setValidators(null);
          acceptedVideoFileTypesControl.setValidators(null);
          acceptedImageFileTypesControl.setValidators(null);
        }

        maxImageFileSizeControl.updateValueAndValidity();
        maxVideoFileSizeControl.updateValueAndValidity();
        acceptedVideoFileTypesControl.updateValueAndValidity();
        acceptedImageFileTypesControl.updateValueAndValidity();
      });
  }

  // set sms service provider field as compulsory when sms settings is activated and vice versa
  setSMSSettingsValidator() {
    const smsServiceProviderControl = this.orgEditForm.get('smsServiceProvider');

    this.orgEditForm.get('sendSMS').valueChanges
      .subscribe(sendSMS => {

        if (sendSMS === true) {
          smsServiceProviderControl.setValidators([Validators.required]);
        }
        if (sendSMS === false) {
          smsServiceProviderControl.setValidators(null);
        }
        smsServiceProviderControl.updateValueAndValidity();
      });
  }

  hidePasswordToggle(){
    this.isPasswordHidden = !this.isPasswordHidden;
  }

  private populateDepartmentsByOrgName(orgName: any) {
    // console.log('hi bee');
    const selectedOrganization = this.orgList.find(m => m.name === orgName);
    this.departmentList = selectedOrganization ? selectedOrganization.departments : [];
  };
}
