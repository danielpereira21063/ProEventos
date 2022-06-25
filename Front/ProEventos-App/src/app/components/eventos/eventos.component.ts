import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from '../../models/Evento';
import { EventoService } from '../../services/evento.service';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss'],
  // providers: [EventoService]
})

export class EventosComponent implements OnInit {
  public modalRef: BsModalRef;
  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public widthImage: number = 150;
  public marginImage: number = 2;
  public showImage: boolean = true;
  private filtroListado: string = "";

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {
  }

  public ngOnInit(): void {
    this.getEventos();
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

  public getEventos(): void {

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
        complete: ()=>{
          this.spinner.hide();
        }
      })
  }

  public alterarImagem(): void {
    this.showImage = !this.showImage;
  }

  public openModal(template: TemplateRef<any>): void {
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  public confirm(): void {
    this.modalRef.hide();
    this.toastr.success('Evento excluído com sucesso!', 'Excluído!');
  }

  public decline(): void {
    this.modalRef.hide();
  }


}
