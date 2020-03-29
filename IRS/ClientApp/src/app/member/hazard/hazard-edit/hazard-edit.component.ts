
import { Component, OnInit, ElementRef, ViewChild, NgZone } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm, AbstractControl, ValidatorFn } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { NgbDatepickerConfig, NgbCalendar, NgbDate, NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';
import { SaveHazard } from 'app/_models/saveHazard';
import { HazardService } from 'app/shared/services/hazard.services';
import { PhotoService } from 'app/shared/services/photo.service';
import { ProgressService } from 'app/shared/services/progress.service';
import { Photo } from 'app/_models/media/photo';
import { Media } from 'app/_models/media/media';
import { environment } from 'environments/environment';
import { NgbCarousel } from '@ng-bootstrap/ng-bootstrap';
import { VgAPI } from 'videogular2/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-hazard-edit',
  templateUrl: './hazard-edit.component.html',
  styleUrls: ['./hazard-edit.component.scss']
})
export class HazardEditComponent implements OnInit {
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
    hazardStatusList: any[];
    sliderWidth: Number = 940;
    sliderImageWidth: Number = 250;
    sliderImageHeight: Number = 200;

    hazardMedia: any[];
    hazardResolutionPhotos: any[] = [];
    hazardResolutionVideos: any[] = [];
    hazardReportedPhotos: any[] = [];
    hazardReportedVideos: any[] = [];
    hazardReporterFeedbackRating = 0;
    hazardManagerFeedbackRating = 0;
    showUploadFileButtons = true;

    isManagerialUser = false;
    loggedIntUserRoles: any[];
    editable: true;
    hazardForm: FormGroup;
    ModalTitle = '';
    hazard: SaveHazard;
    saveHazardData: SaveHazard;
    // check that this conforms with backend
    userPreferredContactList = [{'id': 0, name: 'Email'}, {'id': 1, name: 'SMS'}, {'id': 2, name: 'Phone'}];
    userGenderList = [{'id': 0, name: 'Male'}, {'id': 1, name: 'Female'}];
    memberHazardStatusList = ['Resolved', 'Re-Opened', 'Open'];
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

    isEditMode = false;
    showManageResolutionVideos = false;
    showManageResolutionPhotos = false;
    showManageReportedVideos = false;
    showManageReportedPhotos = false;
    openHazardStatusId = 'a64769a0-ce38-4414-afdb-c6bfac9056a1';
    reOpenedHazardStatusId = 'f59fe8e6-646a-4b04-7fbc-08d702eeea0d';
    closedHazardStatusId = '0839bc87-7d23-431d-86c4-3ca1e4f190ef';
    underReviewHazardStatusId = '53986162-1dae-45f7-8771-08d702ea4b5c';
    currentLoggedInUserId: any;
    currentLoggedInUserOrgId: any;
    jwtHelper = new JwtHelperService();
    isOrgAdminUser = false;

    @ViewChild('carousel') carousel: NgbCarousel;

