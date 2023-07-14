import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowcaseAccordionComponent } from './showcase-accordion.component';

describe('ShowcaseAccordionComponent', () => {
  let component: ShowcaseAccordionComponent;
  let fixture: ComponentFixture<ShowcaseAccordionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowcaseAccordionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShowcaseAccordionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
