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
import { filter, map, Observer, shareReplay, switchMap, take, tap } from 'rxjs';
import { NotificationService } from '../../../services/notification.service';
import { PatientDto, IdApiResponse, PatientDetailsDto } from '../../../models/patient.models';
import { PatientService } from '../../../services/patient.service';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-edit-patient.component',
  imports: [
    AsyncPipe,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    RouterLink,
    ReactiveFormsModule,
  ],
  templateUrl: './edit-patient.component.html',
  styleUrl: './edit-patient.component.scss',
})
export class EditPatientComponent {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly route = inject(ActivatedRoute);
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

  protected readonly patient$ = this.route.data.pipe(
    filter((data) => data['patient'] as boolean),
    map((data) => data['patient'] as PatientDetailsDto),
    tap((patient) => this.formGroup.patchValue(patient)),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

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

  public edit(): void {
    if (this.formGroup.invalid) return;

    const editModel: PatientDto = this.formGroup.value as PatientDto;

    const editObserver: Observer<IdApiResponse> = {
      next: () =>
        this.notificationService.success(`Patient "${editModel.nome}" updated successfully!`, 'OK'),
      error: (err: string) => (console.log(err), this.notificationService.error(err, 'OK')),
      complete: () => void this.router.navigate(['/patients']),
    };

    this.patient$
      .pipe(
        take(1),
        switchMap((patient) => this.patientService.edit(patient.id, editModel)),
      )
      .subscribe(editObserver);
  }
}
