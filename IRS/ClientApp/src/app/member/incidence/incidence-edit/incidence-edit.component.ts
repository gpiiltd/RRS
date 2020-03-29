
import { Component, OnInit, ElementRef, ViewChild, NgZone } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm, AbstractControl, ValidatorFn } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { NgbDatepickerConfig, NgbCalendar, NgbDate, NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';
import { SaveIncidence } from 'app/_models/saveIncidence';
import { IncidenceService } from 'app/shared/services/incidence.service';
import { PhotoService } from 'app/shared/services/photo.service';
import { ProgressService } from 'app/shared/services/progress.service';
import { Photo } from 'app/_models/media/photo';
import { Media } from 'app/_models/media/media';
import { environment } from 'environments/environment';
import { NgbCarousel } from '@ng-bootstrap/ng-bootstrap';
import { VgAPI } from 'videogular2/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-incidence-edit',
  templateUrl: './incidence-edit.component.html',
  styleUrls: ['./incidence-edit.component.scss']
})

export class IncidenceEditComponent implements OnInit {
  @ViewChild('fileInput') fileInput: ElementRef;
  public files: Set<File> = new Set()
    currentJustify = 'start';
    currentPage = 'About';
    countryList: any[];
    stateList: any[];
    cityList: any[];
    areaList: any[];
    organizationList: any[];
    departmentList: any[];
    userList: any[];
    allUserList: any[];
    incidenceTypeList: any[];
    incidenceStatusList: any[];
    sliderWidth: Number = 940;
    sliderImageWidth: Number = 250;
    sliderImageHeight: Number = 200;
    isOrgAdminUser = false;

    incidenceMedia: any[];
    incidenceResolutionPhotos: any[] = [];
    incidenceResolutionVideos: any[] = [];
    incidenceReportedPhotos: any[] = [];
    incidenceReportedVideos: any[] = [];
    incidenceReporterFeedbackRating = 0;
    incidenceManagerFeedbackRating = 0;
    showUploadFileButtons = true;
    isEditMode = false;

    isManagerialUser = false;
    loggedInUserRoles: any[];
    editable: true;
    incidentForm: FormGroup;
    ModalTitle = '';
    incidence: SaveIncidence;
    saveIncidenceData: SaveIncidence;
    // check that this conforms with backend
    userPreferredContactList = [{'id': 0, name: 'Email'}, {'id': 1, name: 'SMS'}, {'id': 2, name: 'Phone'}];
    userGenderList = [{'id': 0, name: 'Male'}, {'id': 1, name: 'Female'}];
    memberIncidenceStatusList = ['Resolved', 'Re-Opened', 'Open'];
    photos: Photo[];
    progress: any;
    imgUrl = environment.imgUrl;
    count = 0;
    photoVideoToggle = true;
    currentIndex = 0;
    api: VgAPI;
    iResolutionVideos = Array<Media>();
    iMediaData = Array<Photo>();
    // currentItem: Media = {
    //   title: '',
    //   src: '',
    //   type: '',
    //   description: ''
    // };
    mediaType = {
      video: true,
      photo: false
    };
    mediaChannel = {
      mobileReported: 0,
      webResolved: 1
    }

    showManageResolutionVideos = false;
    showManageResolutionPhotos = false;
    showManageReportedVideos = false;
    showManageReportedPhotos = false;
    openIncidenceStatusId = 'a64769a0-ce38-4414-afdb-c6bfac9056a1';
    reOpenedIncidenceStatusId = 'f59fe8e6-646a-4b04-7fbc-08d702eeea0d';
    closedIncidenceStatusId = '0839bc87-7d23-431d-86c4-3ca1e4f190ef';
    underReviewIncidenceStatusId = '53986162-1dae-45f7-8771-08d702ea4b5c';
    currentLoggedInUserId: any;
    currentLoggedInUserOrgId: any;
    jwtHelper = new JwtHelperService();

    @ViewChild('carousel') carousel: NgbCarousel;

    constructor(
      private route: ActivatedRoute,
      private iService: IncidenceService,
      private fb: FormBuilder,
      private toastr: ToastrService,
      private router: Router,
      config: NgbDatepickerConfig,
      private zone: NgZone,
      private progressService: ProgressService,
      private photoService: PhotoService) {
    }

