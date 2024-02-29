import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-inicio',
  templateUrl: './inicio.component.html',
  styleUrls: ['./inicio.component.css']
})
export class InicioComponent implements OnInit{

    ngOnInit(): void {
        this.resize();
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
