import { KeyValuePair } from 'app/_models/keyValuePair';
import { ToastrService } from 'ngx-toastr';
import { Component, OnInit, ElementRef, ViewChild, NgZone } from '@angular/core';
import { UserService } from 'app/shared/services/user.service';
import { AuthService } from 'app/shared/services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { SaveUser } from 'app/_models/saveUser';
import {NgbDatepickerConfig, NgbCalendar, NgbDate, NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'environments/environment';
import { PhotoService } from 'app/shared/services/photo.service';

@Component({
  selector: 'app-user-profile-edit',
  templateUrl: './user-profile-edit.component.html',
  styleUrls: ['./user-profile-edit.component.scss']
})
export class UserProfileEditComponent implements OnInit {
  @ViewChild('fileInput') fileInput: ElementRef;
  currentPage = 'About';
  countryList: any[];
  stateList: any[];
  areaList: any[];
  DeptList: any[];
  organizationList: any[];
  currentJustify = 'start';
  editable = true;
  cityList: any[];
  departmentList: any[];
  userEditForm: FormGroup;
  ModalTitle = '';
  user: SaveUser;
  saveUserData: SaveUser;
  isGlobalAdminUser = false;
  isOwnerProfileEdit = false;
  isManagerialUser = false;
  jwtHelper = new JwtHelperService();
  loggedIntUserRoles: any[];
  profilePhotoData: any;
  profilePhotoUrl = 'assets/img/portrait/avatars/avatar-08.png';
  imgUrl = environment.userImgUrl;
  currentLoggedInUserOrgId: any;

  // check that this conforms with backend
  userPreferredContactList = [{'id': 0, name: 'Email'}, {'id': 1, name: 'SMS'}, {'id': 2, name: 'Phone'}]
  // check that this conforms with backend
  userGenderList = [{'id': 0, name: 'Male'}, {'id': 1, name: 'Female'}]

  constructor(private route: ActivatedRoute, private userService: UserService, private fb: FormBuilder, private photoService: PhotoService,
    private toastr: ToastrService, private router: Router, private config: NgbDatepickerConfig) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      if (data['userViewData']) {
        this.user = data['userViewData']
      }
      this.createEditUserForm(this.user);
      this.getToken();
      this.organizationFieldValidator();

      this.countryList = data['countryListData'];
      this.stateList = data['stateListData'];
      this.areaList = data['areaListData'];
      this.cityList = data['cityListData'];
      this.organizationList = data['OrgListData'];
      this.departmentList = data['DeptListData'];
      this.loggedIntUserRoles = data['UserRolesData'];
      this.profilePhotoData = data['UserProfilePhotoData'];
      // console.log(this.profilePhotoData);
      // get url for u
      if (this.profilePhotoData) {
        this.profilePhotoUrl = this.createImgPath(this.profilePhotoData.fileName);
      }
      // console.log(this.userRoles);
      if (this.loggedIntUserRoles) {
        this.isManagerialUser = this.loggedIntUserRoles.indexOf('Admin') > -1 || this.loggedIntUserRoles.indexOf('Organization Admin') > -1
         || this.loggedIntUserRoles.indexOf('Manager') > -1;
      } else {
        this.isManagerialUser = false;
      }

      if (this.isGlobalAdminUser && this.isOwnerProfileEdit) {
        // make username non changable as its used in logics
        this.userEditForm.controls['userName'].disable();
        this.userEditForm.controls['departmentId'].disable();
        this.userEditForm.controls['organizationId'].disable();
        // filter the dept based on selected org if any
        const orgId = this.userEditForm.controls['organizationId'].value;
        if (orgId) {
          this.populateDepartments(orgId);
        }
      }

      if (!this.isManagerialUser) {
        // make username non changable as its used in logics
        this.userEditForm.controls['isActive'].disable();
      }

      // process the timezone differences that causes day to go minus one
      if (this.user && this.user.dateOfDeployment) {
        const dd = new Date(this.userEditForm.controls['dateOfDeployment'].value);
        dd.setHours(dd.getHours() - dd.getTimezoneOffset() / 60);
        this.userEditForm.patchValue({
          dateOfDeployment: dd
        });
      }
      if (this.user && this.user.dateOfSignOff) {
      const ds = new Date(this.userEditForm.controls['dateOfSignOff'].value);
        ds.setHours(ds.getHours() - ds.getTimezoneOffset() / 60);
        this.userEditForm.patchValue({
          dateOfSignOff: ds
        });
      }
      if (this.user && this.user.dateOfBirth) {
        const db = new Date(this.userEditForm.controls['dateOfBirth'].value);
          db.setHours(db.getHours() - db.getTimezoneOffset() / 60);
          this.userEditForm.patchValue({
            dateOfBirth: db
          });
        }

    //  console.log('org...');
    //  console.log(this.user);

      if (!this.user) {
        this.ModalTitle = 'Add User';
      } else {
        this.ModalTitle = 'Edit User Profile';
      }
      this.populateKnownFields();
      if (!this.isGlobalAdminUser) {
        this.userEditForm.controls['organizationId'].disable();
      }
});

  if (!this.isManagerialUser) {
    this.userEditForm.controls['departmentId'].disable();
    this.userEditForm.controls['organizationId'].disable();
    this.userEditForm.controls['jobTitle'].disable();
    this.userEditForm.controls['dateOfDeployment'].disable();
    this.userEditForm.controls['dateOfSignOff'].disable();
    this.userEditForm.controls['areaOfDeploymentId'].disable();
    this.userEditForm.controls['cityOfDeploymentId'].disable();
    this.userEditForm.controls['stateOfDeploymentId'].disable();
    this.userEditForm.controls['countryOfDeploymentId'].disable();
    this.userEditForm.controls['staffNo'].disable();
    this.userEditForm.controls['firstName'].disable();
    this.userEditForm.controls['middleName'].disable();
    this.userEditForm.controls['lastName'].disable();
    }
  }

  // showPage(page: string) {
  //   this.currentPage = page;
  // };



  createEditUserForm(item: SaveUser) {
  if (!item) {
    item = {
      id: '',
      firstName: '',
      middleName: '',
      lastName: '',
      email1: '',
      email2: '',
      phone1: '',
      phone2: '',
      gender: 0,
      dateOfBirth: '',
      introduction: '',
      knownAs: '',
      lastActive: '',
      photoUrl: '',
      jobTitle: '',
      staffNo: '',
      departmentId: '',
      organizationId: '',
      preferredContactMethod: 0,
      areaOfDeploymentId: '',
      cityOfDeploymentId: '',
      stateOfDeploymentId: '',
      countryOfDeploymentId: '',
      areaOfOriginId: '',
      cityOfOriginId: '',
      stateOfOriginId: '',
      countryOfOriginId: '',
      dateOfDeployment: '',
      dateOfSignOff: '',
      userName: '',
      password: '',
      isActive: false,
      mobileAppLoginPattern: ''
    }
  }
  this.userEditForm = this.fb.group({
    id: [item.id ],
    firstName: [item.firstName, Validators.required],
    middleName: [item.middleName],
    lastName: [item.lastName, Validators.required],
    email1: [item.email1, [Validators.required, Validators.email]],
    email2: [item.email2, Validators.email],
    phone1: [item.phone1, Validators.required],
    phone2: [item.phone2],
    gender: [item.gender, Validators.required],
    dateOfBirth: [item.dateOfBirth],
    introduction: [item.introduction],
    knownAs: [item.knownAs],
    lastActive: [item.lastActive],
    photoUrl: [item.photoUrl],
    jobTitle: [item.jobTitle, Validators.required],
    staffNo: [item.staffNo],
    departmentId: [item.departmentId, Validators.required],
    organizationId: [item.organizationId, Validators.required],
    preferredContactMethod: [item.preferredContactMethod],
    areaOfDeploymentId: [item.areaOfDeploymentId],
    cityOfDeploymentId: [item.cityOfDeploymentId],
    stateOfDeploymentId: [item.stateOfDeploymentId],
    countryOfDeploymentId: [item.countryOfDeploymentId],
    dateOfDeployment: [item.dateOfDeployment, Validators.required],
    dateOfSignOff: [item.dateOfSignOff],
    userName: [item.userName, Validators.required],
    password: [''],
    isActive: [item.isActive],
    mobileAppLoginPattern: [item.mobileAppLoginPattern]
  }, {validator: this.deploymentSignOffDatesValidator});
  }

  getToken() {
  const userToken = localStorage.getItem('token');
  const decodedToken = this.jwtHelper.decodeToken(userToken);
  this.currentLoggedInUserOrgId = decodedToken.upn;
  if (decodedToken.unique_name === 'Admin') {
    this.isGlobalAdminUser = true;
  }
  // console.log(decodedToken.nameid);
  // console.log(this.user.id);
  if (this.user && this.user.id === decodedToken.nameid) {
    this.isOwnerProfileEdit = true;
  }
  }

  onOrganizationChange() {
  // removes the selected state and city and repopulates the state list upon changing the country
  this.userEditForm.patchValue({
    departmentId: '',
  });
  this.populateDepartments(this.userEditForm.controls['organizationId'].value);
  }

  private populateDepartments(organizationId: any) {
  const selectedOrganization = this.organizationList.find(m => m.id === organizationId);
  this.departmentList = selectedOrganization ? selectedOrganization.departments : [];
  };

  private populateCities(stateId: any) {
  const selectedState = this.stateList.find(m => m.id === stateId);
  this.cityList = selectedState ? selectedState.cities : [];
  };

  private populateStates(countryId: any) {
  const selectedCountry = this.countryList.find(m => m.id === countryId);
  this.stateList = selectedCountry ? selectedCountry.states : [];
  };

  private populateAreas(stateId: any) {
  const selectedState = this.stateList.find(m => m.id === stateId);
  this.areaList = selectedState ? selectedState.areas : [];
  };

  onStateChange() {
  // removes the selected city and repopulates the list upon changing the state
  this.userEditForm.patchValue({
    cityOfDeploymentId: '',
    areaOfDeploymentId: ''
  });
  this.populateCities(this.userEditForm.controls['stateOfDeploymentId'].value);
  this.populateAreas(this.userEditForm.controls['stateOfDeploymentId'].value);
  }

  onCityChange() {
  this.userEditForm.patchValue({
    areaOfDeploymentId: ''
  });
  }

  onCountryChange() {
  // removes the selected state and city and repopulates the state list upon changing the country
  this.userEditForm.patchValue({
    stateOfDeploymentId: '',
    cityOfDeploymentId: '',
    areaOfDeploymentId: ''
  });
  this.populateStates(this.userEditForm.controls['countryOfDeploymentId'].value);
  this.populateCities(null);
  }

  submit() {
    const deploymentDate = this.userEditForm.controls['dateOfDeployment'].value;
    if (deploymentDate) {
      const dd = new Date(this.userEditForm.controls['dateOfDeployment'].value);
      dd.setHours(dd.getHours() - dd.getTimezoneOffset() / 60);
      this.userEditForm.patchValue({
        dateOfDeployment: dd
      });
    }
    const signOffDate = this.userEditForm.controls['dateOfSignOff'].value;
    if (signOffDate) {
    const ds = new Date(this.userEditForm.controls['dateOfSignOff'].value);
      ds.setHours(ds.getHours() - ds.getTimezoneOffset() / 60);
      this.userEditForm.patchValue({
        dateOfSignOff: ds
      });
    }
    const birthDate = this.userEditForm.controls['dateOfBirth'].value;
    if (birthDate) {
      const db = new Date(this.userEditForm.controls['dateOfBirth'].value);
        db.setHours(db.getHours() - db.getTimezoneOffset() / 60);
        this.userEditForm.patchValue({
          dateOfBirth: db
        });
  }
  if (this.userEditForm.valid) {
    // use this.userEditForm.getRawValue() to retrieve all form values including disabled fields. otherwise use this.userEditForm.value
    this.saveUserData = Object.assign({}, this.userEditForm.getRawValue());
    console.log(this.saveUserData);
    const result$ = (this.saveUserData.id) ? this.userService.updateUser(this.saveUserData)
    : this.userService.createUser(this.saveUserData);
      result$.subscribe(() => {
        this.toastr.success('Record saved successfully');
        // this.populateuserList();
        this.router.navigate(['/dashboard']);
      }, error => {
        // console.log(error);
        this.toastr.error(error.error.Error[0]);
      });
    }
  } // end of submit

  // set Organization field as required when current user is Global Admin. Otherwise system auto detects the user's organization in backend
  organizationFieldValidator() {
  const organizationValidatorControl = this.userEditForm.get('organizationId');

      if (this.isGlobalAdminUser) {
        organizationValidatorControl.setValidators([Validators.required]);
      }
      if (!this.isGlobalAdminUser) {
        organizationValidatorControl.setValidators(null);
      }
      organizationValidatorControl.updateValueAndValidity();
  }

  uploadPhoto() {
    console.log('upload starts...');
    //angular doees not monkey patch the xhr.upload.onprogress in progressService hence no angular change detection for XMLHttpRequest requests. we fix with zones like below
    // this.progressService.startTracking()
    //   .subscribe(progress => {
    //     console.log(progress);
    //     this.zone.run(() => {
    //       this.progress = progress;
    //     });
    //   },
    //   () => {  } );

    var nativeElement: HTMLInputElement = this.fileInput.nativeElement;

    if (nativeElement.files && nativeElement.files.length > 0) {
        const file = nativeElement.files![0];
        this.photoService.uploadUserProfilePhoto(this.userEditForm.controls['id'].value, file)
      .subscribe(photo => {
        console.log(photo);
        this.profilePhotoData = photo;
        this.profilePhotoUrl = this.createImgPath(this.profilePhotoData.fileName),
        this.toastr.success('Profile photo uploaded successfully');
      },
      error => {
        // console.log(error.error);
        // no error.error.Error[0] here
        this.toastr.error(error.error);
      });
    nativeElement.value = '';
    }
  }

  createImgPath(fileName: string) {
    // return profilePhotoUrl when user has no uploaded photo in db
    if (fileName !== '') {
      console.log(this.imgUrl + fileName);
      return this.imgUrl + fileName;
    }
    return this.profilePhotoUrl;
  }

  deploymentSignOffDatesValidator(g: FormGroup) {
    return g.get('dateOfSignOff').value >= g.get('dateOfDeployment').value ? null : {'unsequencial': true};
  }

  populateKnownFields() {
    console.log(this.currentLoggedInUserOrgId);
    if (this.currentLoggedInUserOrgId) {
      this.userEditForm.patchValue({
        organizationId: this.currentLoggedInUserOrgId
      });
    }
  }
}
