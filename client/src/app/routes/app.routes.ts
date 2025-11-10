import { Routes } from '@angular/router';
import { unknownUserGuard } from '../guards/unknown-user.guard';
import { authenticatedUserGuard } from '../guards/authenticated-user.guard';
import { listTop10DoctorsResolver } from '../resolvers/doctor.resolvers';
import { listActivitiesResolver } from '../resolvers/activity.resolvers';

export const routes: Routes = [
  { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
  {
    path: 'auth',
    loadChildren: () => import('../routes/auth.routes').then((route) => route.authRoutes),
    canActivate: [unknownUserGuard],
  },
  {
    path: 'home',
    loadComponent: () =>
      import('../components/home/home.component').then((component) => component.Home),
    canActivate: [authenticatedUserGuard],
    resolve: { top10Doctors: listTop10DoctorsResolver, upcomingActivities: listActivitiesResolver },
  },
  {
    path: 'patients',
    loadChildren: () => import('../routes/patient.routes').then((route) => route.patientRoutes),
    canActivate: [authenticatedUserGuard],
  },
  {
    path: 'doctors',
    loadChildren: () => import('../routes/doctor.routes').then((route) => route.doctorRoutes),
    canActivate: [authenticatedUserGuard],
  },
  {
    path: 'activities',
    loadChildren: () => import('../routes/activity.routes').then((route) => route.activityRoutes),
    canActivate: [authenticatedUserGuard],
  },
];
