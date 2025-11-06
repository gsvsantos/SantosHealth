import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { RouterLink } from '@angular/router';
import { MatDivider } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'app-home',
  imports: [RouterLink, MatDivider, MatButtonModule, MatIcon, MatCardModule, MatListModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class Home {}
