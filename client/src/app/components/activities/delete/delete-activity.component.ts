import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { RouterLink, ActivatedRoute, Router } from '@angular/router';
import { filter, map, Observer, shareReplay, switchMap, take } from 'rxjs';
import { Doctor } from '../../../models/doctor.models';
import { ActivityService } from '../../../services/activity.service';
import { NotificationService } from '../../../services/notification.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Activity } from '../../../models/activity.models';

@Component({
  selector: 'app-delete-activity.component',
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
  templateUrl: './delete-activity.component.html',
  styleUrl: './delete-activity.component.scss',
})
export class DeleteActivityComponent {
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly activityService = inject(ActivityService);
  protected readonly notificationService = inject(NotificationService);

  protected readonly activity$ = this.route.data.pipe(
    filter((data) => data['activity'] as boolean),
    map((data) => data['activity'] as Activity),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  public delete(): void {
    const deleteObserver: Observer<null> = {
      next: () => this.notificationService.success(`Record deleted successfully!`, 'OK'),
      error: (err: HttpErrorResponse) =>
        this.notificationService.error(err.error.erros as string, 'OK'),
      complete: () => void this.router.navigate(['/activities']),
    };

    this.activity$
      .pipe(
        take(1),
        switchMap((activity) => this.activityService.delete(activity.id)),
      )
      .subscribe(deleteObserver);
  }
}
