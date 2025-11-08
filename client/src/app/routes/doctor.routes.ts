import { Routes } from '@angular/router';
import { ListDoctorsComponent } from '../components/doctors/list/list-doctors.component/list-doctors.component';
import { listDoctorsResolver } from '../resolvers/doctor.resolvers';

export const doctorRoutes: Routes = [
  {
    path: '',
    component: ListDoctorsComponent,
    resolve: { doctors: listDoctorsResolver },
  },
  //   { path: 'register', component: RegisterDoctorsComponent },
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
