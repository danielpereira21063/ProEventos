import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ValidatorField } from '@app/helpers/validatorField';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  constructor(
    private formBuilder: FormBuilder
  ) { }

  public form!: FormGroup;

  public get f(): any {
    return this.form.controls;
  }

  ngOnInit(): void {
    this.validation();
  }

  public validation(): void {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch("senha", "confirmeSenha")
    }

    this.form = this.formBuilder.group({
      primeiroNome: ["", [Validators.required]],
      ultimoNome: ["", [Validators.required]],
      email: ["", [Validators.required, Validators.email]],
      userName: ["", [Validators.required]],
      senha: ["", [Validators.required, Validators.minLength(6)]],
      confirmeSenha: ["", Validators.required]
    }, formOptions);
  }

}
