import { Routes } from '@angular/router';
import { WandersteinOverviewPage, WandersteinDetailPage } from './modules/hiking-stones';

export const routes: Routes = [
  { path: '', component: WandersteinOverviewPage },
  { path: 'wandersteine', component: WandersteinOverviewPage },
  { path: 'wandersteine/:uniqueId', component: WandersteinDetailPage },
  { path: '**', redirectTo: '' }
];
