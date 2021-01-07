import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';

import {Observable} from "rxjs";

import {RestaurantAdminFacade} from "../../restaurant-admin.facade";

@Component({
  selector: 'app-image-settings',
  templateUrl: './image-settings.component.html',
  styleUrls: [
    './image-settings.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/backend_v2.min.css'
  ]
})
export class ImageSettingsComponent implements OnInit {

  hasLogo$: Observable<boolean>;
  logoUrl$: Observable<string>;
  @ViewChild('logo') logoElement: ElementRef;

  hasBanner$: Observable<boolean>;
  bannerUrl$: Observable<string>;
  @ViewChild('banner') bannerElement: ElementRef;

  constructor(
    private facade: RestaurantAdminFacade
  ) { }

  ngOnInit(): void {
    this.hasLogo$ = this.facade.getHasLogo$();
    this.logoUrl$ = this.facade.getLogoUrl$();

    this.hasBanner$ = this.facade.getHasBanner$();
    this.bannerUrl$ = this.facade.getBannerUrl$();
  }

  onChangeLogo(event: any): void {
    if (!event.target.files || !event.target.files.length) {
      return;
    }
    const reader = new FileReader();
    const [file] = event.target.files;
    reader.onload = () => {
      this.facade.changeLogo(reader.result as string);
      this.logoElement.nativeElement.value = "";
    };
    reader.readAsDataURL(file);
  }

  onRemoveLogo(): void {
    if (!confirm('Soll das Logo wirklich entfernt werden?')) {
      return;
    }
    this.facade.removeLogo();
  }

  onChangeBanner(event: any): void {
    if (!event.target.files || !event.target.files.length) {
      return;
    }
    const reader = new FileReader();
    const [file] = event.target.files;
    reader.onload = () => {
      this.facade.changeBanner(reader.result as string);
      this.bannerElement.nativeElement.value = "";
    };
    reader.readAsDataURL(file);
  }

  onRemoveBanner(): void {
    if (!confirm('Soll das Banner wirklich entfernt werden?')) {
      return;
    }
    this.facade.removeBanner();
  }

}
