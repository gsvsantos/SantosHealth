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
import { Doctor } from '../../../models/doctor.models';
import { NotificationService } from '../../../services/notification.service';
import { HttpErrorResponse } from '@angular/common/http';
import { DoctorService } from '../../../services/doctor.service';

@Component({
  selector: 'app-delete-doctor.component',
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
  templateUrl: './delete-doctor.component.html',
  styleUrl: './delete-doctor.component.scss',
})
export class DeleteDoctorComponent {
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly doctorService = inject(DoctorService);
  protected readonly notificationService = inject(NotificationService);

  protected readonly doctor$ = this.route.data.pipe(
    filter((data) => data['doctor'] as boolean),
    map((data) => data['doctor'] as Doctor),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public delete(): void {
    const deleteObserver: Observer<null> = {
      next: () => this.notificationService.success(`Record deleted successfully!`, 'OK'),
      error: (err: HttpErrorResponse) =>
        this.notificationService.error(err.error.erros[0] as string, 'OK'),
      complete: () => void this.router.navigate(['/doctors']),
    };

    this.doctor$
      .pipe(
        take(1),
        switchMap((doctor) => this.doctorService.delete(doctor.id)),
      )
      .subscribe(deleteObserver);
  }
}
