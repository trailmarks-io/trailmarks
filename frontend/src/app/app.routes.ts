import { Routes } from '@angular/router';
import { WandersteinOverviewComponent } from './components/wanderstein-overview/wanderstein-overview';

export const routes: Routes = [
  { path: '', component: WandersteinOverviewComponent },
  { path: 'wandersteine', component: WandersteinOverviewComponent },
  { path: '**', redirectTo: '' }
];
