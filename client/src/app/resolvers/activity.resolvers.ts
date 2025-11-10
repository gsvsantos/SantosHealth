import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn } from '@angular/router';
import { Activity } from '../models/activity.models';
import { ActivityService } from '../services/activity.service';
import { map } from 'rxjs';

export const listActivitiesResolver: ResolveFn<Activity[]> = (route) => {
  const activityService = inject(ActivityService);

  const activityType = route.queryParamMap.get('tipoAtividade');

  return activityService.getAll(activityType).pipe(
    map((activities) => {
      return activities.sort(
        (first, next) => new Date(first.inicio).getTime() - new Date(next.inicio).getTime(),
      );
    }),
  );
};

export const activityDetailsResolver: ResolveFn<Activity> = (route: ActivatedRouteSnapshot) => {
  const activityService = inject(ActivityService);

  if (!route.paramMap.has('id')) throw new Error('Missing "ID" parameter.');

  const id: string = route.paramMap.get('id')!;

  return activityService.getById(id);
};
