import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;
  errorMessage: string = '';

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router) {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#@$!%*?&])[A-Za-z\\d@$!%*?&]{6,}$')]]
    });
  }

  onSubmit() {
    if (this.registerForm.valid) {
      const data = this.registerForm.value;
      console.log('Sending registration data:', data);
      this.http.post('http://localhost:5073/api/auth/register', data).subscribe({
        next: (response: any) => {
          this.router.navigate(['/login']); // Silent redirect, no token display
        },
        error: (error) => {
          this.errorMessage = error.error?.error || 'Registration failed';
          if (error.error?.error === 'Email already exists.') {
            this.errorMessage = 'Email already exists. Please use a different email.';
          } else if (error.error?.error === 'Username already exists.') {
            this.errorMessage = 'Username already exists. Please choose a different username.';
          }
          console.error('Registration error:', error);
        }
      });
    }
  }
}