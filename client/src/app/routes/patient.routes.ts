import { Routes } from '@angular/router';
import { listPatientsResolver, patientDetailsResolver } from '../resolvers/patient.resolvers';
import { ListPatientsComponent } from '../components/patients/list/list-patients.component/list-patients.component';
import { RegisterPatientsComponent } from '../components/patients/register/register-patients.component/register-patients.component';
import { EditPatientComponent } from '../components/patients/edit/edit-patient.component/edit-patient.component';
export const patientRoutes: Routes = [
  {
    path: '',
    component: ListPatientsComponent,
    resolve: { patients: listPatientsResolver },
  },
  { path: 'register', component: RegisterPatientsComponent },
  {
    path: 'edit/:id',
    component: EditPatientComponent,
    resolve: { patient: patientDetailsResolver },
  },
  //   {
  //     path: 'delete',
  //     component: DeletePatientComponent,
  //     resolve: { patient: patientDetailsResolver },
  //   },
];
