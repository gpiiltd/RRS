import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { DepartmentService } from 'app/shared/services/department.service';
import { SaveDepartmentDetail } from 'app/_models/saveDepartmentDetail';

@Component({
  selector: 'app-department-modal',
  templateUrl: './department-modal.component.html',
  styleUrls: ['./department-modal.component.scss']
})
export class DepartmentModalComponent implements OnInit {
  currentJustify = 'start';
  // form data
  deptEditForm: FormGroup;

  // import data from the list view
  @Input() orgList: any[];

  // import the data for edit action
  @Input() deptData: SaveDepartmentDetail;
  @Input() screenMode: string;
  saveDepartmentData: SaveDepartmentDetail;
  smsServiceProviderList = [{'id': 0, name: 'Twilio'}];
  // modal data
  ModalTitle = '';
  @Output() deptListOutput = new EventEmitter();
  query: any = {
    pageSize: 20
  };
  isPasswordHidden = true;
  constructor(public activeModal: NgbActiveModal, private fb: FormBuilder, private deptService: DepartmentService,
     private toastr: ToastrService, private router: Router) { }

  ngOnInit() {
    this.createEditDepartmentForm(this.deptData);
    console.log(this.deptData);
    this.setEmailSettingsValidator();
    this.setSMSSettingsValidator();
    this.setFileSettingsValidator();
    if (this.screenMode === 'view') {
      this.deptEditForm.disable();
      this.ModalTitle = 'View Department';
    } else if (this.screenMode === 'add') {
      this.ModalTitle = 'Add Department';
    } else if (this.screenMode === 'edit') {
      this.ModalTitle = 'Edit Department';
      this.deptEditForm.controls['code'].disable(); // make Code non editable in edit screen
    }
  }

  createEditDepartmentForm(item: SaveDepartmentDetail) {
    if (!item) {
      item = {
        id: '',
        code: '',
        name: '',
        description: '',
        hostName: '',
        port: 0,
        email1: '',
        email2: '',
        phone1: '',
        phone2: '',
        maxImageFileSize: 0,
        maxVideoFileSize: 0,
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
        smsRecipientNumbers: ''
      }
    }
    this.deptEditForm = this.fb.group({
      id: [item.id],
      code: [item.code, Validators.required],
      name: [item.name, Validators.required],
      description: [item.description],
      hostName: [item.hostName],
      port: [item.port],
      email1: [item.email1, Validators.email],
      email2: [item.email2, Validators.email],
      phone1: [item.phone1],
      phone2: [item.phone2],
      maxImageFileSize: [item.maxImageFileSize],
      maxVideoFileSize: [item.maxVideoFileSize],
      acceptedVideoFileTypes: [item.acceptedVideoFileTypes],
      acceptedImageFileTypes: [item.acceptedImageFileTypes],
      smsServiceProvider: [item.smsServiceProvider],
      entityName: [item.entityName],
      sendSMS: [item.sendSMS || false],
      activateEmailSenderSettings: [item.activateEmailSenderSettings || false],
      activateFileSettings: [item.activateFileSettings || false],
      emailSendersEmail: [item.emailSendersEmail],
      emailSenderName: [item.emailSenderName],
      emailSenderPassword: [item.emailSenderPassword],
      emailRecipientAddresses: [item.emailRecipientAddresses],
      smsRecipientNumbers: [item.smsRecipientNumbers]
    });
  }

  private populateDepartmentList() {
    this.deptService.getDepartmentDetailList(this.query)
      .subscribe(result => {
        this.deptListOutput.emit(result);
      });
  }

  // set file settings fields as compulsory when file settings is activated and vice versa
  setEmailSettingsValidator() {
    const emailSendersEmailControl = this.deptEditForm.get('emailSendersEmail');
    const emailSenderNameControl = this.deptEditForm.get('emailSenderName');
    const emailSenderPasswordControl = this.deptEditForm.get('emailSenderPassword');
    const hostNameControl = this.deptEditForm.get('hostName');
    const portControl = this.deptEditForm.get('port');

    this.deptEditForm.get('activateEmailSenderSettings').valueChanges
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
    const maxImageFileSizeControl = this.deptEditForm.get('maxImageFileSize');
    const maxVideoFileSizeControl = this.deptEditForm.get('maxVideoFileSize');
    const acceptedVideoFileTypesStringControl = this.deptEditForm.get('acceptedVideoFileTypes');
    const acceptedImageFileTypesStringControl = this.deptEditForm.get('acceptedImageFileTypes');

    this.deptEditForm.get('activateFileSettings').valueChanges
      .subscribe(activateFileSettings => {

        if (activateFileSettings === true) {
          maxImageFileSizeControl.setValidators([Validators.required]);
          maxVideoFileSizeControl.setValidators([Validators.required]);
          acceptedVideoFileTypesStringControl.setValidators([Validators.required]);
          acceptedImageFileTypesStringControl.setValidators([Validators.required]);
        }
        if (activateFileSettings === false) {
          maxImageFileSizeControl.setValidators(null);
          maxVideoFileSizeControl.setValidators(null);
          acceptedVideoFileTypesStringControl.setValidators(null);
          acceptedImageFileTypesStringControl.setValidators(null);
        }

        maxImageFileSizeControl.updateValueAndValidity();
        maxVideoFileSizeControl.updateValueAndValidity();
        acceptedVideoFileTypesStringControl.updateValueAndValidity();
        acceptedImageFileTypesStringControl.updateValueAndValidity();
      });
  }

  // set sms service provider field as compulsory when sms settings is activated and vice versa
  setSMSSettingsValidator() {
    const smsServiceProviderControl = this.deptEditForm.get('smsServiceProvider');

    this.deptEditForm.get('sendSMS').valueChanges
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

  submit() {
    console.log('try saves');
    if (this.deptEditForm.valid) {
      console.log('try save');
      this.saveDepartmentData = Object.assign({}, this.deptEditForm.value);
      console.log(this.saveDepartmentData);
      const result$ = (this.saveDepartmentData.id) ? this.deptService.updateDepartment(this.saveDepartmentData)
         : this.deptService.createDepartment(this.saveDepartmentData);
        result$.subscribe(() => {
          this.toastr.success('Record saved successfully');
          this.populateDepartmentList();
          this.activeModal.dismiss('Cross click')
        }, error => {
          // this.toastr.error('Error saving record');
          this.toastr.error(error.error.Error[0]);
        });
    }
  }

  hidePasswordToggle(){
    this.isPasswordHidden = !this.isPasswordHidden;
  }
}
