import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, NavigationEnd, Router, RouterOutlet} from '@angular/router';
import {Title} from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  routerOutlet: RouterOutlet;

  constructor(
    private activatedRoute: ActivatedRoute,
    private titleService: Title,
    private router: Router
  ) {
  }

  ngOnInit() {
    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      this.titleService.setTitle('Gastromio &ndash; Essen lokal bestellen');
      window.scrollTo(0, 0);
    });
  }

  onActivate(component: any): void {
  }
}