    ngOnInit() {
      this.route.data.subscribe(data => {
        // console.log('inc...');
        // console.log(data['incidenceData']);
        if (data['incidenceData']) {
          this.incidence = data['incidenceData']
        //   console.log('inc...');
        //  console.log(this.incidence);
        }
        this.createEditIncidenceForm(this.incidence);
        console.log(this.incidentForm.controls['reporterFeedbackRating'].value);
        this.incidenceReporterFeedbackRating = this.incidentForm.controls['reporterFeedbackRating'].value;
        this.incidenceManagerFeedbackRating = this.incidentForm.controls['managerFeedbackRating'].value;

        this.countryList = data['countryListData'];
        this.stateList = data['stateListData'];
        this.areaList = data['areaListData'];
        this.cityList = data['cityListData'];
        this.organizationList = data['OrgListData'];
        this.departmentList = data['DeptListData'];
        this.allUserList = data['userListData'];
        console.log('users log');
        console.log(this.allUserList);
        this.userList = this.allUserList;
        this.incidenceTypeList = data['incidenceTypeListData'];
        this.incidenceStatusList = data['incidenceStatusListData'];
        this.incidenceMedia = data['incidenceResolutionPhotosData'];
        // console.log(this.incidenceMedia);
        this.populateMedia();
        this.loggedInUserRoles = data['UserRolesData'];
        console.log(this.loggedInUserRoles);
        if (this.loggedInUserRoles) {
          this.isManagerialUser = this.loggedInUserRoles.indexOf('Admin') > -1
          || this.loggedInUserRoles.indexOf('Organization Admin') > -1
          || this.loggedInUserRoles.indexOf('Manager') > -1;

          this.isOrgAdminUser = this.loggedInUserRoles.indexOf('Organization Admin') > -1;
        } else {
          this.isManagerialUser = false;
        }
        // console.log(this.departmentList);
        // console.log(this.organizationList);
        // console.log(this.countryList);
        // console.log(this.incidence.incidenceStatusId);
        // console.log(this.incidentForm.controls['incidenceStatusId'].value);

        // process the timezone differences that causes day to go minus one
        if (this.incidence && this.incidence.resolutionDate) {
          const dd = new Date(this.incidentForm.controls['resolutionDate'].value);
          dd.setHours(dd.getHours() - dd.getTimezoneOffset() / 60);
          this.incidentForm.patchValue({
            resolutionDate: dd
          });
        }

        if (this.incidence && this.incidence.countryId) {
          this.populateStates(this.incidentForm.controls['countryId'].value);
        }
        if (this.incidence && this.incidence.stateId) {
          this.populateCities(this.incidentForm.controls['stateId'].value);
          this.populateAreas(this.incidentForm.controls['stateId'].value);
        }
        if (this.incidence && this.incidence.assignedOrganizationId) {
          console.log(this.incidentForm.controls['assignedOrganizationId'].value);
          this.populateDepartments(this.incidentForm.controls['assignedOrganizationId'].value);
        }
        if (this.incidence && this.incidence.assignedDepartmentId) {
          console.log(this.incidentForm.controls['assignedDepartmentId'].value);
          this.populateEmployees(this.incidentForm.controls['assignedDepartmentId'].value);
        }

        // disable some fields in edit screen and disable auto-populated ones in new record screen
        if (!this.incidence) {
          this.ModalTitle = 'Add Incidence';
          this.showUploadFileButtons = false;
        } else {
          this.isEditMode = true;
          this.ModalTitle = 'Edit Incidence';
          this.incidentForm.controls['routeCause'].disable();
          this.incidentForm.controls['description'].disable();
          this.incidentForm.controls['reportedIncidenceLatitude'].disable();
          this.incidentForm.controls['reportedIncidenceLongitude'].disable();
          this.incidentForm.controls['reporterName'].disable();
          this.incidentForm.controls['reporterEmail'].disable();
          this.incidentForm.controls['reporterFirstResponderAction'].disable();
          this.incidentForm.controls['reporterFeedbackRating'].disable();
          this.incidentForm.controls['address'].disable();
          this.incidentForm.controls['reporterFeedbackRating'].disable();
          this.incidentForm.controls['resolutionDate'].disable();
          this.incidentForm.controls['reporterDepartmentId'].disable();
        }
        this.incidentForm.controls['assignerId'].disable();
        this.incidentForm.controls['code'].disable();
        this.incidentForm.controls['assignedOrganizationId'].disable();
        this.incidentForm.controls['resolutionDate'].disable();
        this.incidentForm.controls['reporterName'].disable();
        this.incidentForm.controls['reporterEmail'].disable();
        this.incidentForm.controls['reporterDepartmentId'].disable();
        this.incidentForm.controls['assignedOrganizationId'].disable();
        this.incidentForm.controls['resolutionDate'].disable();
      });
      this.setOpenIncidenceValidator();
      this.getToken();
      this.populateNewIncidenceFields();
    }

