import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { AlertService } from 'src/services/alert.service';
import { SignalRService } from '../services/signalr.service ';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'campeonato-ui';

  alert: any;

    constructor(private router: Router, private alertService: AlertService, private signalRService: SignalRService) { }

  ngOnInit(): void {
    this.alertService.getAlerts().subscribe(alert => {
      this.alert = alert;
      setTimeout(() => {
        this.alert = null;
      }, 5000);
    });

    this.signalRService.iniciarConexao();

    this.router.events.pipe(
      filter((event: any) => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      this.updateActiveMenuItem(event.url);
    });
  } 

    updateActiveMenuItem(url: string): void {
        const menuItems = document.querySelectorAll('.main-menu.nav > li');

        menuItems.forEach((item: Element) => {
            const menuItem = item as HTMLElement;
            menuItem.classList.remove('active'); // Remove the active class

            // Get the routerLink attribute from the current item
            const routerLink = menuItem.querySelector('a')?.getAttribute('routerLink');

            if (routerLink) {
                // If the routerLink exists, check if it matches the URL
                if (url.includes(routerLink)) {
                    menuItem.classList.add('active');
                }
            } else {
                // If the routerLink does not exist (like "Destaques"), check its children
                const childRouterLinks = menuItem.querySelectorAll('ul li a[routerLink]');
                childRouterLinks.forEach((childLink) => {
                    const childRouterLink = childLink.getAttribute('routerLink');
                    if (childRouterLink && url.includes(childRouterLink)) {
                        menuItem.classList.add('active');
                    }
                });
            }
        });
    }
}
