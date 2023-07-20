import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PreviousProjectsComponent } from './previous-projects.component';

describe('PreviousProjectsComponent', () => {
  let component: PreviousProjectsComponent;
  let fixture: ComponentFixture<PreviousProjectsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PreviousProjectsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PreviousProjectsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
