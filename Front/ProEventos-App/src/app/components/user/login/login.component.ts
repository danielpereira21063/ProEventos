import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserLogin } from '@app/models/identity/UserLogin';
import { AccountService } from '@app/services/account/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  model = {} as UserLogin;

  constructor(
    private accountService: AccountService,
    private router: Router,
    private toastr: ToastrService
  ) { }


  ngOnInit(): void { }


  public login(): void {
    this.accountService.login(this.model).subscribe(
      () => {
        this.router.navigateByUrl("/dashboard");
      },
      (response: HttpErrorResponse) => {
        this.toastr.error(response.error, "Erro ao fazer login!");
      }
    );
  }

}
