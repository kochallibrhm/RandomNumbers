import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  username: string = '';
  password: string = '';

  constructor(private authService: AuthService, private router: Router, private toastr: ToastrService) { }

  onSubmit(): void {
    this.authService.register(this.username, this.password).subscribe(result => {
      if(result){
        this.authService.login(this.username,this.password).subscribe(token =>{
          this.authService.setToken(token);
          this.router.navigate(['/game/dashboard']).then(() => {
            this.toastr.success('Successfully registered', 'Successful');
          })
        })
      }
      
    });
  }

  login(): void {
    this.router.navigate(['/auth/login']);
  }
}
