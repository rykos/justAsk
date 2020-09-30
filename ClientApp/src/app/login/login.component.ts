import { ActivatedRoute, Router } from '@angular/router';
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
  returnUrl: string;

  constructor(private formBuilder: FormBuilder, private authenticationService: AuthenticationService, private router: Router, private route: ActivatedRoute) {
    this.loginForm = this.formBuilder.group({
      username: '',
      password: ''
    });
  }

  ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  onSubmit(loginData) {
    console.log(loginData);
    this.submitted = true;
    this.authenticationService.login(loginData.username, loginData.password).subscribe(
      user => {
        this.submitted = false;
        this.router.navigateByUrl(this.returnUrl);
      },
      error => {
        console.log(error);
        if (error.error.message) {
          this.error = error.error.message;
        }
        else if (error.error.errors) {
          this.error = error.error.errors[Object.keys(error.error.errors)[0]][0];
        }
        console.log(this.error);
        this.submitted = false;
      }
    );
  }
}
