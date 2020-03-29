import { Component, OnInit, ElementRef, ViewChild, NgZone } from '@angular/core';
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
  selector: 'app-incidence-view',
  templateUrl: './incidence-view.component.html',
  styleUrls: ['./incidence-view.component.scss']
})
export class IncidenceViewComponent implements OnInit {
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

    incidenceMedia: any[];
    incidenceResolutionPhotos: any[] = [];
    incidenceResolutionVideos: any[] = [];
    incidenceReportedPhotos: any[] = [];
    incidenceReportedVideos: any[] = [];
    incidenceReporterFeedbackRating = 0;
    incidenceManagerFeedbackRating = 0;
    showUploadFileButtons = true;

    isManagerialUser = false;
    loggedIntUserRoles: any[];
    editable: true;
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

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      // console.log('inc...');
      // console.log(data['incidenceData']);
      if (data['incidenceData']) {
        this.incidence = data['incidenceData']
       console.log('inc...');
       console.log(this.incidence);
      }
      // this.createEditIncidenceForm(this.incidence);
      // console.log(this.incidentForm.controls['reporterFeedbackRating'].value);
      // this.incidenceReporterFeedbackRating = this.incidentForm.controls['reporterFeedbackRating'].value;
      // this.incidenceManagerFeedbackRating = this.incidentForm.controls['managerFeedbackRating'].value;

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
      this.loggedIntUserRoles = data['UserRolesData'];
      console.log(this.loggedIntUserRoles);
      if (this.loggedIntUserRoles) {
        this.isManagerialUser = this.loggedIntUserRoles.indexOf('Admin') > -1
        || this.loggedIntUserRoles.indexOf('Organization Admin') > -1
        || this.loggedIntUserRoles.indexOf('Manager') > -1;
      } else {
        this.isManagerialUser = false;
      }
      this.ModalTitle = 'View Incidence';
  });
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
