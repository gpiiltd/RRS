/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { HazardEditComponent } from './hazard-edit.component';

describe('HazardEditComponent', () => {
  let component: HazardEditComponent;
  let fixture: ComponentFixture<HazardEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HazardEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HazardEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
