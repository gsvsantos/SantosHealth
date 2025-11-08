import { AsyncPipe, TitleCasePipe, UpperCasePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import {
  MatCard,
  MatCardHeader,
  MatCardContent,
  MatCardActions,
  MatCardTitle,
} from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { filter, map } from 'rxjs';
import { Doctor } from '../../../../models/doctor.models';

@Component({
  selector: 'app-list-doctors.component',
  imports: [
    MatButtonModule,
    AsyncPipe,
    TitleCasePipe,
    UpperCasePipe,
    MatIcon,
    RouterLink,
    MatCard,
    MatCardHeader,
    MatCardContent,
    MatCardActions,
    MatCardTitle,
  ],
  templateUrl: './list-doctors.component.html',
  styleUrl: './list-doctors.component.scss',
})
export class ListDoctorsComponent {
  protected readonly route = inject(ActivatedRoute);

  protected readonly doctors$ = this.route.data.pipe(
    filter((data) => data['doctors'] as boolean),
    map((data) => data['doctors'] as Doctor[]),
  );
}
