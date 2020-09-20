import { environment } from './../../environments/environment';
import { Router } from '@angular/router';
import { User } from './../_models/User';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators'
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  constructor(private httpClient: HttpClient, private router: Router) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  register(username: string, password: string) {
    return this.httpClient.post(`${environment.apiUrl}/register`, { username, password }).subscribe(
      user => {
        console.log("user created");
        this.login(username, password).subscribe(
          x => { this.router.navigate(['/']) }
        );
      }
    );
  }

  login(username: string, password: string) {
    return this.httpClient.post<User>(`${environment.apiUrl}/login`, { username, password }).pipe(map(user => {
      localStorage.setItem('currentUser', JSON.stringify(user))
      this.currentUserSubject.next(user);
      return user;
    }));
  }

  logout() {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
    location.reload();
  }
}
