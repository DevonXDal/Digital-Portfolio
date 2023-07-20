import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrossDeviceShowcaseComponent } from './cross-device-showcase.component';

describe('CrossDeviceShowcaseComponent', () => {
  let component: CrossDeviceShowcaseComponent;
  let fixture: ComponentFixture<CrossDeviceShowcaseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CrossDeviceShowcaseComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CrossDeviceShowcaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
