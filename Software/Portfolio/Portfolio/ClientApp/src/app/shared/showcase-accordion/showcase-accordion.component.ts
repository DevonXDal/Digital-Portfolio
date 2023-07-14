import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'showcase-accordion',
  templateUrl: './showcase-accordion.component.html',
  styleUrls: ['./showcase-accordion.component.scss']
})
export class ShowcaseAccordionComponent implements OnInit {
  
  @Input() public showcaseName: string = 'MISSING';
  @Input() public showcaseImgPath: string = 'MISSING'
  @Input() public description: string = 'MISSING';
  @Input() public hasExpired: boolean = false;
  @Input() public hasVerification: boolean = false;
  @Input() public verificationLink?: string;
  @Input() public verificationCode?: string; 

  constructor() { }

  ngOnInit(): void {
  }

}
