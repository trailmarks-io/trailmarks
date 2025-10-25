import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

export interface WandersteinResponse {
  id: number;
  name: string;
  unique_Id: string;
  preview_Url: string;
  created_At: string;
  latitude?: number;
  longitude?: number;
  location: string;
}

export interface WandersteinDetailResponse {
  id: number;
  name: string;
  unique_Id: string;
  preview_Url: string;
  description: string;
  location: string;
  created_At: string;
  updated_At: string;
  latitude?: number;
  longitude?: number;
}

@Injectable({
  providedIn: 'root'
})
export class WandersteinService {
  private apiUrl = `${environment.apiUrl}/api/wandersteine`;

  constructor(private http: HttpClient) { }

  getRecentWandersteine(): Observable<WandersteinResponse[]> {
    return this.http.get<WandersteinResponse[]>(`${this.apiUrl}/recent`);
  }

  getAllWandersteine(): Observable<WandersteinResponse[]> {
    return this.http.get<WandersteinResponse[]>(this.apiUrl);
  }

  getNearbyWandersteine(latitude?: number, longitude?: number, radiusKm?: number): Observable<WandersteinResponse[]> {
    const params: any = {};
    if (latitude !== undefined) params.latitude = latitude.toString();
    if (longitude !== undefined) params.longitude = longitude.toString();
    if (radiusKm !== undefined) params.radiusKm = radiusKm.toString();
    
    return this.http.get<WandersteinResponse[]>(`${this.apiUrl}/nearby`, { params });
  }

  getWandersteinByUniqueId(uniqueId: string): Observable<WandersteinDetailResponse> {
    return this.http.get<WandersteinDetailResponse>(`${this.apiUrl}/${uniqueId}`);
  }
}