    constructor(
      private route: ActivatedRoute,
      private hazardService: HazardService,
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
        console.log('inc...');
        console.log(data['hazardData']);
        if (data['hazardData']) {
          this.hazard = data['hazardData']
          console.log('inc...');
         console.log(this.hazard);
        }
        this.createEditHazardForm(this.hazard);
        console.log(this.hazardForm.controls['reporterFeedbackRating'].value);
        this.hazardReporterFeedbackRating = this.hazardForm.controls['reporterFeedbackRating'].value;
        this.hazardManagerFeedbackRating = this.hazardForm.controls['managerFeedbackRating'].value;

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
        this.hazardStatusList = data['hazardStatusListData'];
        this.hazardMedia = data['hazardResolutionPhotosData'];
        // console.log(this.hazardMedia);
        this.populateMedia();
        this.loggedIntUserRoles = data['UserRolesData'];
        console.log(this.loggedIntUserRoles);
        if (this.loggedIntUserRoles) {
          this.isManagerialUser = this.loggedIntUserRoles.indexOf('Admin') > -1
          || this.loggedIntUserRoles.indexOf('Organization Admin') > -1
          || this.loggedIntUserRoles.indexOf('Manager') > -1;

          this.isOrgAdminUser = this.loggedIntUserRoles.indexOf('Organization Admin') > -1;
        } else {
          this.isManagerialUser = false;
        }
        // console.log(this.departmentList);
        // console.log(this.organizationList);
        // console.log(this.countryList);
        // console.log(this.hazard.hazardStatusId);
        // console.log(this.incidentForm.controls['hazardStatusId'].value);

        // process the timezone differences that causes day to go minus one
        if (this.hazard && this.hazard.resolutionDate) {
          const dd = new Date(this.hazardForm.controls['resolutionDate'].value);
          dd.setHours(dd.getHours() - dd.getTimezoneOffset() / 60);
          this.hazardForm.patchValue({
            resolutionDate: dd
          });
        }

        if (this.hazard && this.hazard.countryId) {
          this.populateStates(this.hazardForm.controls['countryId'].value);
        }
        if (this.hazard && this.hazard.stateId) {
          this.populateCities(this.hazardForm.controls['stateId'].value);
          this.populateAreas(this.hazardForm.controls['stateId'].value);
        }
        if (this.hazard && this.hazard.assignedOrganizationId) {
          console.log(this.hazardForm.controls['assignedOrganizationId'].value);
          this.populateDepartments(this.hazardForm.controls['assignedOrganizationId'].value);
        }
        if (this.hazard && this.hazard.assignedDepartmentId) {
          console.log(this.hazardForm.controls['assignedDepartmentId'].value);
          this.populateEmployees(this.hazardForm.controls['assignedDepartmentId'].value);
        }

        // disable some fields in edit screen and disable auto-populated ones in new record screen
        if (!this.hazard) {
          this.ModalTitle = 'Add Hazard';
          this.showUploadFileButtons = false;
        } else {
          this.isEditMode = true;
          this.ModalTitle = 'Edit Hazard';
          this.hazardForm.controls['routeCause'].disable();
          this.hazardForm.controls['description'].disable();
          this.hazardForm.controls['reportedHazardLatitude'].disable();
          this.hazardForm.controls['reportedHazardLongitude'].disable();
          this.hazardForm.controls['reporterName'].disable();
          this.hazardForm.controls['reporterEmail'].disable();
          this.hazardForm.controls['reporterFirstResponderAction'].disable();
          this.hazardForm.controls['reporterFeedbackRating'].disable();
          this.hazardForm.controls['address'].disable();
          this.hazardForm.controls['reporterFeedbackRating'].disable();
          this.hazardForm.controls['resolutionDate'].disable();
          this.hazardForm.controls['reporterDepartmentId'].disable();
        }
        this.hazardForm.controls['assignerId'].disable();
        this.hazardForm.controls['code'].disable();
        this.hazardForm.controls['reporterName'].disable();
        this.hazardForm.controls['reporterEmail'].disable();
        this.hazardForm.controls['reporterDepartmentId'].disable();
        this.hazardForm.controls['assignedOrganizationId'].disable();
        this.hazardForm.controls['resolutionDate'].disable();
      });
      this.setOpenHazardValidator();
      this.getToken();
      this.populateNewHazardFields();
    }

    createEditHazardForm(item: SaveHazard) {
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
          reportedHazardLatitude: '',
          reportedHazardLongitude: '',
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
          incidenceStatusId: '',
          resolutionDate: '',
          address: ''
        }
      }
  this.hazardForm = this.fb.group({
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
        reportedHazardLatitude: [item.reportedHazardLatitude],
        reportedHazardLongitude: [item.reportedHazardLongitude],
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
        incidenceStatusId: [item.incidenceStatusId, [this.hazardStatusValidator()]],
        resolutionDate: [item.resolutionDate],
        address: [item.address, Validators.required]
      });
    }

    // disallow users with member roles only from closing/reviewing hazards
    hazardStatusValidator(): ValidatorFn {
      return (control: AbstractControl) => {
        // control.value gets the value of this.incidentForm.controls['hazardStatusId'])
        if ((control.value === this.closedHazardStatusId || control.value === this.underReviewHazardStatusId) && !this.isManagerialUser)
          return { 'unpermitted' : { value: control.value } };
        else
          return null;
      }
    }

    onOrganizationChange() {
      // removes the selected state and city and repopulates the state list upon changing the country
      this.hazardForm.patchValue({
        departmentId: '',
        assigneeId: ''
      });
      if (this.hazardForm.controls['organizationId']) {
        this.populateDepartments(this.hazardForm.controls['organizationId'].value);
      }
    }

    onDepartmentChange() {
      // removes the assignee
      this.hazardForm.patchValue({
        assigneeId: ''
      });
      if (this.hazardForm.controls['assignedDepartmentId']) {
        this.populateEmployees(this.hazardForm.controls['assignedDepartmentId'].value);
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
      this.hazardForm.patchValue({
        cityId: '',
        areaId: ''
      });
      this.populateCities(this.hazardForm.controls['stateId'].value);
      this.populateAreas(this.hazardForm.controls['stateId'].value);
    }

    onCityChange() {
      this.hazardForm.patchValue({
        areaId: ''
      });
    }

    onCountryChange() {
      // removes the selected state and city and repopulates the state list upon changing the country
      this.hazardForm.patchValue({
        stateId: '',
        cityId: '',
        areaId: ''
      });
      this.populateStates(this.hazardForm.controls['countryId'].value);
      this.populateCities(null);
      this.populateAreas(null);
    }

    submit() {
      console.log('create or edit hazard');
      console.log(this.hazardForm);
      // handle date of deployment timezone
      if (this.hazardForm.controls['resolutionDate'].value) {
        const dd = new Date(this.hazardForm.controls['resolutionDate'].value);
        dd.setHours(dd.getHours() - dd.getTimezoneOffset() / 60);
        this.hazardForm.patchValue({
          dateOfDeployment: dd,
        });
      }

      console.log(this.hazardManagerFeedbackRating);
      // patching to enable the rating component consume integer rather than text
        this.hazardForm.patchValue({
          reporterFeedbackRating: this.hazardReporterFeedbackRating,
          managerFeedbackRating: this.hazardManagerFeedbackRating
        });

      if (this.hazardForm.valid) {
        this.saveHazardData = Object.assign({}, this.hazardForm.getRawValue());
        console.log(this.saveHazardData);
        const result$ = (this.saveHazardData.id) ? this.hazardService.updateHazard(this.saveHazardData)
         : this.hazardService.createHazard(this.saveHazardData);
          result$.subscribe(() => {
            this.toastr.success('Record saved successfully');
            // this.populateuserList();
            this.router.navigate(['/hazards']);
          }, error => {
            console.log(error);
            this.toastr.error(error.error.Error[0]);
          });
        }
      } // end of submit

    uploadPhoto(channel) {
    console.log('upload starts...');
    this.RefreshHazardMediaFiles();
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
        this.photoService.uploadHazardPhoto(this.hazardForm.controls['id'].value, file, channel)
      .subscribe(photo => {
        // console.log(photo);
        this.hazardMedia.push(photo);
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
    console.log(this.hazardMedia);
    this.hazardResolutionPhotos = this.getMediaFiles(this.mediaType.photo, this.mediaChannel.webResolved);
    // console.log('photo resolved');
    // console.log(this.hazardResolutionPhotos);

    this.hazardResolutionVideos = this.getMediaFiles(this.mediaType.video, this.mediaChannel.webResolved);
    // console.log('video resolved');
    // console.log(this.hazardResolutionVideos);

    this.hazardReportedPhotos = this.getMediaFiles(this.mediaType.photo, this.mediaChannel.mobileReported);
    // console.log('photo reported');
    // console.log(this.hazardReportedPhotos);

    this.hazardReportedVideos = this.getMediaFiles(this.mediaType.video, this.mediaChannel.mobileReported);
    // console.log('video reported');
    // console.log(this.hazardResolutionVideos);
  }

  // for photo and video slide show
  getMediaFiles(mediaType, iMediaChannel) {
    this.iMediaData = [];
    if (this.hazardMedia) {
      const hazardResolutionData = [] = this.hazardMedia
      .filter(m => m.isVideo === mediaType && m.fileUploadChannel === iMediaChannel);
    // console.log(hazardResolutionData);
      if (hazardResolutionData && hazardResolutionData.length > 0) {
        for (let i = 0; i < hazardResolutionData.length; i++) {
          this.iMediaData.push({
            id: hazardResolutionData[i].id,
            image: this.createImgPath(hazardResolutionData[i].fileName),
            thumbImage: this.createImgPath(hazardResolutionData[i].fileName),
            title: hazardResolutionData[i].description,
            alt: hazardResolutionData[i].description
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

  // set assignee as compulsory when hazard status becomes open/re-opened vice versa
  setOpenHazardValidator() {
    const assigneeControl = this.hazardForm.get('assigneeId');

    this.hazardForm.get('incidenceStatusId').valueChanges
      .subscribe(incidenceStatusId => {
        // console.log(incidenceStatusId);
        if (incidenceStatusId === this.openHazardStatusId || incidenceStatusId === this.reOpenedHazardStatusId) {
          assigneeControl.setValidators([Validators.required]);
        }
        else {
          assigneeControl.setValidators(null);
        }
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

  populateNewHazardFields() {
    console.log(this.departmentList);
    console.log(this.currentLoggedInUserOrgId);
    if (this.currentLoggedInUserOrgId) {
      this.hazardForm.patchValue({
        assignedOrganizationId: this.currentLoggedInUserOrgId
      });
      this.populateDepartments(this.currentLoggedInUserOrgId);
    }
  }

  RefreshHazardMediaFiles(){
    console.log('re-pop media');
    this.photoService.getHazardMedia(this.hazardForm.controls['id'].value)
      .subscribe(result => {
        console.log('result..:');
        console.log(result);
        this.hazardMedia = result
      });
    this.populateMedia();
  }
}
