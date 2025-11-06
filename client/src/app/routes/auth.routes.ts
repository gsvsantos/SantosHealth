import { Routes } from '@angular/router';
import { LoginComponent } from '../components/auth/login.component/login.component';
import { RegisterComponent } from '../components/auth/register.component/register.component';

export const authRoutes: Routes = [
  {
    path: '',
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
    ],
  },
];
