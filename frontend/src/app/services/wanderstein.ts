import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface WandersteinResponse {
  id: number;
  name: string;
  unique_id: string;
  preview_url: string;
  created_at: string;
}

@Injectable({
  providedIn: 'root'
})
export class WandersteinService {
  private apiUrl = 'http://localhost:8080/api/wandersteine';

  constructor(private http: HttpClient) { }

  getRecentWandersteine(): Observable<WandersteinResponse[]> {
    return this.http.get<WandersteinResponse[]>(`${this.apiUrl}/recent`);
  }

  getAllWandersteine(): Observable<WandersteinResponse[]> {
    return this.http.get<WandersteinResponse[]>(this.apiUrl);
  }
}
