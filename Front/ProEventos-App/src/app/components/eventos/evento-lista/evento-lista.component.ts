import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/evento.service';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  public modalRef: BsModalRef;
  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public widthImage: number = 150;
  public marginImage: number = 2;
  public showImage: boolean = true;
  public eventoId: number;
  private filtroListado: string = "";

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {
  }

  public ngOnInit(): void {
    this.carregarEventos();
    this.spinner.show();
  }

  public get filtroLista() {
    return this.filtroListado;
  }

  public set filtroLista(value: string) {
    this.filtroListado = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(value) : this.eventos;
  }

  public filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLowerCase();
    return this.eventos.filter((evento: Evento) => evento.tema.toLowerCase().indexOf(filtrarPor) != -1);
  }

  public carregarEventos(): void {
    this.eventoService
      .getEventos()
      .subscribe({
        next: (eventos: Evento[]) => {
          this.eventos = eventos;
          this.eventosFiltrados = this.eventos;
        },
        error: (error: HttpErrorResponse) => {
          this.spinner.hide();
          this.toastr.error(error.message, "Erro");
          console.log(error);
        },
        complete: () => {
          this.spinner.hide();
        }
      })
  }

  public alterarImagem(): void {
    this.showImage = !this.showImage;
  }

  public detalheEvento(id: number): void {
    this.router.navigate([`eventos/detalhe/${id}`]);
    console.log(id)
  }

  public openModal($event: any, template: TemplateRef<any>, eventoId: number): void {
    $event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  public confirm(): void {
    this.modalRef.hide();
    this.spinner.show();

    this.eventoService.deleteEvento(this.eventoId).subscribe({
      next: (result: any) => {
        console.log(result);
        if (result.success) {
          this.toastr.success("Evento deletado com sucesso", "Deletado!");
          this.spinner.hide();
          this.carregarEventos();
        }
      },
      error: (error: HttpErrorResponse) => {
        this.toastr.error(error.message, "Erro ao deletar evento!");
        this.spinner.hide();
        console.log(error);
      },
      complete: () => {
        this.spinner.hide();
      }
    })
  }

  public decline(): void {
    this.modalRef.hide();
  }

}
