import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Photo } from 'app/_models/photo';
import { PhotoService } from 'app/shared/services/photo.service';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-video-editor',
  templateUrl: './video-editor.component.html',
  styleUrls: ['./video-editor.component.scss']
})
export class VideoEditorComponent implements OnInit {
  @Input() videos: any[];
  baseUrl = environment.apiUrl;
  imgUrl = environment.imgUrl;

  constructor(private photoService: PhotoService, private toastr: ToastrService) { }

  ngOnInit() {
    console.log(this.videos);
  }

  deletePhoto(id) {
    (Swal as any).fire({
      title: 'Are you sure',
      text: 'You want to delete this photo?',
      type: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes'
    }).then((result) => {
      if (result.value) {
        const result$ = this.photoService.deletePhotoVideo(id);
        result$.subscribe(() => {
          this.videos.splice(this.videos.findIndex(p => p.id === id), 1);
          (Swal as any).fire(
            'Deleted!',
            'The record has been deleted successfully',
            'success'
          )
        }, error => {
          // console.log(error);
          // console.log(error.error);
          this.toastr.error(error.error.Error[0]);
        });
      }
    });
  }

}
