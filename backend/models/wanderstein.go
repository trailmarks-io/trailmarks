package models

import (
	"time"
)

// Wanderstein represents a hiking stone with its metadata
type Wanderstein struct {
	ID          uint      `json:"id" gorm:"primaryKey" example:"1"`
	Name        string    `json:"name" gorm:"not null" example:"Steiniger Pfad"`
	UniqueID    string    `json:"unique_id" gorm:"uniqueIndex;not null" example:"WS-2024-001"`
	PreviewURL  string    `json:"preview_url" example:"https://example.com/images/wanderstein1.jpg"`
	Description string    `json:"description" example:"Ein wunderschöner Wanderstein am Bergpfad"`
	Location    string    `json:"location" example:"Schwarzwald, Baden-Württemberg"`
	CreatedAt   time.Time `json:"created_at"`
	UpdatedAt   time.Time `json:"updated_at"`
}

// WandersteinResponse represents the response format for API
type WandersteinResponse struct {
	ID         uint   `json:"id" example:"1"`
	Name       string `json:"name" example:"Steiniger Pfad"`
	UniqueID   string `json:"unique_id" example:"WS-2024-001"`
	PreviewURL string `json:"preview_url" example:"https://example.com/images/wanderstein1.jpg"`
	CreatedAt  string `json:"created_at" example:"2024-01-15T10:30:00Z"`
}

// ToResponse converts Wanderstein to WandersteinResponse
func (w *Wanderstein) ToResponse() WandersteinResponse {
	return WandersteinResponse{
		ID:         w.ID,
		Name:       w.Name,
		UniqueID:   w.UniqueID,
		PreviewURL: w.PreviewURL,
		CreatedAt:  w.CreatedAt.Format(time.RFC3339),
	}
}