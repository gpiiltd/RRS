import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from 'app/shared/services/user.service';
import { User } from 'app/_models/user';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-user-view',
  templateUrl: './user-view.component.html',
  styleUrls: ['./user-view.component.scss']
})
export class UserViewComponent implements OnInit {
currentJustify = 'start';
currentPage = 'About';
user: User;
countryList: any[];
stateList: any[];
areaList: any[];
cityList: any[];
organizationList: any[];
departmentList: any[];
isGlobalAdminUser = false;
jwtHelper = new JwtHelperService();

// check that this conforms with backend
userPreferredContactList = [{'id': 0, name: 'Email'}, {'id': 1, name: 'SMS'}, {'id': 2, name: 'Phone'}]
// check that this conforms with backend
userGenderList = [{'id': 0, name: 'Male'}, {'id': 1, name: 'Female'}]

constructor(private route: ActivatedRoute, private userService: UserService) { }

ngOnInit() {
    this.route.data.subscribe(data => {
        this.user = data['userViewData']; // userProfileData from route.ts
        this.countryList = data['countryListData'];
        this.stateList = data['stateListData'];
        this.cityList = data['cityListData'];
        this.areaList = data['areaListData'];
        this.organizationList = data['OrgListData'];
      this.departmentList = data['DeptListData'];
      });
      this.getToken();
}

showPage(page: string) {
    this.currentPage = page;
  };

  getToken() {
    const userToken = localStorage.getItem('token');
    const decodedToken = this.jwtHelper.decodeToken(userToken);
    if (decodedToken.unique_name === 'Admin') {
      this.isGlobalAdminUser = true;
    }
  }
}
