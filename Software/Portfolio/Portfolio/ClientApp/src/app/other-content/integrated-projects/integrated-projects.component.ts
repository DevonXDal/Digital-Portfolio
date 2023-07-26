import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-integrated-projects',
  templateUrl: './integrated-projects.component.html',
  styleUrls: ['./integrated-projects.component.scss']
})
export class IntegratedProjectsComponent implements OnInit {
  dockItems: MenuItem[] = [];
  currentTimestamp: number;

  menubarItems: any[] = [];

  constructor() {
    this.currentTimestamp = Date.now();

    setInterval(() => {
      this.currentTimestamp = Date.now();
    }, 5000)
  }

  ngOnInit(): void {

    this.dockItems = [
      {
        label: 'Chesshogi - Planned', // Public Domain Icon
        icon: 'assets/svg/ChesshogiIcon.svg',
        tooltipOptions: {
          tooltipLabel: "Chesshogi - Planned",
          tooltipPosition: 'top',
          positionTop: -15,
          positionLeft: 15
        }
      },
      {
        label: 'Labyrinth of Devon 3.0 - Planned', // Public Domain Icon
        icon: 'assets/svg/LabyrinthOfDevonIcon.svg',
        tooltipOptions: {
          tooltipLabel: "Labyrinth of Devon 3.0 - Planned",
          tooltipPosition: 'top',
          positionTop: -15,
          positionLeft: 15
        }
      },
      {
        label: 'Script to Dart Transpiler - Planned', // MIT License Icon
        icon: 'assets/svg/ScrToDartIcon.svg',
        tooltipOptions: {
          tooltipLabel: "Script to Dart Transpiler - Planned",
          tooltipPosition: 'top',
          positionTop: -15,
          positionLeft: 15
        }
      }
    ];

    this.menubarItems = [
      {
          label: 'Integrated ProjectOS',
          className: 'menubar-root'
      },
  ];
  }

}
