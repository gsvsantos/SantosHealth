import { Routes } from '@angular/router';
import { ListActivitiesComponent } from '../components/activities/list/list-activities.component';
import { listActivitiesResolver } from '../resolvers/activity.resolvers';
import { RegisterActivitiesComponent } from '../components/activities/register/register-activities.component';
import { listPatientsResolver } from '../resolvers/patient.resolvers';
import { listDoctorsResolver } from '../resolvers/doctor.resolvers';

export const activityRoutes: Routes = [
  {
    path: '',
    component: ListActivitiesComponent,
    resolve: { activities: listActivitiesResolver },
    runGuardsAndResolvers: 'paramsOrQueryParamsChange',
  },
  {
    path: 'register',
    component: RegisterActivitiesComponent,
    resolve: { patients: listPatientsResolver, doctors: listDoctorsResolver },
  },
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
