import { Component } from '@angular/core';
import { Time } from 'src/models/time';
import { JogadorService } from 'src/services/jogador.service';

@Component({
  selector: 'app-times',
  templateUrl: './times.component.html',
  styleUrls: ['./times.component.css']
})
export class TimesComponent {
  times: Time[] = []; 

  constructor(private rankingService: JogadorService) { }

  ngOnInit(): void {
    this.rankingService.getTimes().subscribe(data => {
      console.log(data)
      this.times = data
    });
  }
}
