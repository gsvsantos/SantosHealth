import { MatSnackBar } from '@angular/material/snack-bar';
import { inject, Injectable } from '@angular/core';

@Injectable()
export class NotificationService {
  protected readonly snackBar = inject(MatSnackBar);

  public success(message: string, action: string): void {
    this.snackBar.open(message, action, {
      panelClass: ['notification-success'],
    });
  }
  public warning(message: string, action: string): void {
    this.snackBar.open(message, action, {
      panelClass: ['notification-warning'],
    });
  }
  public error(message: string, action: string): void {
    this.snackBar.open(message, action, {
      panelClass: ['notification-error'],
    });
  }
}
