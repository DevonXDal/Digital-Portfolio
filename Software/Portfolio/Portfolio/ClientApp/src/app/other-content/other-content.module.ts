import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IntegratedProjectsComponent } from './integrated-projects/integrated-projects.component';
import { UpdateListComponent } from './update-list/update-list.component';
import { RouterModule } from '@angular/router';
import {DockModule} from 'primeng/dock';
import {MenubarModule} from 'primeng/menubar';

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
      { path: `${base}/integrated-projects`, component: IntegratedProjectsComponent},
      { path: `${base}/updates`, component: UpdateListComponent },
    ]),
    DockModule,
    MenubarModule
  ]
})
export class OtherContentModule { }
