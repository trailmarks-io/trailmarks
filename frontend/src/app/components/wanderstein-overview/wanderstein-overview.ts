import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WandersteinService, WandersteinResponse } from '../../services/wanderstein';

@Component({
  selector: 'app-wanderstein-overview',
  imports: [CommonModule],
  templateUrl: './wanderstein-overview.html',
  styleUrl: './wanderstein-overview.css'
})
export class WandersteinOverviewComponent implements OnInit {
  wandersteine: WandersteinResponse[] = [];
  loading = true;
  error: string | null = null;

  constructor(private wandersteinService: WandersteinService) {}

  ngOnInit(): void {
    this.loadRecentWandersteine();
  }

  loadRecentWandersteine(): void {
    this.loading = true;
    this.error = null;
    
    this.wandersteinService.getRecentWandersteine().subscribe({
      next: (data) => {
        this.wandersteine = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Fehler beim Laden der Wandersteine: ' + err.message;
        this.loading = false;
      }
    });
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('de-DE', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }
}
