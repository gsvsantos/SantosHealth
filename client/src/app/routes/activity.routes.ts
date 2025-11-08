import { Routes } from '@angular/router';
import { ListActivitiesComponent } from '../components/activities/list/list-activities.component';
import { listActivitiesResolver } from '../resolvers/activity.resolvers';

export const activityRoutes: Routes = [
  {
    path: '',
    component: ListActivitiesComponent,
    resolve: { activities: listActivitiesResolver },
    runGuardsAndResolvers: 'paramsOrQueryParamsChange',
  },
  //   { path: 'register', component: RegisterActivitiesComponent },
  //   {
  //     path: 'edit/:id',
  //     component: EditActivityComponent,
  //     resolve: { activity: activityDetailsResolver },
  //   },
  //   {
  //     path: 'delete/:id',
  //     component: DeleteActivityComponent,
  //     resolve: { activity: activityDetailsResolver },
  //   },
];
