import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IntegratedProjectsComponent } from './integrated-projects.component';

describe('IntegratedProjectsComponent', () => {
  let component: IntegratedProjectsComponent;
  let fixture: ComponentFixture<IntegratedProjectsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IntegratedProjectsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IntegratedProjectsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
