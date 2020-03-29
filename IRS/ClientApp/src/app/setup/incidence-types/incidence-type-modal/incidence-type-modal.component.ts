import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { IncidenceType } from 'app/_models/incidenceType';
import { IncidenceTypeService } from 'app/shared/services/incidence-type.service';

@Component({
  selector: 'app-incidence-type-modal',
  templateUrl: './incidence-type-modal.component.html',
  styleUrls: ['./incidence-type-modal.component.scss']
})
export class IncidenceTypeModalComponent implements OnInit {
  queryResult: any = {};
  incidenceTypeData: any;
  query: any = {
    pageSize: 50
  };

  // form data
  incidenceTypeEditForm: FormGroup;

  // import the data for edit action
  @Input() itdData: IncidenceType;
  @Input() screenMode: string;
  @Input() incidenceTypesList: any[];
  saveItdData: IncidenceType;

  // modal data
  ModalTitle = '';
  incidenceTypeDeptListData: any[];
  @Output() incidenceTypeListOutput = new EventEmitter();
  saveIncidenceTypeData: IncidenceType;

  constructor(public activeModal: NgbActiveModal, private fb: FormBuilder, private itService: IncidenceTypeService,
     private toastr: ToastrService, private router: Router) { }

  ngOnInit() {
    // console.log('daa');
    // console.log(this.incidenceTypeData);
    this.createEditIncidenceTypeForm(this.incidenceTypeData);
    if (this.screenMode === 'view') {
      this.incidenceTypeEditForm.disable();
      this.ModalTitle = 'View Incidence Type';
    } else if (this.screenMode === 'add') {
      this.ModalTitle = 'Add Incidence Type';
    } else if (this.screenMode === 'edit') {
      this.ModalTitle = 'Edit Incidence Type';
      this.incidenceTypeEditForm.controls['name'].disable();
    }
  }

  createEditIncidenceTypeForm(item: IncidenceType) {
    if (!item) {
      item = {
        id: '',
        name: '',
        description: ''
      }
    }
    this.incidenceTypeEditForm = this.fb.group({
      id: [item.id || ''],
      name: [item.name || '', Validators.required],
      description: [item.description || '']
    });
  }

  private populateIncidenceTypeList() {
    this.itService.getIncidenceTypesList(this.query)
      .subscribe(result => {
        this.incidenceTypeListOutput.emit(result);
      });
  }

  submit() {
    if (this.incidenceTypeEditForm.valid) {
      this.saveIncidenceTypeData = Object.assign({}, this.incidenceTypeEditForm.value);
      console.log(this.saveIncidenceTypeData);
      const result$ = (this.saveIncidenceTypeData.id) ? this.itService.updateIncidenceType(this.saveIncidenceTypeData)
       : this.itService.createIncidenceType(this.saveIncidenceTypeData);
        result$.subscribe(() => {
          this.toastr.success('Record saved successfully');
          this.populateIncidenceTypeList();
          this.activeModal.dismiss('Cross click')
        }, error => {
          // this.toastr.error('Error saving record');
          this.toastr.error(error.error.Error[0]);
        });
    }
  }
}
