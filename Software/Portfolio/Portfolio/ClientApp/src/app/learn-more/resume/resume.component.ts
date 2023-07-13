import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-resume',
  templateUrl: './resume.component.html',
  styleUrls: ['./resume.component.scss']
})
export class ResumeComponent implements OnInit {
  public resumePath = '/assets/documents/Resume_Dalrymple.pdf';

  constructor() { }

  ngOnInit(): void {
  }

}
