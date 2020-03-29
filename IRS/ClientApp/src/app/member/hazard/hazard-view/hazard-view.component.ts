import { Component, OnInit, ElementRef, ViewChild, NgZone } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { NgbDatepickerConfig, NgbCalendar, NgbDate, NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';
import { HazardService } from 'app/shared/services/hazard.services';
import { PhotoService } from 'app/shared/services/photo.service';
import { ProgressService } from 'app/shared/services/progress.service';
import { Photo } from 'app/_models/media/photo';
import { Media } from 'app/_models/media/media';
import { environment } from 'environments/environment';
import { NgbCarousel } from '@ng-bootstrap/ng-bootstrap';
import { VgAPI } from 'videogular2/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { SaveHazard } from 'app/_models/saveHazard';

@Component({
  selector: 'app-hazard-view',
  templateUrl: './hazard-view.component.html',
  styleUrls: ['./hazard-view.component.scss']
})
export class HazardViewComponent implements OnInit {
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
    hazardTypeList: any[];
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

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      // console.log('inc...');
      // console.log(data['hazardData']);
      if (data['hazardData']) {
        this.hazard = data['hazardData']
       console.log('inc...');
       console.log(this.hazard);
      }
      // this.createEditHazardForm(this.hazard);
      // console.log(this.incidentForm.controls['reporterFeedbackRating'].value);
      // this.hazardReporterFeedbackRating = this.incidentForm.controls['reporterFeedbackRating'].value;
      // this.hazardManagerFeedbackRating = this.incidentForm.controls['managerFeedbackRating'].value;

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
      this.hazardTypeList = data['hazardTypeListData'];
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
      } else {
        this.isManagerialUser = false;
      }
      this.ModalTitle = 'View Hazard';
  });
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

  createImgPath(serverPath: string) {
    return this.imgUrl + serverPath;
    // console.log(this.imgUrl + serverPath);
  }

  uploadPhoto(channel) {
  }

  onChange() {
    this.photoVideoToggle = !this.photoVideoToggle;
  }
}
