import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';

import {AboutComponent} from "./components/about/about.component";
import {ImprintComponent} from "./components/imprint/imprint.component";
import {PrivacyPolicyComponent} from "./components/privacy-policy/privacy-policy.component";

const routes: Routes = [
  {path: 'about', component: AboutComponent},
  {path: 'imprint', component: ImprintComponent},
  {path: 'privacy-policy', component: PrivacyPolicyComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CommonUiRoutingModule {
}
