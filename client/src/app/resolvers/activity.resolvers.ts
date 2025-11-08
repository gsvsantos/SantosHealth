import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { Activity } from '../models/activity.models';
import { ActivityService } from '../services/activity.service';

export const listActivitiesResolver: ResolveFn<Activity[]> = (route) => {
  const activityService = inject(ActivityService);

  const activityType = route.queryParamMap.get('tipoAtividade');

  return activityService.getAll(activityType);
};
