import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShowcaseAccordionComponent } from './showcase-accordion/showcase-accordion.component';
import {AccordionModule} from 'primeng/accordion';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


@NgModule({
  declarations: [
    ShowcaseAccordionComponent
  ],
  imports: [
    CommonModule,
    AccordionModule,
    BrowserAnimationsModule
  ],
  exports: [
    CommonModule,
    ShowcaseAccordionComponent
  ]
})
export class SharedModule { }
