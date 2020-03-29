import { KeyValuePair } from 'app/_models/keyValuePair';
import { Component, OnInit } from '@angular/core';
import { User } from 'app/_models/user';
import { ActivatedRoute } from '@angular/router';
import { UserService } from 'app/shared/services/user.service';
import { formatDate } from '@angular/common';
import { environment } from 'environments/environment';

@Component({
    selector: 'app-user-profile-page',
    templateUrl: './user-profile-page.component.html',
    styleUrls: ['./user-profile-page.component.scss']
})

export class UserProfilePageComponent implements OnInit {
    currentPage = 'About';
    user: User;
    countryList: any[];
    stateList: any[];
    areaList: any[];
    profilePhotoData: any;
    profilePhotoUrl = 'assets/img/portrait/avatars/avatar-08.png';
    imgUrl = environment.userImgUrl;

    // check that this conforms with backend
    userPreferredContactList = [{'id': 0, name: 'Email'}, {'id': 1, name: 'SMS'}, {'id': 2, name: 'Phone'}]
    userGenderList = [{'id': 0, name: 'Male'}, {'id': 1, name: 'Female'}]

    constructor(private route: ActivatedRoute, private userService: UserService) { }

    ngOnInit() {
        this.route.data.subscribe(data => {
            //this.user = data['userProfileData']; // userProfileData from route.ts
            this.countryList = data['countryListData'];
            this.stateList = data['stateListData'];
            this.areaList = data['areaListData'];

            if (data['userProfileData'].id) {
              this.user = data['userProfileData'];
              // this.setUser(data['userProfileData']);
              // console.log('loading user..');
              // console.log(data['userProfileData']);
              // console.log(this.user);
            }

            this.profilePhotoData = data['UserProfilePhotoData'];
            // console.log(this.profilePhotoData);
            // get url for u
            if (this.profilePhotoData) {
              this.profilePhotoUrl = this.createImgPath(this.profilePhotoData.fileName);
            }
          });

          this.populateAreas(); // filter lga's based on auto-selected State

          if (this.user.dateOfBirth === '0001-01-01T00:00:00') {
            this.user.dateOfBirth = '';
          }
          if (this.user.lastActive === '0001-01-01T00:00:00') {
            this.user.lastActive = '';
          }
          if (this.user.dateOfDeployment === '0001-01-01T00:00:00') {
            this.user.dateOfDeployment = '';
          }
          if (this.user.dateOfSignOff === '0001-01-01T00:00:00') {
            this.user.dateOfSignOff = '';
          }
    }

    showPage(page: string) {
        this.currentPage = page;
      };

    private populateAreas() {
        // console.log(this.user.stateOfOriginId);
        if (!this.user.stateOfDeploymentId) {
        const selectedState = this.stateList.find(m => m.id === this.user.stateOfDeploymentId);
        this.areaList = selectedState ? selectedState.areas : [];
        }
      };

      createImgPath(fileName: string) {
        // return profilePhotoUrl when user has no uploaded photo in db
        if (fileName !== '') {
          console.log(this.imgUrl + fileName);
          return this.imgUrl + fileName;
        }
        return this.profilePhotoUrl;
      }
}