    createEditIncidenceForm(item: SaveIncidence) {
      if (!item) {
        item = {
          serialNumber: 0,
          id: '',
          code: '',
          title: '',
          description: '',
          comment: '',
          routeCause: '',
          areaId: '',
          cityId: '',
          stateId: '',
          countryId: '',
          reportedIncidenceLatitude: '',
          reportedIncidenceLongitude: '',
          suggestion: '',
          assignerId: '',
          assigneeId: '',
          organizationId: '',
          assignedDepartmentId: '',
          assignedOrganizationId: '',
          reporterName: '',
          reporterEmail: '',
          reporterFirstResponderAction: '',
          reporterFeedbackRating: 0,
          managerFeedbackRating: 0,
          reporterDepartmentId: '',
          incidenceTypeId: '',
          incidenceStatusId: '',
          resolutionDate: '',
          address: ''
        }
      }
  this.incidentForm = this.fb.group({
        id: [item.id ],
        code: [item.code],
        title: [item.title],
        description: [item.description, Validators.required],
        comment: [item.comment],
        routeCause: [item.routeCause],
        areaId: [item.areaId],
        cityId: [item.cityId],
        stateId: [item.stateId],
        countryId: [item.countryId],
        reportedIncidenceLatitude: [item.reportedIncidenceLatitude],
        reportedIncidenceLongitude: [item.reportedIncidenceLongitude],
        suggestion: [item.suggestion],
        assignerId: [item.assignerId],
        assigneeId: [item.assigneeId],
        organizationId: [item.organizationId],
        assignedDepartmentId: [item.assignedDepartmentId],
        assignedOrganizationId: [item.assignedOrganizationId],
        reporterName: [item.reporterName],
        reporterEmail: [item.reporterEmail],
        reporterFirstResponderAction: [item.reporterFirstResponderAction],
        reporterFeedbackRating: [item.reporterFeedbackRating],
        managerFeedbackRating: [item.managerFeedbackRating],
        reporterDepartmentId: [item.reporterDepartmentId],
        incidenceTypeId: [item.incidenceTypeId, Validators.required],
        incidenceStatusId: [item.incidenceStatusId, [this.incidenceStatusValidator()]],
        resolutionDate: [item.resolutionDate],
        address: [item.address, Validators.required]
      });
    }

    // disallow users with member roles only from closing/reviewing incidences
    incidenceStatusValidator(): ValidatorFn {
      return (control: AbstractControl) => {
        // control.value gets the value of this.incidentForm.controls['incidenceStatusId']) 
        if ((control.value === this.closedIncidenceStatusId || control.value === this.underReviewIncidenceStatusId) && !this.isManagerialUser)
          return { 'unpermitted' : { value: control.value } };
        else
          return null;
      }
    }

    onOrganizationChange() {
      // removes the selected state and city and repopulates the state list upon changing the country
      this.incidentForm.patchValue({
        departmentId: '',
        assigneeId: ''
      });
      if (this.incidentForm.controls['organizationId']) {
        this.populateDepartments(this.incidentForm.controls['organizationId'].value);
      }
    }

    onDepartmentChange() {
      // removes the assignee
      this.incidentForm.patchValue({
        assigneeId: ''
      });
      if (this.incidentForm.controls['assignedDepartmentId']) {
        this.populateEmployees(this.incidentForm.controls['assignedDepartmentId'].value);
      }
    }

