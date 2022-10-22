import { Component } from '@angular/core';
import { User } from './models/identity/User';
import { AccountService } from './services/account/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ProEventos-App';

  constructor(public accountService: AccountService) {
  }

  ngOnInit(): void {
    this.setCurrentUser();
  }

  public setCurrentUser() {
    let user = {} as User;

    if (localStorage.getItem("user")) {
      user = JSON.parse(localStorage.getItem("user") ?? "{}");
    }

    if ((user?.primeiroNome?.length > 0)) {
      this.accountService.setCurrentUser(user);
    }
  }
}
