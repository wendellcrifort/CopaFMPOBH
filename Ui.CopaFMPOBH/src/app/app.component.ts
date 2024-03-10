import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { AlertService } from 'src/services/alert.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'campeonato-ui';

  alert: any;

  constructor(private router: Router, private alertService: AlertService) { }

  ngOnInit(): void {
    this.alertService.getAlerts().subscribe(alert => {
      this.alert = alert;
      setTimeout(() => {
        this.alert = null;
      }, 5000);
    });

    this.router.events.pipe(
      filter((event: any) => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      this.updateActiveMenuItem(event.url);
    });
  } 

  updateActiveMenuItem(url: string): void {
    const menuItems = document.querySelectorAll('.main-menu.nav li');
    menuItems.forEach((item: Element) => {
      const menuItem = item as HTMLElement;
      menuItem.classList.remove('active');
      if (url.includes(menuItem.querySelector('a')?.getAttribute('routerLink') || '')) {
        menuItem.classList.add('active');
      }
    });
  }
}
