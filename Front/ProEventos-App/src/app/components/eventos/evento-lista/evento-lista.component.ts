import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/eventos/evento.service';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

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
  public pagination = {} as Pagination;

  public termoBuscaChanged: Subject<string> = new Subject<string>();

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {
  }

  public ngOnInit(): void {
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 3,
      totalItems: 1,
    } as Pagination;

    this.carregarEventos();
  }

  public filtrarEventos(evt: any): void {
    if (this.termoBuscaChanged.observers.length === 0) {

      this.termoBuscaChanged.pipe(debounceTime(250)).subscribe(
        filtrarPor => {
          this.spinner.show();
          this.eventoService.getEventos(this.pagination.currentPage, this.pagination.itemsPerPage, filtrarPor)
            .subscribe({
              next: (paginatedResult: PaginatedResult<Evento[]>) => {
                this.eventos = paginatedResult.result;
                this.eventosFiltrados = this.eventos;
                this.pagination = paginatedResult.pagination;
              },
              error: (error: HttpErrorResponse) => {
                this.spinner.hide();
                this.toastr.error(error.message, "Erro");
              }
            }).add(this.spinner.hide());
        }
      );
    }
    this.termoBuscaChanged.next(evt.value);
  }

  public pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.carregarEventos();
  }

  public carregarEventos(): void {
    this.spinner.show();

    this.eventoService
      .getEventos(this.pagination.currentPage, this.pagination.itemsPerPage)
      .subscribe({
        next: (paginatedResult: PaginatedResult<Evento[]>) => {
          this.eventos = paginatedResult.result;
          this.eventosFiltrados = this.eventos;
          this.pagination = paginatedResult.pagination;
        },
        error: (error: HttpErrorResponse) => {
          this.spinner.hide();
          this.toastr.error(error.message, "Erro");
        }
      }).add(this.spinner.hide());
  }

  public alterarImagem(): void {
    this.showImage = !this.showImage;
  }

  public detalheEvento(id: number): void {
    this.router.navigate([`eventos/detalhe/${id}`]);
  }

  public mostraImagem(imagemUrl: string): string {
    return (imagemUrl != "")
      ? environment.apiUrl + '/resources/images/' + imagemUrl
      : '/assets/img/semImagem.jpeg';
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
