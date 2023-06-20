import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ResumeComponent } from './resume/resume.component';
import { TranscriptComponent } from './transcript/transcript.component';
import { CertificationListComponent } from './certification-list/certification-list.component';
import { OtherCredentialsListComponent } from './other-credentials-list/other-credentials-list.component';



@NgModule({
  declarations: [
    ResumeComponent,
    TranscriptComponent,
    CertificationListComponent,
    OtherCredentialsListComponent
  ],
  imports: [
    CommonModule
  ]
})
export class LearnMoreModule { }
