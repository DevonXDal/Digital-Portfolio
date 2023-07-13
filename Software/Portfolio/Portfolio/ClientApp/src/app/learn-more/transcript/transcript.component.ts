import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-transcript',
  templateUrl: './transcript.component.html',
  styleUrls: ['./transcript.component.scss']
})
export class TranscriptComponent implements OnInit {

  public transcriptPath = '/assets/documents/Transcript_Dalrymple.pdf';

  constructor() { }

  ngOnInit(): void {
  }

}
