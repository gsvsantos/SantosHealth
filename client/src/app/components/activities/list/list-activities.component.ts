import { AsyncPipe, DatePipe, TitleCasePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import {
  MatCard,
  MatCardHeader,
  MatCardContent,
  MatCardActions,
  MatCardTitle,
  MatCardSubtitle,
} from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatOption, MatSelect } from '@angular/material/select';
import { filter, map } from 'rxjs';
import { Activity } from '../../../models/activity.models';

@Component({
  selector: 'app-list-activities.component',
  imports: [
    MatButtonModule,
    AsyncPipe,
    TitleCasePipe,
    DatePipe,
    MatIcon,
    RouterLink,
    MatCard,
    MatCardHeader,
    MatCardContent,
    MatCardActions,
    MatCardTitle,
    MatCardSubtitle,
    MatFormField,
    MatLabel,
    MatSelect,
    MatOption,
  ],
  templateUrl: './list-activities.component.html',
  styleUrl: './list-activities.component.scss',
})
export class ListActivitiesComponent {
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);

  protected readonly activities$ = this.route.data.pipe(
    filter((data) => data['activities'] as boolean),
    map((data) => data['activities'] as Activity[]),
  );

  public onFilterChange(type: string): void {
    void this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { tipoAtividade: type },
      queryParamsHandling: 'merge',
    });
  }

  public clearFilter(): void {
    void this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { tipoAtividade: null },
      queryParamsHandling: 'merge',
    });
  }
}
