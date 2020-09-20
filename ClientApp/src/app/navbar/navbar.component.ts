import { AuthenticationService } from './../_services/authentication.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  loggedIn: boolean;

  constructor(private authenticationService: AuthenticationService) {
    this.loggedIn = this.authenticationService.currentUserValue !== null;
  }

  ngOnInit(): void {
  }

  logout() {
    this.authenticationService.logout();
  }
}
