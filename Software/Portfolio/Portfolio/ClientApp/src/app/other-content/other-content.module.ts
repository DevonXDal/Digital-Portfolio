import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IntegratedProjectsComponent } from './integrated-projects/integrated-projects.component';
import { UpdateListComponent } from './update-list/update-list.component';



@NgModule({
  declarations: [
    IntegratedProjectsComponent,
    UpdateListComponent
  ],
  imports: [
    CommonModule
  ]
})
export class OtherContentModule { }
