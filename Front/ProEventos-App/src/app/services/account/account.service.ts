import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable, ReplaySubject } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { User } from '@app/models/identity/User';
import { UserUpdate } from '@app/models/identity/UserUpdate';

@Injectable()

export class AccountService {
  private baseUrl = environment.apiUrl + "/api/account";
  private currentUserSource = new ReplaySubject<User>(1);
  public currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }


  public login(model: any): Observable<void> {
    return this.http
      .post<User>(this.baseUrl + "/login", model)
      .pipe(
        take(1),
        map((response: User) => {
          if (response) {
            this.setCurrentUser(response);
          }
        })
      );
  }

  public register(model: User): Observable<void> {
    return this.http
      .post<User>(this.baseUrl + "/register", model)
      .pipe(
        take(1),
        map((response: User) => {
          if (response) {
            this.setCurrentUser(response);
          }
        })
      );
  }

  public logout(): void {
    localStorage.removeItem("user");
    this.currentUserSource.next();
    this.currentUserSource.complete();
  }

  public setCurrentUser(user: User): void {
    localStorage.setItem("user", JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  public getUser(): Observable<User> {
    return this.http
      .get<UserUpdate>(this.baseUrl + "/getUser")
      .pipe(take(1))
  }

  public updateUser(model: User): Observable<void> {
    return this.http.put<UserUpdate>(this.baseUrl + "/update", model)
      .pipe(take(1), map((user: UserUpdate) => {
        this.setCurrentUser(user);
      }))
  }

  public getCurrentUser(): User {
    let currentUser = JSON.parse(localStorage.getItem("user") ?? "null") as User;
    return currentUser;
  }
}
