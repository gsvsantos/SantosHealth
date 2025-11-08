import { PatientService } from './../../../../services/patient.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
  AbstractControl,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { RouterLink, Router } from '@angular/router';
import { Observer } from 'rxjs';
import { NotificationService } from '../../../../services/notification.service';
import { IdApiResponse, PatientDto } from '../../../../models/patient.models';

@Component({
  selector: 'app-register-patients.component',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    RouterLink,
    ReactiveFormsModule,
  ],
  templateUrl: './register-patients.component.html',
  styleUrl: './register-patients.component.scss',
})
export class RegisterPatientsComponent {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly patientService = inject(PatientService);
  protected readonly notificationService = inject(NotificationService);

  protected formGroup: FormGroup = this.formBuilder.group({
    nome: ['', [Validators.required.bind(this), Validators.minLength(3)]],
    cpf: ['', [Validators.required.bind(this), Validators.pattern(/^\d{3}\.\d{3}\.\d{3}-\d{2}$/)]],
    email: ['', [Validators.required.bind(this), Validators.email.bind(this)]],
    telefone: [
      '',
      [Validators.required.bind(this), Validators.pattern(/^\(\d{2}\)\s\d{4,5}-\d{4}$/)],
    ],
  });

  public get nome(): AbstractControl | null {
    return this.formGroup.get('nome');
  }

  public get cpf(): AbstractControl | null {
    return this.formGroup.get('cpf');
  }

  public get email(): AbstractControl | null {
    return this.formGroup.get('email');
  }

  public get telefone(): AbstractControl | null {
    return this.formGroup.get('telefone');
  }

  public register(): void {
    if (this.formGroup.invalid) return;

    const registerModel: PatientDto = this.formGroup.value as PatientDto;

    const cadastroObserver: Observer<IdApiResponse> = {
      next: () =>
        this.notificationService.success(
          `Patient "${registerModel.nome}" registered successfully!`,
          'OK',
        ),
      error: (err: HttpErrorResponse) =>
        this.notificationService.error(err.error.erros as string, 'OK'),
      complete: () => void this.router.navigate(['/patients']),
    };

    this.patientService.register(registerModel).subscribe(cadastroObserver);
  }
}
