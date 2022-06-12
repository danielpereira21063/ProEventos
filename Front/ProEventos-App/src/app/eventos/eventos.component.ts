import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss'],
})

export class EventosComponent implements OnInit {
  public eventos: any = [];
  public eventosFiltrados: any = [];
  public widthImage: number = 150;
  public marginImage: number = 2;
  public showImage: boolean = true;
  private _filtroLista: string = "";

  public get filtroLista() {
    return this._filtroLista;
  }

  public set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this._filtroLista) : this.eventos;
  }

  public filtrarEventos(filtrarPor: string) {
    filtrarPor = filtrarPor.toLowerCase();
    return this.eventos.filter((evento: any) => evento.tema.toLowerCase().indexOf(filtrarPor) != -1);
  }

  public getEventos(): void {
    this.http.get("https://localhost:5001/api/evento").subscribe(
      response => {
        this.eventos = response;
        this.eventosFiltrados = response;
      },
      error => console.log(error)
    )
  }

  alterarImagem() {
    this.showImage = !this.showImage;
  }

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getEventos();
  }
}
