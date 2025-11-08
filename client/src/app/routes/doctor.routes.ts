import { Routes } from '@angular/router';
import { doctorDetailsResolver, listDoctorsResolver } from '../resolvers/doctor.resolvers';
import { ListDoctorsComponent } from '../components/doctors/list/list-doctors.component';
import { RegisterDoctorsComponent } from '../components/doctors/register/register-doctor.component';
import { EditDoctorComponent } from '../components/doctors/edit/edit-doctor.component';
import { DeleteDoctorComponent } from '../components/doctors/delete/delete-doctor.component';

export const doctorRoutes: Routes = [
  {
    path: '',
    component: ListDoctorsComponent,
    resolve: { doctors: listDoctorsResolver },
  },
  { path: 'register', component: RegisterDoctorsComponent },
  {
    path: 'edit/:id',
    component: EditDoctorComponent,
    resolve: { doctor: doctorDetailsResolver },
  },
  {
    path: 'delete/:id',
    component: DeleteDoctorComponent,
    resolve: { doctor: doctorDetailsResolver },
  },
];
