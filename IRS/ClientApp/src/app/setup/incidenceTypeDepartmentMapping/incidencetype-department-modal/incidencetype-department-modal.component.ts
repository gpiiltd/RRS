import { Component, OnInit, Input, ViewChild, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { SaveIncidenceTypeDepartment } from 'app/_models/saveIncidenceTypeDepartment';
import { IncidenceTypeDepartment } from 'app/_models/incidenceTypeDepartment';
import { IncidenceTypeDepartmentService } from 'app/shared/services/incidencetype-department.service';

@Component({
  selector: 'app-incidencetype-department-modal',
  templateUrl: './incidencetype-department-modal.component.html',
  styleUrls: ['./incidencetype-department-modal.component.scss']
})
export class IncidencetypeDepartmentModalComponent implements OnInit {
    // form data
    incidenceTypeDepartmentEditForm: FormGroup;
    unmappedIncidenceTypes: any[];

    // import the data for edit action
    @Input() itdData: IncidenceTypeDepartment;
    @Input() screenMode: string;
    @Input() incidenceTypesList: any[];
    @Input() departmentList: any[];
    @Input() organizationList: any[];
    saveItdData: SaveIncidenceTypeDepartment;

    // modal data
    ModalTitle = '';
    incidenceTypeDeptListData: any[];
    @Output() itdOutput = new EventEmitter();
    query: any = {
      pageSize: 50
    };

  constructor(public activeModal: NgbActiveModal, private fb: FormBuilder, private itdService: IncidenceTypeDepartmentService,
    private toastr: ToastrService, private router: Router) { }

  ngOnInit() {
    this.createIncidenceTypeDepartmentForm(this.itdData);
    this.ModalTitle = 'Add Incidence Type - Department Mapping';
  }

  private populateList() {
    // we are not removing any mapped incidece type from drop down
    // const index = this.unmapedIncidenceTypesList.findIndex(x => x.id === this.saveItdData.incidenceTypeId);
    // this.unmapedIncidenceTypesList.splice(index, 1);
    this.itdService.getIncidenceTypeDepartmentList(this.query)
      .subscribe(result => {
        this.incidenceTypeDeptListData = result;
        this.itdOutput.emit(this.incidenceTypeDeptListData);
      });
  }

  createIncidenceTypeDepartmentForm(item: SaveIncidenceTypeDepartment) {
    if (!item) {
      item = {
        id: '',
        incidenceTypeId: '',
        departmentId: '',
      }
    }
    this.incidenceTypeDepartmentEditForm = this.fb.group({
      id: [item.id || ''],
      incidenceTypeId: [item.incidenceTypeId || '', Validators.required],
      departmentId: [item.departmentId || '', Validators.required],
    });
  }

  submit() {
    if (this.incidenceTypeDepartmentEditForm.valid) {
      this.saveItdData = Object.assign({}, this.incidenceTypeDepartmentEditForm.value);
      const result$ = this.itdService.createIncidenceTypeDepartment(this.saveItdData);
        result$.subscribe(() => {
          this.toastr.success('Record saved successfully');
          this.populateList();
          this.activeModal.dismiss('Cross click');
        }, error => {
          this.toastr.error(error.error.Error[0]);
        });
    }
  }

}
