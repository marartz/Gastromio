import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-general-error',
  template: '<p class="error" *ngIf="generalError">{{generalError}}</p>',
  styles: [`
    .error {
      color: #E64135;
      font-family: proxima-nova, sans-serif;
      font-size: 0.75rem;
      font-weight: 400;
      margin: 0;
      letter-spacing: 0;
      line-height: 1.375em;
      transition: none;
      white-space: pre-wrap;
    }
  `]
})

export class GeneralErrorComponent implements OnInit {
  @Input() public generalError: string;

  ngOnInit() {
  }
}