    private populateDepartments(organizationId: any) {
      const selectedOrganization = this.organizationList.find(m => m.id === organizationId);
      this.departmentList = selectedOrganization ? selectedOrganization.departments : [];
    };

    private populateEmployees(departmentId: any) {
      const selectedDepartment = this.departmentList.find(m => m.id === departmentId);
      this.userList = selectedDepartment ? selectedDepartment.users : [];
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
      this.incidentForm.patchValue({
        cityId: '',
        areaId: ''
      });
      this.populateCities(this.incidentForm.controls['stateId'].value);
      this.populateAreas(this.incidentForm.controls['stateId'].value);
    }

    onCityChange() {
      this.incidentForm.patchValue({
        areaId: ''
      });
    }

    onCountryChange() {
      // removes the selected state and city and repopulates the state list upon changing the country
      this.incidentForm.patchValue({
        stateId: '',
        cityId: '',
        areaId: ''
      });
      this.populateStates(this.incidentForm.controls['countryId'].value);
      this.populateCities(null);
      this.populateAreas(null);
    }

    submit() {
      console.log('create or edit incidence');
      console.log(this.incidentForm);
      // handle date of deployment timezone
      if (this.incidentForm.controls['resolutionDate'].value) {
        const dd = new Date(this.incidentForm.controls['resolutionDate'].value);
        dd.setHours(dd.getHours() - dd.getTimezoneOffset() / 60);
        this.incidentForm.patchValue({
          dateOfDeployment: dd,
        });
      }

      console.log(this.incidenceManagerFeedbackRating);
      // patching to enable the rating component consume integer rather than text
        this.incidentForm.patchValue({
          reporterFeedbackRating: this.incidenceReporterFeedbackRating,
          managerFeedbackRating: this.incidenceManagerFeedbackRating
        });

      if (this.incidentForm.valid) {
        this.saveIncidenceData = Object.assign({}, this.incidentForm.getRawValue());
        console.log(this.saveIncidenceData);
        const result$ = (this.saveIncidenceData.id) ? this.iService.updateIncidence(this.saveIncidenceData)
         : this.iService.createIncidence(this.saveIncidenceData);
          result$.subscribe(() => {
            this.toastr.success('Record saved successfully');
            // this.populateuserList();
            this.router.navigate(['/incidences']);
          }, error => {
            console.log(error);
            this.toastr.error(error.error.Error[0]);
          });
        }
      } // end of submit

  uploadPhoto(channel) {
    console.log('upload starts...');
    this.RefreshIncidenceMediaFiles();
    // angular doees not monkey patch the xhr.upload.onprogress in progressService hence no angular 
    // change detection for XMLHttpRequest requests. we fix with zones
    this.progressService.startTracking()
      .subscribe(progress => {
        console.log(progress);
        this.zone.run(() => {
          this.progress = progress;
        });
      },
      () => {  } );

    var nativeElement: HTMLInputElement = this.fileInput.nativeElement;

    if (nativeElement.files && nativeElement.files.length > 0) {
      for (let i = 0; i < nativeElement.files.length; i++) {
        var file = nativeElement.files![i];
        this.photoService.upload(this.incidentForm.controls['id'].value, file, channel)
      .subscribe(photo => {
        // console.log(photo);
        this.incidenceMedia.push(photo);
        this.populateMedia();
        this.toastr.success('File uploaded successfully');
      },
      error => {
        // console.log(error.error); error is different here, it does not come in Error array
        this.toastr.error(error.error);
      });
    }
    nativeElement.value = '';
    }
  }

  // change serverPath to fileName
  createImgPath(serverPath: string) {
    return this.imgUrl + serverPath;
    // console.log(this.imgUrl + serverPath);
  }

  onChange() {
    this.photoVideoToggle = !this.photoVideoToggle;
  }

  populateMedia() {
    console.log('pop media');
    console.log(this.incidenceMedia);
    this.incidenceResolutionPhotos = this.getMediaFiles(this.mediaType.photo, this.mediaChannel.webResolved);
    // console.log('photo resolved');
    // console.log(this.incidenceResolutionPhotos);

    this.incidenceResolutionVideos = this.getMediaFiles(this.mediaType.video, this.mediaChannel.webResolved);
    // console.log('video resolved');
    // console.log(this.incidenceResolutionVideos);

    this.incidenceReportedPhotos = this.getMediaFiles(this.mediaType.photo, this.mediaChannel.mobileReported);
    // console.log('photo reported');
    // console.log(this.incidenceReportedPhotos);

    this.incidenceReportedVideos = this.getMediaFiles(this.mediaType.video, this.mediaChannel.mobileReported);
    // console.log('video reported');
    // console.log(this.incidenceResolutionVideos);
  }

