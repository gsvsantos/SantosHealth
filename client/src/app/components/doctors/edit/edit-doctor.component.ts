import { AsyncPipe } from '@angular/common';
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
import { Doctor, DoctorDto } from '../../../models/doctor.models';
import { IdApiResponse, PatientDetailsDto } from '../../../models/patient.models';
import { DoctorService } from '../../../services/doctor.service';
import { NotificationService } from '../../../services/notification.service';

@Component({
  selector: 'app-edit-doctor.component',
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
  templateUrl: './edit-doctor.component.html',
  styleUrl: './edit-doctor.component.scss',
})
export class EditDoctorComponent {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly doctorService = inject(DoctorService);
  protected readonly notificationService = inject(NotificationService);

  protected formGroup: FormGroup = this.formBuilder.group({
    nome: ['', [Validators.required.bind(this), Validators.minLength(3)]],
    crm: ['', [Validators.required.bind(this), Validators.pattern(/^\d{5}-[A-Z]{2}$/)]],
  });

  protected readonly doctor$ = this.route.data.pipe(
    filter((data) => data['doctor'] as boolean),
    map((data) => data['doctor'] as Doctor),
    tap((doctor) => this.formGroup.patchValue(doctor)),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public get nome(): AbstractControl | null {
    return this.formGroup.get('nome');
  }

  public get crm(): AbstractControl | null {
    return this.formGroup.get('crm');
  }

  public edit(): void {
    if (this.formGroup.invalid) return;

    const editModel: DoctorDto = this.formGroup.value as DoctorDto;

    const editObserver: Observer<IdApiResponse> = {
      next: () =>
        this.notificationService.success(`Doctor "${editModel.nome}" updated successfully!`, 'OK'),
      error: (err: string) => this.notificationService.error(err, 'OK'),
      complete: () => void this.router.navigate(['/doctors']),
    };

    this.doctor$
      .pipe(
        take(1),
        switchMap((doctor) => this.doctorService.edit(doctor.id, editModel)),
      )
      .subscribe(editObserver);
  }
}
