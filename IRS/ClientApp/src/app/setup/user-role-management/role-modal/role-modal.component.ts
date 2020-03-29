import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { UsersRoles } from 'app/_models/usersRoles';
import { UserService } from 'app/shared/services/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-role-modal',
  templateUrl: './role-modal.component.html',
  styleUrls: ['./role-modal.component.scss']
})
export class RoleModalComponent implements OnInit {
  @Input() userAndRoleData: UsersRoles;
  @Input() screenMode: string;
  roles: any[] = [];
  ModalTitle = '';
  @Output() userListOutput = new EventEmitter();
  userListData: any[];
  query: any = {
    pageSize: 50
  };

  constructor(public activeModal: NgbActiveModal, public userService: UserService, private toastr: ToastrService) { }

  ngOnInit() {
    this.roles = this.getRolesArray(this.userAndRoleData);
    // console.log('user roles...');
    // console.log(this.userAndRole.roles);
    // console.log('new roles...');
    // console.log(this.roles);
  }

  private getRolesArray(userRoleData) {
    const roles = [];
    const userRoles = userRoleData.rolesForUser;
    const availableRoles: any[] = [
      {name: 'Admin', value: 'Admin'},
      {name: 'Organization Admin', value: 'Organization Admin'},
      {name: 'Manager', value: 'Manager'},
      {name: 'Member', value: 'Member'},
      {name: 'Executive', value: 'Executive'},
    ];

    for (let i = 0; i < availableRoles.length; i++) {
      let isMatch = false;
      for (let j = 0; j < userRoles.length; j++) {
        if (availableRoles[i].name === userRoles[j]) {
          isMatch = true;
          availableRoles[i].checked = true;
          roles.push(availableRoles[i]);
          break;
        }
      }
      if (!isMatch) {
        availableRoles[i].checked = false;
        roles.push(availableRoles[i]);
      }
    }
    return roles;
  }

  private populateUserRolesList() {
    this.userService.getUsersWithRoles(this.query)
      .subscribe(result => {
        this.userListOutput.emit(result);
      });
  }

  updateRoles() {
    console.log('my roles...');
    const rolesToUpdate = {
      // ... is a spread operator in js that creates a new array
      roleNames: [...this.roles.filter(el => el.checked === true).map(el => el.name)]
    };
    // console.log(rolesToUpdate);
    console.log(rolesToUpdate);
    if (rolesToUpdate) {
      this.userService.updateUserRoles(this.userAndRoleData, rolesToUpdate).subscribe(() => {
        this.userAndRoleData.roles = [...rolesToUpdate.roleNames];
        this.populateUserRolesList();
        this.toastr.success('Role Successfully updated');
      }, error => {
        this.toastr.error(error.error.Error[0]);
        //this.toastr.error('Operation Failed');
        // console.log(error);
      });
    }
    this.activeModal.dismiss('Cross click')
  }

}
