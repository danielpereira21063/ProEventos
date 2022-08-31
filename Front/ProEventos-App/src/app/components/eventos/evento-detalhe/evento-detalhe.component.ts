import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Lote } from '@app/models/Lote';
import { EventoService } from '@app/services/eventos/evento.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from '@app/models/Evento';
import { LoteService } from '@app/services/lotes/lote.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  modalRef: BsModalRef;
  public eventoId: number;
  public evento = {} as Evento;
  public form: FormGroup;
  public estadoSalvar = 'post';
  public loteAtual: { id: 0, nome: "", indice: number };

  ngOnInit(): void {
    this.validation();
    this.carregarEvento();

    this.loteAtual = { id: 0, nome: "", indice: 0 }
  }

  constructor(
    private formBuilder: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toast: ToastrService,
    private modalService: BsModalService,
    private loteService: LoteService,
    private router: Router
  ) {
  }

  get modoEditar(): boolean {
    return this.estadoSalvar == 'put'
  }

  get lotes(): FormArray {
    return (this.form.get("lotes") as FormArray);
  }

  public get f(): any {
    return this.form.controls;
  }

  public get bsConfig(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      // containerClass: 'theme-default',
      showWieekNumber: false
    };
  }

  public get bsConfigLote(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY',
      // containerClass: 'theme-default',
      showWieekNumber: false
    };
  }

  public carregarEvento(): void {
    this.eventoId = Number(this.activatedRoute.snapshot.paramMap.get("id") ?? 0);

    if (this.eventoId == 0) {
      return;
    }

    this.spinner.show();

    this.estadoSalvar = 'put';

    this.eventoService.getEventoById(this.eventoId).subscribe({
      next: (evento: Evento) => {
        this.evento = { ...evento };
        this.form.patchValue(this.evento);
        this.evento.lotes.forEach(x => {
          this.lotes.push(this.criarLote(x));
        });
      },
      error: (error: HttpErrorResponse) => {
        this.toast.error(error.message, "Erro ao tentar carregar evento");
      }
    }).add(() => this.spinner.hide())
  }

  public salvarEvento(): void {
    if (!this.form.valid) {
      return;
    }

    this.spinner.show();

    this.evento = this.estadoSalvar == 'post' ? { ...this.form.value } : { id: this.evento.id, ...this.form.value };
    const state = this.estadoSalvar == "post" ? "post" : "put";

    this.eventoService[state](this.evento).subscribe({
      next: (evento: Evento) => {
        this.toast.success("Evento salvo com sucesso.", "Sucesso!");
        this.router.navigate([`/eventos/detalhe/${evento.id}`]);
      },
      error: (error: HttpErrorResponse) => {
        this.toast.success(error.message, "Erro!");
      }
    }).add(() => this.spinner.hide())
  }

  public salvarLotes(): void {
    if (!this.form.controls.lotes.valid) {
      return;
    }

    this.spinner.show();

    this.loteService.saveLote(this.eventoId, this.form.value.lotes)
      .subscribe({
        next: () => {
          this.toast.success("Lotes salvos com sucesso.", "Sucesso!");
          this.lotes.reset();
        },
        error: (error: HttpErrorResponse) => {
          this.toast.error(error.message, "Erro ao salvar lotes!");
          console.error(error);
        }
      }).add(() => this.spinner.hide());

    this.spinner.hide();
  }

  public validation(): void {
    this.form = this.formBuilder.group({
      tema: ["", [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],

      local: ["", Validators.required],

      dataEvento: ["", Validators.required],

      qtdPessoas: ["", [Validators.required, Validators.maxLength(12000)]],

      imagemUrl: ["", [Validators.required]],

      telefone: ["", Validators.required],

      email: ["", [Validators.required, Validators.email]],

      lotes: this.formBuilder.array([])
    });
  }

  public adicionarLote(): void {
    this.lotes.push(this.criarLote({ id: 0 } as Lote));
  }

  public criarLote(lote: Lote): FormGroup {
    return this.formBuilder.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim]
    })
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(formControl: FormControl | AbstractControl | any): any {
    return { 'is-invalid': formControl?.errors && formControl?.touched };
  }


  public removerLote(template: TemplateRef<any>, indice: number): void {
    this.loteAtual.id = this.lotes.get(indice + ".id")?.value;
    this.loteAtual.nome = this.lotes.get(indice + ".nome")?.value;
    this.loteAtual.indice = indice;

    this.modalRef = this.modalService.show(template, { class: "modal-sm" });
  }

  public mudarValorData(value: Date, indice: number, campo: string): void {
    this.lotes.value[indice][campo] = value;
  }

  public retornaTituloLote(nome: string): string {
    return nome == null || nome == "" ? 'Nome do lote' : nome;
  }

  confirmDeleteLote(): void {
    this.modalRef.hide();
    this.spinner.show();

    this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe(
      () => {
        this.toast.success("Lote deletado com sucesso", "Sucesso!");
        this.lotes.removeAt(this.loteAtual.indice);
      },
      (error: HttpErrorResponse) => {
        this.toast.success(error.message, "Erro ao deletar lote!");
      }
    ).add(() => this.spinner.hide())
  }

  declineDeleteLote(): void {
    this.modalRef.hide();
  }
}
