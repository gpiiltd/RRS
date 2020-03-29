import { Injectable } from '@angular/core';
import { Subject } from "rxjs/Subject";
import { BrowserXhr } from "@angular/http";

@Injectable()
export class ProgressService {
    //Subject is a type of Observable that allows pushing of values into that Observable
  private uploadProgress: Subject<any>;

  startTracking() { 
    this.uploadProgress = new Subject();
    return this.uploadProgress;
  }

  notify(progress) {
    // push progress into the Subject Observable i.e this.service.uploadProgress.next(progress)
    // when we subscribe to uploadProgress Observable in our component, everytime there is a new value, we get that progress object
    if (this.uploadProgress)
      this.uploadProgress.next(progress);
  }

  endTracking() {
    if (this.uploadProgress)
      this.uploadProgress.complete();
  }
}

@Injectable()
export class BrowserXhrWithProgress extends BrowserXhr {
    // Angular uses class BrowserXhr which uses XMLHttpRequest internally

  constructor(private service: ProgressService) { super(); }

  build(): XMLHttpRequest {
    // call build method of the base class
    var xhr: XMLHttpRequest = super.build();
    
    // subscribe to the onProgress event of the xhr object before returning the object
    // set the onProgress event to a function that takes the event object
    // here we handling the onprogress event of the XMLHttpRequest object
    xhr.upload.onprogress = (event) => {
      this.service.notify(this.createProgress(event));
    };

    xhr.upload.onloadend = () => {
      this.service.endTracking();
    }
    
    return xhr; 
  }

  private createProgress(event) {
    return {
        total: event.total,
        percentage: Math.round(event.loaded / event.total * 100)
    };
  }
}