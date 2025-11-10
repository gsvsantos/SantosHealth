import { HttpErrorResponse } from '@angular/common/http';
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
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { filter, map, Observer } from 'rxjs';
import { ActivityDto } from '../../../models/activity.models';
import { IdApiResponse, Patient } from '../../../models/patient.models';
import { ActivityService } from '../../../services/activity.service';
import { NotificationService } from '../../../services/notification.service';
import { MatSelectModule } from '@angular/material/select';
import { AsyncPipe } from '@angular/common';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatRadioModule } from '@angular/material/radio';
import { Doctor } from '../../../models/doctor.models';
import { OwlDateTimeModule, OwlNativeDateTimeModule } from '@danielmoncada/angular-datetime-picker';
import { OverlayModule } from '@angular/cdk/overlay';

@Component({
  selector: 'app-register-activities.component',
  imports: [
    OverlayModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatRadioModule,
    OwlDateTimeModule,
    OwlNativeDateTimeModule,
    AsyncPipe,
    ReactiveFormsModule,
    RouterLink,
  ],
  providers: [provideNativeDateAdapter()],
  templateUrl: './register-activity.component.html',
  styleUrl: './register-activity.component.scss',
})
export class RegisterActivitiesComponent {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly activityService = inject(ActivityService);
  protected readonly notificationService = inject(NotificationService);

  protected readonly patients$ = this.route.data.pipe(
    filter((data) => data['patients'] as boolean),
    map((data) => data['patients'] as Patient[]),
  );

  protected readonly doctors$ = this.route.data.pipe(
    filter((data) => data['doctors'] as boolean),
    map((data) => data['doctors'] as Doctor[]),
  );

  protected formGroup: FormGroup = this.formBuilder.group({
    pacienteId: ['', [Validators.required.bind(this)]],
    inicio: [null, [Validators.required.bind(this)]],
    termino: [null, [Validators.required.bind(this)]],
    tipoAtividade: ['', [Validators.required.bind(this)]],
    medicos: [[], [Validators.required.bind(this)]],
  });

  public get pacienteId(): AbstractControl | null {
    return this.formGroup.get('pacienteId');
  }

  public get inicio(): AbstractControl | null {
    return this.formGroup.get('inicio');
  }

  public get termino(): AbstractControl | null {
    return this.formGroup.get('termino');
  }

  public get tipoAtividade(): AbstractControl | null {
    return this.formGroup.get('tipoAtividade');
  }

  public get medicos(): AbstractControl | null {
    return this.formGroup.get('medicos');
  }

  public register(): void {
    if (this.formGroup.invalid) return;

    const medicosValor = this.medicos?.value as string | string[] | null | undefined;

    const medicosArray: string[] = Array.isArray(medicosValor)
      ? medicosValor
      : medicosValor
        ? [medicosValor]
        : [];

    const registerModel: ActivityDto = {
      ...(this.formGroup.value as ActivityDto),
      medicos: medicosArray,
    };

    const registerObserver: Observer<IdApiResponse> = {
      next: () => this.notificationService.success(`Activity registered successfully!`, 'OK'),
      error: (err: HttpErrorResponse) =>
        this.notificationService.error(err.error.erros as string, 'OK'),
      complete: () => void this.router.navigate(['/activities']),
    };

    this.activityService.register(registerModel).subscribe(registerObserver);
  }
}
