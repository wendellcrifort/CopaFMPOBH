import { Component } from '@angular/core';
import { AlertService } from 'src/services/alert.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'campeonato-ui';

  alert: any;

  constructor(private alertService: AlertService) { }

  ngOnInit(): void {
    this.alertService.getAlerts().subscribe(alert => {
      this.alert = alert;
      setTimeout(() => {
        this.alert = null;
      }, 5000);
    });
  } 
}
