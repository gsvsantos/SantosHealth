import { AsyncPipe, TitleCasePipe } from '@angular/common';
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
import { Patient } from '../../../models/patient.models';

@Component({
  selector: 'app-list-patients.component',
  imports: [
    MatButtonModule,
    AsyncPipe,
    TitleCasePipe,
    MatIcon,
    RouterLink,
    MatCard,
    MatCardHeader,
    MatCardContent,
    MatCardActions,
    MatCardTitle,
  ],
  templateUrl: './list-patients.component.html',
  styleUrl: './list-patients.component.scss',
})
export class ListPatientsComponent {
  protected readonly route = inject(ActivatedRoute);

  protected readonly patients$ = this.route.data.pipe(
    filter((data) => data['patients'] as boolean),
    map((data) => data['patients'] as Patient[]),
  );
}
