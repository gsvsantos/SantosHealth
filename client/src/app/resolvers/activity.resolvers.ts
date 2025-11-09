import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, ResolveFn } from '@angular/router';
import { Activity } from '../models/activity.models';
import { ActivityService } from '../services/activity.service';

export const listActivitiesResolver: ResolveFn<Activity[]> = (route) => {
  const activityService = inject(ActivityService);

  const activityType = route.queryParamMap.get('tipoAtividade');

  return activityService.getAll(activityType);
};

export const activityDetailsResolver: ResolveFn<Activity> = (route: ActivatedRouteSnapshot) => {
  const activityService = inject(ActivityService);

  if (!route.paramMap.has('id')) throw new Error('Missing "ID" parameter.');

  const id: string = route.paramMap.get('id')!;

  return activityService.getById(id);
};
