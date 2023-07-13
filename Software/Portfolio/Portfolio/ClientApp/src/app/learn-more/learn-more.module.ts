import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ResumeComponent } from './resume/resume.component';
import { TranscriptComponent } from './transcript/transcript.component';
import { CertificationListComponent } from './certification-list/certification-list.component';
import { OtherCredentialsListComponent } from './other-credentials-list/other-credentials-list.component';
import { RouterModule } from '@angular/router';

let base = 'learn-more';

@NgModule({
  declarations: [
    ResumeComponent,
    TranscriptComponent,
    CertificationListComponent,
    OtherCredentialsListComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: `${base}`, redirectTo: `${base}/resume`, pathMatch: 'full' },
      { path: `${base}/resume`, component: ResumeComponent, pathMatch: 'full' },
      { path: `${base}/transcript`, component: TranscriptComponent },
      { path: `${base}/skills`, component: ResumeComponent },
      { path: `${base}/certifications`, component: CertificationListComponent },
      { path: `${base}/other-credentials`, component: OtherCredentialsListComponent },
    ]),
  ]
})
export class LearnMoreModule { }
