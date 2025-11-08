import { Routes } from '@angular/router';
import { listDoctorsResolver } from '../resolvers/doctor.resolvers';
import { ListDoctorsComponent } from '../components/doctors/list/list-doctors.component';
import { RegisterDoctorsComponent } from '../components/doctors/register/register-doctor.component';

export const doctorRoutes: Routes = [
  {
    path: '',
    component: ListDoctorsComponent,
    resolve: { doctors: listDoctorsResolver },
  },
  { path: 'register', component: RegisterDoctorsComponent },
  //   {
  //     path: 'edit/:id',
  //     component: EditDoctorComponent,
  //     resolve: { doctor: doctorDetailsResolver },
  //   },
  //   {
  //     path: 'delete/:id',
  //     component: DeleteDoctorComponent,
  //     resolve: { doctor: doctorDetailsResolver },
  //   },
];
