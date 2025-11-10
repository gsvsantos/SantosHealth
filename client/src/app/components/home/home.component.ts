import { AsyncPipe, DatePipe, TitleCasePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatDivider } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { filter, map, tap } from 'rxjs';
import { DoctorDtoToTop10 } from '../../models/doctor.models';
import { Activity } from '../../models/activity.models';

@Component({
  selector: 'app-home',
  imports: [
    AsyncPipe,
    TitleCasePipe,
    DatePipe,
    RouterLink,
    MatDivider,
    MatButtonModule,
    MatIcon,
    MatCardModule,
    MatListModule,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class Home {
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);

  protected readonly upcomingActivities$ = this.route.data.pipe(
    filter((data) => data['upcomingActivities'] as boolean),
    map((data) => data['upcomingActivities'] as Activity[]),
    map((activities) => {
      const nowUtcMs = Date.now();

      return activities
        .filter((activity) => {
          const inicio = activity.inicio;
          if (!inicio) return false;

          const inicioMs = new Date(inicio).getTime();
          return Number.isFinite(inicioMs) && inicioMs >= nowUtcMs;
        })
        .slice(0, 10);
    }),
  );

  protected readonly top10Doctors$ = this.route.data.pipe(
    filter((data) => data['top10Doctors'] as boolean),
    map((data) => data['top10Doctors'] as DoctorDtoToTop10[]),
  );
}
