import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-register',
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  standalone: true,
  providers: [
    { provide: HttpClient, useClass: HttpClient }
  ]
})
export class RegisterComponent {
  username: string = '';
  password: string = '';
  loading: boolean = false;
  progress: number = 0;

  constructor(private authService: AuthService, private router: Router) { }

  onSubmit() {
    this.loading = true;
    this.authService.register(this.username, this.password).subscribe(
      () => {
        setTimeout(() => {
          this.loading = false;
          this.router.navigate(['/login']);
        }, 1000)
      },
      (error) => {
        this.loading = false;
        console.error('Registration error:', error);
        // Handle error (e.g., show error message)
      }
    );
  }
}
