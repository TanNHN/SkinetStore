import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { MatAnchor } from "@angular/material/button";

@Component({
  selector: 'app-error',
  imports: [MatAnchor],
  templateUrl: './error.component.html',
  styleUrl: './error.component.scss',
})
export class ErrorComponent {
  baseUrl = "https://localhost:5001/api/buggy/";
  private http = inject(HttpClient);
  validationErrors = signal([]); 
  get404() {
    this.http.get(this.baseUrl + 'notfound').subscribe({
      next: result => console.log(result),
      error: err => console.log(err)
    })
  }
  get400() {
    this.http.get(this.baseUrl + 'badrequest').subscribe({
      next: result => console.log(result),
      error: err => console.log(err)
    })
  }
  get401() {
    this.http.get(this.baseUrl + 'unauthorized').subscribe({
      next: result => console.log(result),
      error: err => console.log(err)
    })
  }
  get500() {
    this.http.get(this.baseUrl + 'internalerror').subscribe({
      next: result => console.log(result),
      error: err => console.log(err)
    })
  }
  get400ValidationError() {
    this.http.post(this.baseUrl + 'validationerror', {} ).subscribe({
      next: result => console.log(result),
      error: err => this.validationErrors.set(err),
    })
  }
}
