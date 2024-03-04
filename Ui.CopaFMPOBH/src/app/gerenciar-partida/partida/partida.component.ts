import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Partida } from 'src/models/partida';
import { PartidaService } from 'src/services/partida.service';

@Component({
  selector: 'app-partida',
  templateUrl: './partida.component.html',
  styleUrls: ['./partida.component.css']
})
export class PartidaComponent implements OnInit {

  id: string | null = null;
  partida: Partida | undefined;

  constructor(private route: ActivatedRoute, private partidaService: PartidaService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      const partidaId = +params['id'];
      this.partidaService.obterPartidaPorId(partidaId)?.subscribe(x=> this.partida = x);
    });
  }

}
