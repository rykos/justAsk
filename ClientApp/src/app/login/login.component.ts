import { Router } from '@angular/router';
import { AuthenticationService } from './../_services/authentication.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  submitted = false;
  error;

  constructor(private formBuilder: FormBuilder, private authenticationService: AuthenticationService, private router: Router) {
    this.loginForm = this.formBuilder.group({
      username: '',
      password: ''
    });
  }

  ngOnInit(): void {
  }

  onSubmit(loginData) {
    console.log(loginData);
    this.submitted = true;
    this.authenticationService.login(loginData.username, loginData.password).subscribe(
      user => {
        this.submitted = false;
        this.router.navigate(['/']).then(() => location.reload());
      },
      error => {
        console.log(error);
        if (error.error.message) {
          this.error = error.message;
        }
        else if (error.error.errors) {
          this.error = error.error.errors[Object.keys(error.error.errors)[0]][0];
          console.log(error.error.errors[Object.keys(error.error.errors)[0]][0]);
        }
        this.submitted = false;
      }
    );
  }
}