  // for photo and video slide show
  getMediaFiles(mediaType, iMediaChannel) {
    this.iMediaData = [];
    if (this.incidenceMedia) {
      const incidenceResolutionData = [] = this.incidenceMedia
      .filter(m => m.isVideo === mediaType && m.fileUploadChannel === iMediaChannel);
    // console.log(incidenceResolutionData);
      if (incidenceResolutionData && incidenceResolutionData.length > 0) {
        for (let i = 0; i < incidenceResolutionData.length; i++) {
          this.iMediaData.push({
            id: incidenceResolutionData[i].id,
            image: this.createImgPath(incidenceResolutionData[i].fileName),
            thumbImage: this.createImgPath(incidenceResolutionData[i].fileName),
            title: incidenceResolutionData[i].description,
            alt: incidenceResolutionData[i].description
          });
        }
      }
      // console.log(this.iMediaData);
      return this.iMediaData;
    }
  }

  manageResolutionVideos() {
    this.showManageResolutionVideos = !this.showManageResolutionVideos;
  }

  manageResolutionPhotos() {
    this.showManageResolutionPhotos = !this.showManageResolutionPhotos;
  }

  manageReportedVideos() {
    this.showManageReportedVideos = !this.showManageReportedVideos;
  }

  manageReportedPhotos() {
    this.showManageReportedPhotos = !this.showManageReportedPhotos;
  }

  // set assignee as compulsory when incidence status becomes open/re-opened vice versa
  setOpenIncidenceValidator() {
    const assigneeControl = this.incidentForm.get('assigneeId');

    this.incidentForm.get('incidenceStatusId').valueChanges
      .subscribe(incidenceStatusId => {
        console.log(incidenceStatusId);
        if (incidenceStatusId === this.openIncidenceStatusId || incidenceStatusId === this.reOpenedIncidenceStatusId) {
          assigneeControl.setValidators([Validators.required]);
          // Set the Assigner at the backend i.e after save action and incidence status is Open or Re-opened. No way to know if status change is happening after incidence was previously assigned or newly assigned hence assigner cant just go blank.
          // As we still want to see assigner after incidence is Under Review or Closed
          // console.log(this.currentLoggedInUserId);
          // console.log(this.allUserList);
          // this.incidentForm.patchValue({
          //   assignerId: this.currentLoggedInUserId
          // });
        }
        else {
          assigneeControl.setValidators(null);
        }
        // we want to see assigner after incidence closure
        // else if (incidenceStatusId !== this.openIncidenceStatusId && incidenceStatusId !== this.reOpenedIncidenceStatusId) {
        //   assigneeControl.setValidators(null);
        //   this.incidentForm.patchValue({
        //     assignerId: ''
        //   });
        // }
        assigneeControl.updateValueAndValidity();
    });
  }

  getToken() {
    const userToken = localStorage.getItem('token');
    const decodedToken = this.jwtHelper.decodeToken(userToken);
    // console.log(decodedToken);
    this.currentLoggedInUserId = decodedToken.nameid;
    this.currentLoggedInUserOrgId = decodedToken.upn;
  }

  populateNewIncidenceFields() {
    console.log(this.departmentList);
    console.log(this.currentLoggedInUserOrgId);
    if (this.currentLoggedInUserOrgId) {
      this.incidentForm.patchValue({
        assignedOrganizationId: this.currentLoggedInUserOrgId
      });
      this.populateDepartments(this.currentLoggedInUserOrgId);
    }
  }

  RefreshIncidenceMediaFiles(){
    //console.log('re-pop media');
    this.photoService.getIncidenceMedia(this.incidentForm.controls['id'].value)
      .subscribe(result => {
        console.log('result..:');
        console.log(result);
        this.incidenceMedia = result
      });
    this.populateMedia();
  }
}
