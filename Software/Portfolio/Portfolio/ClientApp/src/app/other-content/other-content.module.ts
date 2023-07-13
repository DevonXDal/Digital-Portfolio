import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IntegratedProjectsComponent } from './integrated-projects/integrated-projects.component';
import { UpdateListComponent } from './update-list/update-list.component';
import { RouterModule } from '@angular/router';

let base = 'other-content';

@NgModule({
  declarations: [
    IntegratedProjectsComponent,
    UpdateListComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: `${base}`, redirectTo: `${base}/updates`, pathMatch: 'full' },
      { path: `${base}/integrated-projects`, component: IntegratedProjectsComponent, pathMatch: 'full' },
      { path: `${base}/updates`, component: UpdateListComponent },
    ]),
  ]
})
export class OtherContentModule { }
