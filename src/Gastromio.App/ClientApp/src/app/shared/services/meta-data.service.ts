import { Injectable } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, ActivatedRouteSnapshot, NavigationEnd, Router } from '@angular/router';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root'
})
export class MetaDataService {
  
  public canonicalBaseUrl: string = environment.baseUrl;
  public canonicalDomElement: HTMLLinkElement = document.querySelector(`link[rel='canonical']`);

  constructor(private router: Router, private titleService: Title) {
    if (this.canonicalDomElement == null) {
      this.createCanonicalDomElement();
    } 
    router.events.subscribe((val) => {
      if (val instanceof NavigationEnd) {
        const canonicalUrl = this.router.url === '/' ? this.canonicalBaseUrl : this.canonicalBaseUrl + this.router.url.split('?')[0];
        this.updateAlternateUrls(canonicalUrl);
        this.updateCanonicalUrl(canonicalUrl);
        this.updateMetaUrls(canonicalUrl);
      }
    });
  }

  public setTitle(newTitle: string): void {
    this.titleService.setTitle(newTitle);
    
    const titlesToEdit = [
      'og:title',
      'twitter:title',
      'dc.title'
    ];
    titlesToEdit.forEach(titleToEdit => {
      const elem = document.querySelector(`meta[name='${ titleToEdit }'], meta[property='${ titleToEdit }']`);
      elem.setAttribute('content', newTitle);
    });
  }

  private updateAlternateUrls(url: string): void {
    document.querySelectorAll(`link[rel='alternate']`).forEach(lang => {
      lang.setAttribute('href', url);
    });
  }

  private updateMetaUrls(url: string): void {
    const metaSelectors = [
      'og:url',
      'dc.source'
    ]
    metaSelectors.forEach(meta => {
      const elem = document.querySelector(`meta[name='${ meta }'], meta[property='${ meta }']`);
      elem.setAttribute('content', url);
    });
  }

  private updateCanonicalUrl(url: string): void {
    this.canonicalDomElement.setAttribute('href', url);
  }

  private createCanonicalDomElement(): void {
    this.canonicalDomElement = document.createElement('link') as HTMLLinkElement;
    this.canonicalDomElement.setAttribute('rel','canonical')
    document.getElementsByTagName('head')[0].appendChild(this.canonicalDomElement);
  }
}
