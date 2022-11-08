import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account/account.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil-detalhe',
  templateUrl: './perfil-detalhe.component.html',
  styleUrls: ['./perfil-detalhe.component.scss']
})
export class PerfilDetalheComponent implements OnInit {

  constructor(
    private formBuilder: FormBuilder,
    public accountService: AccountService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.validation();

    this.carregarUsuario();
  }

  public get f(): any {
    return this.form.controls;
  }

  public userUpdate = {} as UserUpdate;
  public form!: FormGroup;

  public carregarUsuario(): void {
    this.spinner.show();

    this.accountService.getUser().subscribe(
      (user: any) => {
        console.log(user);
        this.userUpdate = user;
        this.form.patchValue(this.userUpdate);
        this.toastr.success("Usuário carregado com sucesso", "Sucesso!");
      },
      (error: HttpErrorResponse) => {
        this.toastr.error(error.message, "Erro!");
        this.router.navigate(["/dashboard"]);
      }
    ).add(this.spinner.hide());
  }

  public validation() {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch("password", "confirmePassword")
    }

    this.form = this.formBuilder.group({
      primeiroNome: ["", [Validators.required]],
      ultimoNome: ["", [Validators.required]],
      email: ["", [Validators.required, Validators.email]],
      phoneNumber: [""],
      userName: [""],
      titulo: ["NaoInformado", [Validators.required]],
      funcao: ["NaoInformado", [Validators.required]],
      descricao: ["", [Validators.required]],
      password: ['', [Validators.minLength(4), Validators.nullValidator]],
      confirmePassword: ['', Validators.nullValidator],
    }, formOptions);
  }

  public resetForm(event: any): void {
    event.preventDefault();
    this.form.reset();
  }

  public onSubmit(): void {
    this.atualizarUsuario();
  }

  public atualizarUsuario() {
    this.userUpdate = { ... this.form.value }
    this.spinner.show();

    this.accountService.updateUser(this.userUpdate).subscribe(
      () => {
        this.toastr.success("Usuário atualizado com sucesso", "Sucesso");
      },
      (error: HttpErrorResponse) => {
        this.toastr.error(error.message);
      }
    ).add(() => {
      this.spinner.hide();
    });
  }

}
