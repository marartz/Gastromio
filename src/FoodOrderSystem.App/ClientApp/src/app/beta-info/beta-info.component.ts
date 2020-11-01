import {Component, OnInit} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-beta-info',
  templateUrl: './beta-info.component.html',
  styleUrls: [
    './beta-info.component.css',
    '../../assets/css/frontend_v3.min.css'
  ]
})
export class BetaInfoComponent implements OnInit {
  constructor(
    public activeModal: NgbActiveModal,
  ) {
  }

  ngOnInit() {
  }
}
