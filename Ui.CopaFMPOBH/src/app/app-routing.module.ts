import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InicioComponent } from './inicio/inicio.component';
import { PartidasComponent } from './partidas/partidas.component';
import { TimesComponent } from './times/times.component';
import { JogadorComponent } from './jogadores/jogadores.component';
import { GerenciarPartidaComponent } from './gerenciar-partida/gerenciar-partida.component';
import { PartidaComponent } from './gerenciar-partida/partida/partida.component';

const routes: Routes = [
  { path: 'inicio', component: InicioComponent },
  { path: 'partidas', component: PartidasComponent },
  { path: 'gerenciarPartida', component: GerenciarPartidaComponent },
  { path: 'gerenciarPartida/:id', component: PartidaComponent },
  { path: 'times', component: TimesComponent },
  { path: 'jogadores/:id', component: JogadorComponent },
  { path: '', redirectTo: '/inicio', pathMatch: 'full' } // Rota padrão
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
