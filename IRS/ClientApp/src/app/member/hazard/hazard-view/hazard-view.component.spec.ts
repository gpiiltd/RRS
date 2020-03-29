/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { HazardViewComponent } from './hazard-view.component';

describe('HazardViewComponent', () => {
  let component: HazardViewComponent;
  let fixture: ComponentFixture<HazardViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HazardViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HazardViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
