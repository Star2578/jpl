import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: true,
  providers: [
    { provide: HttpClient, useClass: HttpClient }
  ]
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  loading: boolean = false;

  constructor(private authService: AuthService, private router: Router) { }

  onSubmit() {
    this.loading = true;
    this.authService.login(this.username, this.password).subscribe(
      () => {
        setTimeout(() => {
          this.loading = false;
          this.router.navigate(['/list']);
        }, 1000)
      },
      error => {
        this.loading = false;
        alert('Login failed');
      }
    );
  }
}
