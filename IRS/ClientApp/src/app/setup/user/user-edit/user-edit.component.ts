
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { UserService } from 'app/shared/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { SaveUser } from 'app/_models/saveUser';
import {NgbDatepickerConfig, NgbCalendar, NgbDate, NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.scss']
})
export class UserEditComponent implements OnInit {
  currentJustify = 'start';
  currentPage = 'About';
  countryList: any[];
  stateList: any[];
  cityList: any[];
  areaList: any[];
  organizationList: any[];
  departmentList: any[];
  editable: true;
  userEditForm: FormGroup;
  ModalTitle = '';
  user: SaveUser;
  saveUserData: SaveUser;
  isGlobalAdminUser = false;
  isOwnerProfileEdit = false;
  isManagerialUser = false;
  jwtHelper = new JwtHelperService();
  loggedIntUserRoles: any[];
  currentLoggedInUserOrgId: any;

  // check that this conforms with backend
  userPreferredContactList = [{'id': 0, name: 'Email'}, {'id': 1, name: 'SMS'}, {'id': 2, name: 'Phone'}]
  userGenderList = [{'id': 0, name: 'Male'}, {'id': 1, name: 'Female'}]
  constructor(private route: ActivatedRoute, private userService: UserService, private fb: FormBuilder,
     private toastr: ToastrService, private router: Router, config: NgbDatepickerConfig) {
  }

  ngOnInit() {
    this.route.data.subscribe(data => {
      if (data['userViewData']) {
        this.user = data['userViewData']
      }
      this.createEditUserForm(this.user);
      this.getToken();
      this.populateKnownFields();
      this.organizationFieldValidator();

      this.countryList = data['countryListData'];
      this.stateList = data['stateListData'];
      this.areaList = data['areaListData'];
      this.cityList = data['cityListData'];
      this.organizationList = data['OrgListData'];
      console.log(this.user);
      this.departmentList = data['DeptListData'];
      this.loggedIntUserRoles = data['UserRolesData'];
      // console.log(this.loggedIntUserRoles);
      if (this.loggedIntUserRoles) {
        this.isManagerialUser = this.loggedIntUserRoles.indexOf('Admin') > -1 || this.loggedIntUserRoles.indexOf('Organization Admin') > -1
         || this.loggedIntUserRoles.indexOf('Manager') > -1;
      } else {
        this.isManagerialUser = false;
      }
      // console.log(this.isManagerialUser);

      if (this.isGlobalAdminUser && this.isOwnerProfileEdit) {
        // make username non changable as its used in logics
        this.userEditForm.controls['userName'].disable();
        this.userEditForm.controls['departmentId'].disable();
        this.userEditForm.controls['organizationId'].disable();
      }

      if (!this.isManagerialUser) {
        // make username non changable as its used in logics
        this.userEditForm.controls['isActive'].disable();
      }

      // filter the dept based on selected org if any
      const orgId = this.userEditForm.controls['organizationId'].value;
      if (orgId) {
        this.populateDepartments(orgId);
      } else {
        this.populateDepartments(null);
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

      if (!this.user) {
        this.ModalTitle = 'Add User';
      } else {
        this.ModalTitle = 'Edit User Info';
      }
    });

    if (!this.isManagerialUser) {
      this.userEditForm.controls['departmentId'].disable();
      this.userEditForm.controls['jobTitle'].disable();
      this.userEditForm.controls['dateOfDeployment'].disable();
      this.userEditForm.controls['dateOfSignOff'].disable();
      this.userEditForm.controls['areaOfDeploymentId'].disable();
      this.userEditForm.controls['cityOfDeploymentId'].disable();
      this.userEditForm.controls['stateOfDeploymentId'].disable();
      this.userEditForm.controls['countryOfDeploymentId'].disable();
      this.userEditForm.controls['staffNo'].disable();
    }

    if (!this.isGlobalAdminUser) {
      this.userEditForm.controls['organizationId'].disable();
    }
  }

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
    if (this.userEditForm.controls['organizationId']) {
      this.populateDepartments(this.userEditForm.controls['organizationId'].value);
    }
  }

  private populateDepartments(organizationId: any) {
    // console.log('hi bee');
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
      this.saveUserData = Object.assign({}, this.userEditForm.getRawValue());
      console.log(this.saveUserData);
      const result$ = (this.saveUserData.id) ? this.userService.updateUser(this.saveUserData)
       : this.userService.createUser(this.saveUserData);
        result$.subscribe(() => {
          this.toastr.success('Record saved successfully');
          // this.populateuserList();
          this.router.navigate(['/setup/users']);
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

    deploymentSignOffDatesValidator(g: FormGroup) {
      //console.log(g.get('dateOfSignOff').value);
      if (g.get('dateOfSignOff').value && g.get('dateOfSignOff').value !== 'undefined') {
        return g.get('dateOfSignOff').value >= g.get('dateOfDeployment').value ? null : {'unsequencial': true};
      }
      return null;
    }

    populateKnownFields() {
      // console.log(this.currentLoggedInUserOrgId);
      if (this.currentLoggedInUserOrgId) {
        this.userEditForm.patchValue({
          organizationId: this.currentLoggedInUserOrgId
        });
      }
    }
  }
