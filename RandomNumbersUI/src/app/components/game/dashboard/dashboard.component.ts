import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { interval } from 'rxjs';
import { startWith } from 'rxjs/operators';
import { Match } from 'src/app/models/match.model';
import { AuthService } from 'src/app/services/auth.service';
import { MatchService } from 'src/app/services/match.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  matches: Match[] = [];

  constructor(private router: Router, private authService: AuthService, private matchService: MatchService, private toastr: ToastrService) { }

  ngOnInit() {
    this.loadMatches();
    this.startCountdown();
  }

  loadMatches(): void {
    let isMatchesEmpty = this.matches.length <= 0;

    this.matchService.getAll().subscribe(result => {
      this.matches = result;
      if (!isMatchesEmpty) {
        this.toastr.success("Result updated", "Successful");
      }
    })
  }

  startCountdown() {
    interval(1000).pipe(
      startWith(0)
    ).subscribe(() => {
      this.matches.forEach(match => {
        if (!match.isDone) {
          let timeParts = match.expiryTimestamp.split(':');
          let hour = parseInt(timeParts[0], 10);
          let minutes = parseInt(timeParts[1], 10);
          let seconds = parseInt(timeParts[2], 10);
          let totalSeconds = hour*3600 + minutes * 60 + seconds;

          if (totalSeconds > 0) {
            totalSeconds -= 1;
            let newHour = Math.floor(totalSeconds / 3600);
            let newMinutes = Math.floor(totalSeconds / 60);
            let newSeconds = totalSeconds % 60;
            match.expiryTimestamp = `${this.padNumber(newHour)}:${this.padNumber(newMinutes)}:${this.padNumber(newSeconds)}`;
          } else {
            match.isDone = true;
            match.winnerName = "Press the 'Refresh Results' button";
          }
        }
      });
    });
  }

  padNumber(num: number): string {
    return num < 10 ? '0' + num : num.toString();
  }

  formatExpiryTimestamp(timespan: string): string {
    return timespan;
  }

  playMatch(match: Match) {
    if (!this.isLoggedIn) {
      this.toastr.error("You must log in to play", "Error");
      return;
    }

    if(match.currentUserScore>0){
      this.toastr.error("User has already played this match.", "Error");
      return;
    }

    this.matchService.play(match.id).subscribe(result => {
      match.currentUserScore = result;
      this.toastr.success("You have successfully joined. Your number: " + result, "Successful");
    })
  }

  login(): void {
    this.router.navigate(['/auth/login']);
  }

  register(): void {
    this.router.navigate(['/auth/register']);
  }

  logout(): void {
    this.authService.setToken('');
    this.loadMatches();
  }

  get isLoggedIn() {
    return this.authService.isLoggedIn();
  }
}