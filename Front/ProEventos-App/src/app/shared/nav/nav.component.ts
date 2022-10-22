import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '@app/services/account/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  isCollapsed = true;
  constructor(
    private router: Router,
    public accountService: AccountService
    ) { }

  ngOnInit(): void {
  }

  logout(): void{
    this.accountService.logout();
    this.router.navigateByUrl("/home");
  }

  showMenu() {
    return this.router.url != "/user/login";
  }
}
