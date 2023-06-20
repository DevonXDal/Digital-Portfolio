import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtherCredentialsListComponent } from './other-credentials-list.component';

describe('OtherCredentialsListComponent', () => {
  let component: OtherCredentialsListComponent;
  let fixture: ComponentFixture<OtherCredentialsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OtherCredentialsListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OtherCredentialsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
