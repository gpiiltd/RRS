/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { HazardListComponent } from './hazard-list.component';

describe('HazardListComponent', () => {
  let component: HazardListComponent;
  let fixture: ComponentFixture<HazardListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HazardListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HazardListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
