import { AsyncPipe } from '@angular/common';
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
import { filter, map, Observer, shareReplay, switchMap, take, tap } from 'rxjs';
import { Doctor } from '../../../models/doctor.models';
import { ActivityService } from '../../../services/activity.service';
import { NotificationService } from '../../../services/notification.service';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatTimepickerModule } from '@angular/material/timepicker';
import { Activity, EditActivityDto } from '../../../models/activity.models';
import { HttpErrorResponse } from '@angular/common/http';
import { IdApiResponse } from '../../../models/patient.models';
import { provideNativeDateAdapter } from '@angular/material/core';
import { OwlDateTimeModule, OwlNativeDateTimeModule } from '@danielmoncada/angular-datetime-picker';

@Component({
  selector: 'app-edit-activity.component',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatTimepickerModule,
    MatDatepickerModule,
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
  templateUrl: './edit-activity.component.html',
  styleUrl: './edit-activity.component.scss',
})
export class EditActivityComponent {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly activityService = inject(ActivityService);
  protected readonly notificationService = inject(NotificationService);

  protected formGroup: FormGroup = this.formBuilder.group({
    inicio: [null, [Validators.required.bind(this)]],
    termino: [null, [Validators.required.bind(this)]],
    tipoAtividade: ['', [Validators.required.bind(this)]],
    medicos: [[], [Validators.required.bind(this)]],
  });

  protected readonly doctors$ = this.route.data.pipe(
    filter((data) => data['doctors'] as boolean),
    map((data) => data['doctors'] as Doctor[]),
  );

  protected readonly activity$ = this.route.data.pipe(
    filter((data) => data['activity'] as boolean),
    map((data) => data['activity'] as Activity),
    tap((activity) => {
      const medicoIds = activity.medicos?.map((medico) => medico.id) ?? [];
      if (activity.tipoAtividade === 'Consulta') {
        this.formGroup.patchValue({ ...activity, medicos: medicoIds[0] ?? null });
      } else {
        this.formGroup.patchValue({ ...activity, medicos: medicoIds });
      }
    }),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

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

  public edit(): void {
    if (this.formGroup.invalid) return;

    const medicosValor = this.medicos?.value as string | string[] | null | undefined;

    const medicosArray: string[] = Array.isArray(medicosValor)
      ? medicosValor
      : medicosValor
        ? [medicosValor]
        : [];

    const editModel: EditActivityDto = {
      ...(this.formGroup.value as EditActivityDto),
      medicos: medicosArray,
    };

    const editObserver: Observer<IdApiResponse> = {
      next: () => this.notificationService.success(`Activity updated successfully!`, 'OK'),
      error: (err: string) => this.notificationService.error(err, 'OK'),
      complete: () => void this.router.navigate(['/activities']),
    };

    this.activity$
      .pipe(
        take(1),
        switchMap((doctor) => this.activityService.edit(doctor.id, editModel)),
      )
      .subscribe(editObserver);
  }
}
