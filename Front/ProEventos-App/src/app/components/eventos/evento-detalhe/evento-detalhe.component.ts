import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  public evento = {} as Evento;
  public form: FormGroup;
  public estadoSalvar = 'post';

  constructor(
    private formBuilder: FormBuilder,
    private localeService: BsLocaleService,
    private router: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toast: ToastrService
  ) {
    this.localeService.use('pt-br');
  }

  ngOnInit(): void {
    this.validation();
    this.carregarEvento();
  }

  public get f(): any {
    return this.form.controls;
  }

  public get bsConfig(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY HH:MM',
      // containerClass: 'theme-default',
      showWieekNumber: false
    };
  }

  public carregarEvento(): void {
    const eventoIdParam = this.router.snapshot.paramMap.get("id");

    if (eventoIdParam != null) {
      this.spinner.show();

      this.estadoSalvar = 'put';

      this.eventoService.getEventoById(+eventoIdParam).subscribe({
        next: (evento: Evento) => {
          this.evento = { ...evento };
          this.form.patchValue(this.evento);
          console.log(evento);
        },
        error: (error: HttpErrorResponse) => {
          this.toast.error(error.message, "Erro ao tentar carregar evento");
        }
      }).add(() => this.spinner.hide())
    }
  }


  public salvarAlteracao(): void {
    this.spinner.show();

    if (this.form.valid) {
      this.evento = this.estadoSalvar == 'post' ? { ...this.form.value } : { id: this.evento.id, ...this.form.value };
      const state = this.estadoSalvar == "post" ? "post" : "put";
      this.eventoService[state](this.evento).subscribe({
        next: () => {
          this.spinner.hide();
          this.toast.success("Evento salvo com sucesso.", "Sucesso!");
        },
        error: (error: HttpErrorResponse) => {
          this.spinner.hide();
          this.toast.success(error.message, "Erro!");
        }
      }).add(() => this.spinner.hide())
    }
  }


  public validation(): void {
    this.form = this.formBuilder.group({
      tema: ["", [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],

      local: ["", Validators.required],

      dataEvento: ["", Validators.required],

      qtdPessoas: ["", [Validators.required, Validators.maxLength(12000)]],

      imagemUrl: ["", [Validators.required]],

      telefone: ["", Validators.required],

      email: ["", [Validators.required, Validators.email]]
    });
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(formControl: FormControl): any {
    return { 'is-invalid': formControl.errors && formControl.touched };
  }

}
