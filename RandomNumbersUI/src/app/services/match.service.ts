import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Match } from '../models/match.model';
import { BaseService } from './base.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class MatchService extends BaseService {
  private matchApiUrl: string;

  constructor(httpClient: HttpClient) {
    super(httpClient);

    this.matchApiUrl = `${this.apiUrl}/Match`;
  }

  play(matchId: number): Observable<number> {
    return this.http.post<number>(`${this.matchApiUrl}/play`, { MatchId: matchId });
  }

  getAll(): Observable<Match[]> {
    return this.http.get<Match[]>(`${this.matchApiUrl}`);
  }
}
