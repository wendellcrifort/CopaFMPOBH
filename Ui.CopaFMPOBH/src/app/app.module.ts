import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PartidasComponent } from './partidas/partidas.component';
import { InicioComponent } from './inicio/inicio.component';
import { MenuComponent } from './menu/menu.component';
import { TimesComponent } from './times/times.component';
import { JogadorComponent } from './jogadores/jogadores.component';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule, DatePipe } from '@angular/common';
import { GerenciarPartidaComponent } from './gerenciar-partida/gerenciar-partida.component';
import { PartidaComponent } from './gerenciar-partida/partida/partida.component';
import { EventosComponent } from './gerenciar-partida/partida/eventos/eventos.component';
import { ClassificacaoComponent } from './classificacao/classificacao.component';
import { ModalConfimacaoComponent } from './modal-confimacao/modal-confimacao.component';
import { HistoricoEventosComponent } from './gerenciar-partida/partida/historico-eventos/historico-eventos.component';
import { CriarPartidaComponent } from './criar-partida/criar-partida.component';
import { FormsModule } from '@angular/forms';
import { GoleiroComponent } from './melhor-goleiro/goleiro.component';

@NgModule({
  declarations: [
    AppComponent,
    PartidasComponent,
    InicioComponent,
    MenuComponent,
    TimesComponent,
    JogadorComponent,
    GerenciarPartidaComponent,
    PartidaComponent,
    EventosComponent,
    ClassificacaoComponent,
    GoleiroComponent,
    ModalConfimacaoComponent,
    HistoricoEventosComponent,
    CriarPartidaComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule    
  ],
  providers: [DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
