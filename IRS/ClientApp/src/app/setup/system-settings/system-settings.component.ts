import { Component, OnInit } from '@angular/core';
import { SystemSettings } from 'app/_models/systemSettings';
import { ActivatedRoute, Router } from '@angular/router';
import { SystemSettingsService } from 'app/shared/services/system-settings.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-system-settings',
  templateUrl: './system-settings.component.html',
  styleUrls: ['./system-settings.component.scss']
})
export class SystemSettingsComponent implements OnInit {
  currentJustify = 'start';
  settings: SystemSettings;
  saveSettingsData: SystemSettings;
  ModalTitle = '';
  systemSettingsEditForm: FormGroup;
  showEmailSenderPassword = false;
  smsServiceProviderList = [{'id': 0, name: 'Twilio'}]
  isPasswordHidden = true;

  constructor(private route: ActivatedRoute, private settingsService: SystemSettingsService, private fb: FormBuilder,
    private toastr: ToastrService, private router: Router) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      if (data['systemSettingsData']) {
        this.settings = data['systemSettingsData']
      }
      console.log('settings detail');
        console.log(this.settings);
      this.createEditUserForm(this.settings);

      if (!this.settings) {
        this.ModalTitle = 'Add System Settings';
      } else {
        this.ModalTitle = 'Edit System Settings';
      }
    });
  }

  createEditUserForm(item: SystemSettings) {
    if (!item) {
      item = {
        id: '',
        enableBranding: '',
        brandTitle: '',
        brandIcon: '',
        useSsl: true,
        hostName: '',
        port: 0,
        pageSize: 20,
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
        emailSendersEmail: '',
        emailSenderName: '',
        emailSenderPassword: ''
      }
    }
this.systemSettingsEditForm = this.fb.group({
      id: [item.id ],
      enableBranding: [item.enableBranding],
      brandTitle: [item.brandTitle],
      brandIcon: [item.brandIcon],
      useSsl: [item.useSsl],
      hostName: [item.hostName, Validators.required],
      port: [item.port, Validators.required],
      pageSize: [item.pageSize],
      email1: [item.email1],
      email2: [item.email2],
      phone1: [item.phone1],
      phone2: [item.phone2],
      maxImageFileSize: [item.maxImageFileSize, Validators.required],
      maxVideoFileSize: [item.maxVideoFileSize, Validators.required],
      acceptedVideoFileTypes: [item.acceptedVideoFileTypes, Validators.required],
      acceptedImageFileTypes: [item.acceptedImageFileTypes, Validators.required],
      smsServiceProvider: [item.smsServiceProvider, Validators.required],
      entityName: [item.entityName],
      sendSMS: [item.sendSMS],
      emailSendersEmail: [item.emailSendersEmail, Validators.required],
      emailSenderName: [item.emailSenderName, Validators.required],
      emailSenderPassword: [item.emailSenderPassword, Validators.required],
    });
  }

  submit() {
    console.log('create or edit user');
    if (this.systemSettingsEditForm.valid) {
      this.saveSettingsData = Object.assign({}, this.systemSettingsEditForm.value);
      console.log(this.saveSettingsData);
      const result$ = (this.saveSettingsData.id) ? this.settingsService.updateSystemSettings(this.saveSettingsData)
        : this.settingsService.createSystemSettings(this.saveSettingsData);
        result$.subscribe(() => {
          this.toastr.success('Record saved successfully');
          // this.populateuserList();
          this.router.navigate(['/setup/systemsettings/edit']);
        }, error => {
          this.toastr.error(error);
          console.log(error);
        });
      }
    } // end of submit


    showPassword() {
      this.showEmailSenderPassword = !this.showEmailSenderPassword;
    }

    hidePasswordToggle(){
      this.isPasswordHidden = !this.isPasswordHidden;
    }
}
