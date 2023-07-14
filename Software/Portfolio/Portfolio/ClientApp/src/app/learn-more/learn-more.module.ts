import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ResumeComponent } from './resume/resume.component';
import { TranscriptComponent } from './transcript/transcript.component';
import { CertificationListComponent } from './certification-list/certification-list.component';
import { OtherCredentialsListComponent } from './other-credentials-list/other-credentials-list.component';
import { RouterModule } from '@angular/router';
import { SkillsComponent } from './skills/skills.component';
import { ChartModule } from 'primeng/chart';
import { SharedModule } from '../shared/shared.module';

let base = 'learn-more';

@NgModule({
  declarations: [
    ResumeComponent,
    TranscriptComponent,
    CertificationListComponent,
    OtherCredentialsListComponent,
    SkillsComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild([
      { path: `${base}`, redirectTo: `${base}/resume`, pathMatch: 'full' },
      { path: `${base}/resume`, component: ResumeComponent, pathMatch: 'full' },
      { path: `${base}/transcript`, component: TranscriptComponent },
      { path: `${base}/skills`, component: SkillsComponent },
      { path: `${base}/certifications`, component: CertificationListComponent },
      { path: `${base}/other-credentials`, component: OtherCredentialsListComponent },
    ]),
    ChartModule
  ]
})
export class LearnMoreModule { }
