import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-skills',
  templateUrl: './skills.component.html',
  styleUrls: ['./skills.component.scss']
})
export class SkillsComponent implements OnInit {
  
  // Programming Languages Polar Area Chart
  languagesPolarAreaChartData = {
    datasets: [{
        data: [10, 3, 1, 1, 3, 8, 3, 10, 9],

        label: 'Programming Languages'
    },],
    labels: ["C#", "Java", "Python", "Rust", "Dart", "JavaScript", "TypeScript", "HTML/CSS/SCSS", "SQL (MySQL/T-SQL/NpgSQL)"]
  };

  // Tools Polar Area Chart
  toolsPolarAreaChartData = {
    datasets: [{
        data: [5, 3, 1, 1, 3, 5, 2, 1, 2, 3, 4],

        label: 'Programming Tools and Technologies'
    },

    ],
    labels: ["Docker", "Jenkins", "Flyway", "Fly.io", "OAuth (any)", "Visio", "PlantUML", "RabbitMQ", "SQL Server Management Studio", "PgAdmin", "HeidiSQL"]
  };

  // Frameworks Polar Area Chart
  frameworksPolarAreaChartData = {
    datasets: [{
        data: [3, 1, 10, 2, 2, 6, 1, 3, 2, 1],

        label: 'Language Frameworks'
    },

    ],
    labels: ["Flutter", "Flask", "ASP.NET Core", "Angular", "AWT & Swing", "Razor", "Blazor", "Bootstrap", "TailwindCSS", "PrimeNG"]
  };

  polarAreaChartchartOptions = {
      plugins: {
          legend: {
              labels: {
                  color: '888888'
              }
          }
      },
      scales: {
          r: {
              grid: {
                  color: 'var(--ctaPrimary)'
              }
          }
      }
  }

  constructor() { }

  ngOnInit(): void {
  }

}
