import { Component, DOCUMENT, EventEmitter, inject, Input, Output } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { AsyncPipe } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { AuthenticatedUserModel } from '../../../models/auth.models';
import { MatSlideToggleChange, MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatMenuModule } from '@angular/material/menu';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-shell',
  templateUrl: './shell.component.html',
  styleUrl: './shell.component.scss',
  imports: [
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    MatSlideToggleModule,
    MatMenuModule,
    AsyncPipe,
    RouterLink,
  ],
})
export class ShellComponent {
  private breakpointObserver = inject(BreakpointObserver);
  private document = inject(DOCUMENT);

  @Input({ required: true }) public authenticatedUser?: AuthenticatedUserModel;
  @Output() public logoutEvent = new EventEmitter<void>();

  public isHandset$: Observable<boolean> = this.breakpointObserver
    .observe([Breakpoints.XSmall, Breakpoints.Small, Breakpoints.Handset])
    .pipe(
      map((result) => result.matches),
      shareReplay(),
    );

  public itensNavbar = [
    {
      titulo: 'Home',
      icone: 'home',
      link: '/home',
    },
    {
      titulo: 'Activities',
      icone: 'pending_actions',
      link: 'activities',
    },
    {
      titulo: 'Patients',
      icone: 'people_alt',
      link: 'patients',
    },
    {
      titulo: 'Doctors',
      icone: 'medical_information',
      link: 'doctors',
    },
  ];

  public onThemeChange(___: MatSlideToggleChange): void {
    void this.document.body.classList.toggle('dark');
  }
}
