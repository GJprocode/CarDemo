import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';

// ✅ Add required Angular Material modules
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatError } from '@angular/material/form-field';
import { MatLabel } from '@angular/material/form-field';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule // ✅ Required for mat-raised-button
  ],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;
  errorMessage: string = '';

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router) {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [
        Validators.required,
        Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#@$!%*?&])[A-Za-z\\d@$!%*?&]{6,}$')
      ]]
    });
  }

  onSubmit() {
    if (this.registerForm.valid) {
      const data = this.registerForm.value;
      this.http.post('http://localhost:5073/api/auth/register', data).subscribe({
        next: () => this.router.navigate(['/login']),
        error: (error) => {
          this.errorMessage = error.error?.error || 'Registration failed';
          if (error.error?.error === 'Email already exists.') {
            this.errorMessage = 'This email is already registered. Please use a different email.';
          } else if (error.error?.error === 'Username already exists.') {
            this.errorMessage = 'This username is already taken. Please choose another.';
          }
          console.error('Registration error:', error);
        }
      });
    }
  }
}
