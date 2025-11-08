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
import { AuthApiResponse, LoginAuthDto } from '../../../models/auth.models';
import { AuthService } from '../../../services/auth.service';
import { NotificationService } from '../../../services/notification.service';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-login.component',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    RouterLink,
    ReactiveFormsModule,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  protected readonly formBuilder = inject(FormBuilder);
  protected readonly router = inject(Router);
  protected readonly authService = inject(AuthService);
  protected readonly notificationService = inject(NotificationService);

  protected formGroup: FormGroup = this.formBuilder.group({
    userName: ['', [Validators.required.bind(this), Validators.minLength(3)]],
    password: ['', [Validators.required.bind(this), Validators.minLength(6)]],
  });

  public get userName(): AbstractControl<unknown, unknown, unknown> | null {
    return this.formGroup.get('userName');
  }

  public get password(): AbstractControl<unknown, unknown, unknown> | null {
    return this.formGroup.get('password');
  }

  public login(): void {
    if (this.formGroup.invalid) return;

    const loginModel: LoginAuthDto = this.formGroup.value as LoginAuthDto;

    const loginObserver: PartialObserver<AuthApiResponse> = {
      error: (err: HttpErrorResponse) =>
        this.notificationService.error(err.error.erros as string, 'OK'),
      complete: () => void this.router.navigate(['/home']),
    };

    this.authService.login(loginModel).subscribe(loginObserver);
  }
}
