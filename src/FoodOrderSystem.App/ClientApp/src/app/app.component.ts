import {Component} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {OrderHomeComponent} from './order-home/order-home.component';
import {Title} from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  routerOutlet: RouterOutlet;

  constructor(
    private titleService: Title
  ) {
    titleService.setTitle('Gastromio - Einfach. Lokal. Bestellen.');
  }

  onActivate(component: any): void {
  }
}