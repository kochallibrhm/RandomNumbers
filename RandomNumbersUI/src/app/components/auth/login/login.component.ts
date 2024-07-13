import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  user!: User | undefined;

  constructor(private authService: AuthService, private router: Router, private toastr: ToastrService) { }

  onSubmit(): void {
    this.authService.login(this.username, this.password).subscribe(result => {
      this.authService.setToken(result);
      this.router.navigate(['/game/dashboard']).then(() => {
        this.toastr.success('Successfully login', 'Successful');
      })
    })
  }

  register(): void {
    this.router.navigate(['/auth/register']);
  }
}
