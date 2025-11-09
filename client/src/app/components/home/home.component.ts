import { AsyncPipe, TitleCasePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatDivider } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { filter, map, tap } from 'rxjs';
import { DoctorDtoToTop10 } from '../../models/doctor.models';

@Component({
  selector: 'app-home',
  imports: [
    AsyncPipe,
    TitleCasePipe,
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

  protected readonly top10Doctors$ = this.route.data.pipe(
    filter((data) => data['top10Doctors'] as boolean),
    map((data) => data['top10Doctors'] as DoctorDtoToTop10[]),
    tap((res) => console.log(res)),
  );
}
