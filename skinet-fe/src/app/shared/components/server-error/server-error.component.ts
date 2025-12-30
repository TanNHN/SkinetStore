import { HttpErrorResponse } from '@angular/common/http';
import { Component, signal } from '@angular/core';
import { Router } from '@angular/router';
import { MatCard } from "@angular/material/card";

@Component({
  selector: 'app-server-error',
  imports: [MatCard],
  templateUrl: './server-error.component.html',
  styleUrl: './server-error.component.scss',
})
export class ServerErrorComponent {
  error = signal<any>('');
  // 

// we are going to use the navigation extras inside the constructor for this class, because that is the only

// place that we're going to be able to access this information.

// We can't access it in one of the servers lifetime events like the Oninit.

// We have to do this inside the constructor.
  constructor(private router: Router){
    const nav = this.router.currentNavigation();
    this.error.set(nav?.extras.state?.['error']);
  }
}
