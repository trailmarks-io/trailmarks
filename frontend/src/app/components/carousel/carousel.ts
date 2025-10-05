import { Component, Input, HostListener, TemplateRef, ContentChild } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-carousel',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './carousel.html'
})
export class CarouselComponent {
  @Input() items: any[] = [];
  @Input() maxVisibleItems = 3;
  @ContentChild(TemplateRef) itemTemplate!: TemplateRef<any>;

  currentIndex = 0;
  private touchStartX = 0;
  private touchEndX = 0;

  get visibleItems(): any[] {
    const start = this.currentIndex;
    const end = Math.min(start + this.maxVisibleItems, this.items.length);
    return this.items.slice(start, end);
  }

  get canGoPrevious(): boolean {
    return this.currentIndex > 0;
  }

  get canGoNext(): boolean {
    return this.currentIndex + this.maxVisibleItems < this.items.length;
  }

  goToPrevious(): void {
    if (this.canGoPrevious) {
      this.currentIndex--;
    }
  }

  goToNext(): void {
    if (this.canGoNext) {
      this.currentIndex++;
    }
  }

  @HostListener('touchstart', ['$event'])
  onTouchStart(event: TouchEvent): void {
    this.touchStartX = event.changedTouches[0].screenX;
  }

  @HostListener('touchend', ['$event'])
  onTouchEnd(event: TouchEvent): void {
    this.touchEndX = event.changedTouches[0].screenX;
    this.handleSwipe();
  }

  private handleSwipe(): void {
    const swipeThreshold = 50;
    const diff = this.touchStartX - this.touchEndX;

    if (Math.abs(diff) > swipeThreshold) {
      if (diff > 0) {
        // Swipe left - go to next
        this.goToNext();
      } else {
        // Swipe right - go to previous
        this.goToPrevious();
      }
    }
  }
}
