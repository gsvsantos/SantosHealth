import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  AbstractControl,
  ReactiveFormsModule,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { PartialObserver } from 'rxjs';
import { AuthService } from '../../../services/auth.service';
import { NotificationService } from '../../../services/notification.service';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { AuthApiResponse, RegisterAuthDto } from '../../../models/auth.models';

@Component({
  selector: 'app-register.component',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    RouterLink,
    ReactiveFormsModule,
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly authService = inject(AuthService);
  protected readonly notificationService = inject(NotificationService);

  protected formGroup: FormGroup = this.formBuilder.group({
    userName: ['', [Validators.required.bind(this), Validators.minLength(3)]],
    email: ['', [Validators.required.bind(this), Validators.email.bind(this)]],
    password: ['', [Validators.required.bind(this), Validators.minLength(6)]],
  });

  public get userName(): AbstractControl<unknown, unknown, unknown> | null {
    return this.formGroup.get('userName');
  }

  public get email(): AbstractControl<unknown, unknown, unknown> | null {
    return this.formGroup.get('email');
  }

  public get password(): AbstractControl<unknown, unknown, unknown> | null {
    return this.formGroup.get('password');
  }

  public register(): void {
    if (this.formGroup.invalid) return;

    const registerModel: RegisterAuthDto = this.formGroup.value as RegisterAuthDto;

    const registerObserver: PartialObserver<AuthApiResponse> = {
      error: (err: string) => (console.log(err), this.notificationService.error(err, 'OK')),
      complete: () => void this.router.navigate(['/home']),
    };

    this.authService.register(registerModel).subscribe(registerObserver);
  }
}
