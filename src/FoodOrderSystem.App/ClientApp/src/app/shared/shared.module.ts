import {NgModule} from '@angular/core';
import {CommonModule} from "@angular/common";
import {RouterModule} from "@angular/router";

import {HttpErrorHandlingService} from './services/http-error-handling.service';
import {ClientPaginationComponent} from './components/pagination/client-pagination.component';
import {ServerPaginationComponent} from './components/pagination/server-pagination.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule
  ],
  declarations: [
    ClientPaginationComponent,
    ServerPaginationComponent
  ],
  providers: [
    HttpErrorHandlingService
  ],
  exports: [
    ClientPaginationComponent,
    ServerPaginationComponent
  ]
})
export class SharedModule {
}
