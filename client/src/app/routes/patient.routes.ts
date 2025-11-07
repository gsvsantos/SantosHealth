import { Routes } from '@angular/router';
import { listPatientsResolver } from '../resolvers/patient.resolvers';
import { ListPatientsComponent } from '../components/patients/list/list-patients.component/list-patients.component';
export const patientRoutes: Routes = [
  {
    path: '',
    component: ListPatientsComponent,
    resolve: { patients: listPatientsResolver },
  },
  //   { path: 'register', component: RegisterPatientComponent },
  //   { path: 'edit', component: EditPatientComponent, resolve: { patient: patientDetailsResolver } },
  //   {
  //     path: 'delete',
  //     component: DeletePatientComponent,
  //     resolve: { patient: patientDetailsResolver },
  //   },
];
