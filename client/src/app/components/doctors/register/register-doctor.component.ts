import { DoctorService } from '../../../services/doctor.service';
import { Component, inject } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterLink } from '@angular/router';
import { NotificationService } from '../../../services/notification.service';
import { DoctorDto } from '../../../models/doctor.models';
import { HttpErrorResponse } from '@angular/common/http';
import { Observer } from 'rxjs';
import { IdApiResponse } from '../../../models/patient.models';

@Component({
  selector: 'app-register-doctors.component',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    RouterLink,
    ReactiveFormsModule,
  ],
  templateUrl: './register-doctor.component.html',
  styleUrl: './register-doctor.component.scss',
})
export class RegisterDoctorsComponent {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly doctorService = inject(DoctorService);
  protected readonly notificationService = inject(NotificationService);

  protected formGroup: FormGroup = this.formBuilder.group({
    nome: ['', [Validators.required.bind(this), Validators.minLength(3)]],
    crm: ['', [Validators.required.bind(this), Validators.pattern(/^\d{5}-[A-Z]{2}$/)]],
  });

  public get nome(): AbstractControl | null {
    return this.formGroup.get('nome');
  }

  public get crm(): AbstractControl | null {
    return this.formGroup.get('crm');
  }

  public register(): void {
    if (this.formGroup.invalid) return;

    const registerModel: DoctorDto = this.formGroup.value as DoctorDto;

    const registerObserver: Observer<IdApiResponse> = {
      next: () =>
        this.notificationService.success(
          `Doctor "${registerModel.nome}" registered successfully!`,
          'OK',
        ),
      error: (err: HttpErrorResponse) =>
        this.notificationService.error(err.error.erros[0] as string, 'OK'),
      complete: () => void this.router.navigate(['/doctors']),
    };

    this.doctorService.register(registerModel).subscribe(registerObserver);
  }
}
