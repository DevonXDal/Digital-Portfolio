import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IntegratedProjectsComponent } from './integrated-projects/integrated-projects.component';
import { UpdateListComponent } from './update-list/update-list.component';
import { RouterModule } from '@angular/router';
import { PreviousProjectsComponent } from './previous-projects/previous-projects.component';

let base = 'other-content';

@NgModule({
  declarations: [
    IntegratedProjectsComponent,
    UpdateListComponent,
    PreviousProjectsComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: `${base}`, redirectTo: `${base}/updates`, pathMatch: 'full' },
      { path: `${base}/integrated-projects`, component: IntegratedProjectsComponent},
      { path: `${base}/previous-projects`, component: PreviousProjectsComponent},
      { path: `${base}/updates`, component: UpdateListComponent },
    ]),
  ]
})
export class OtherContentModule { }
