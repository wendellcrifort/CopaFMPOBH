import { Component } from '@angular/core';
import { Time } from 'src/models/time';
import { RankingService } from 'src/services/ranking.service';

@Component({
  selector: 'app-times',
  templateUrl: './times.component.html',
  styleUrls: ['./times.component.css']
})
export class TimesComponent {
  times: Time[] = []; 

  constructor(private rankingService: RankingService) { }

  ngOnInit(): void {
    this.rankingService.getTimes().subscribe(data => {
      this.times = data
    });
  }
}
