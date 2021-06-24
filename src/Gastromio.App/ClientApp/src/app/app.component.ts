import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, NavigationEnd, Router} from '@angular/router';
import {Title} from '@angular/platform-browser';
import AOS from 'aos'
import { MetaDataService } from './shared/services/meta-data.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private metaDataService: MetaDataService
  ) {
  }

  ngOnInit() {
    
    AOS.init();

    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      this.metaDataService.setTitle('Gastromio – Essen lokal bestellen');
      window.scrollTo(0, 0);
    });
  }
}