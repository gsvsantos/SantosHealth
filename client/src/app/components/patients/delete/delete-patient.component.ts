import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { filter, map, Observer, shareReplay, switchMap, take } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { PatientDetailsDto } from '../../../models/patient.models';
import { NotificationService } from '../../../services/notification.service';
import { PatientService } from '../../../services/patient.service';

@Component({
  selector: 'app-delete-patient.component',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    RouterLink,
    AsyncPipe,
    FormsModule,
  ],
  templateUrl: './delete-patient.component.html',
  styleUrl: './delete-patient.component.scss',
})
export class DeletePatientComponent {
  private readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly patientService = inject(PatientService);
  protected readonly notificationService = inject(NotificationService);

  protected readonly patient$ = this.route.data.pipe(
    filter((data) => data['patient'] as boolean),
    map((data) => data['patient'] as PatientDetailsDto),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public delete(): void {
    const deleteObserver: Observer<null> = {
      next: () => this.notificationService.success(`Record deleted successfully!`, 'OK'),
      error: (err: string) => this.notificationService.error(err, 'OK'),
      complete: () => void this.router.navigate(['/patients']),
    };

    this.patient$
      .pipe(
        take(1),
        switchMap((patient) => this.patientService.delete(patient.id)),
      )
      .subscribe(deleteObserver);
  }
}
