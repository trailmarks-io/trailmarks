import { Routes } from '@angular/router';
import { WandersteinOverviewPage } from './modules/hiking-stones';

export const routes: Routes = [
  { path: '', component: WandersteinOverviewPage },
  { path: 'wandersteine', component: WandersteinOverviewPage },
  { path: '**', redirectTo: '' }
];
