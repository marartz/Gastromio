import {Component, OnInit} from '@angular/core';
import {NavigationEnd, Router, RouterOutlet} from '@angular/router';
import {OrderHomeComponent} from './order-home/order-home.component';
import {Title} from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  routerOutlet: RouterOutlet;

  constructor(
    private titleService: Title,
    private router: Router
  ) {
    titleService.setTitle('Gastromio - Einfach. Lokal. Bestellen.');
  }

  ngOnInit() {
    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      window.scrollTo(0, 0);
    });
  }

  onActivate(component: any): void {
  }
}
