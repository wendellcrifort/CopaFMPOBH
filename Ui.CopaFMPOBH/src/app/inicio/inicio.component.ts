import { Component, OnInit } from '@angular/core';
import { PartidasHome } from '../../models/partidaHome';
import { ActivatedRoute } from '@angular/router';
import { PartidaService } from '../../services/partida.service';

@Component({
  selector: 'app-inicio',
  templateUrl: './inicio.component.html',
  styleUrls: ['./inicio.component.css']
})
export class InicioComponent {

  partidas: PartidasHome | undefined;

  constructor(private route: ActivatedRoute,
    private partidaService: PartidaService) { }

  ngOnInit(): void {
    //this.resize();
    this.route.params.subscribe(params => {
      this.partidaService.obterPartidasHome()?.subscribe(x => { this.partidas = x, console.log(x) });
    });
  }

  resize() {
    const mainSlider = document.getElementById('main-slider');
    const sliderItem = document.getElementById('slider-item');
    const windowHeight = window.innerHeight;
    const elementTop = sliderItem!.getBoundingClientRect().top;
    const itemHeight = windowHeight - elementTop;

    if (sliderItem) {
      sliderItem.style.height = itemHeight + 'px';
    }
    if (mainSlider) {
      mainSlider.style.height = itemHeight + 'px';
    }
  }
}
