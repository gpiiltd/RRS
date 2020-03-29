import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { IncidenceStatus } from 'app/_models/incidenceStatus';
import { IncidenceStatusService } from 'app/shared/services/incidence-status.service';

@Component({
  selector: 'app-incidence-status-modal',
  templateUrl: './incidence-status-modal.component.html',
  styleUrls: ['./incidence-status-modal.component.scss']
})
export class IncidenceStatusModalComponent implements OnInit {
    incidenceStatusEditForm: FormGroup;

    // import the data for edit action
    @Input() incidenceStatusData: IncidenceStatus;
    @Input() screenMode: string;
    saveIncidenceStatusData: IncidenceStatus;

    // modal data
    ModalTitle = '';
    @Output() incidenceStatusListOutput = new EventEmitter();
    query: any = {
      pageSize: 50
    };

  constructor(public activeModal: NgbActiveModal, private fb: FormBuilder, private isService: IncidenceStatusService,
     private toastr: ToastrService, private router: Router) { }

  ngOnInit() {
    this.createEditIncidenceStatusForm(this.incidenceStatusData);
    if (this.screenMode === 'view') {
      this.incidenceStatusEditForm.disable();
      this.ModalTitle = 'View Incidence Status';
    } else if (this.screenMode === 'add') {
      this.ModalTitle = 'Add Incidence Status';
    } else if (this.screenMode === 'edit') {
      this.ModalTitle = 'Edit Incidence Status';
      this.incidenceStatusEditForm.controls['name'].disable(); // make Code non editable in edit screen
    }
    }

  createEditIncidenceStatusForm(item: IncidenceStatus) {
    if (!item) {
      item = {
        id: '',
        name: '',
        description: ''
      }
    }
    this.incidenceStatusEditForm = this.fb.group({
      id: [item.id || ''],
      name: [item.name || '', Validators.required],
      description: [item.description || '']
    });
  }

  private populateIncidenceStatusList() {
    this.isService.getIncidenceStatusList(this.query)
      .subscribe(result => {
        this.incidenceStatusListOutput.emit(result);
      });
  }

  submit() {
    if (this.incidenceStatusEditForm.valid) {
      this.saveIncidenceStatusData = Object.assign({}, this.incidenceStatusEditForm.value);
      const result$ = (this.saveIncidenceStatusData.id) ? this.isService.updateIncidenceStatus(this.saveIncidenceStatusData)
       : this.isService.createIncidenceStatus(this.saveIncidenceStatusData);
        result$.subscribe(() => {
          this.toastr.success('Record saved successfully');
          this.populateIncidenceStatusList();
          this.activeModal.dismiss('Cross click')
        }, error => {
          // this.toastr.error('Error saving record');
          this.toastr.error(error.error.Error[0]);
        });
    }
  }
}
