package database

import (
	"fmt"
	"os"
	"gorm.io/driver/postgres"
	"gorm.io/driver/sqlite"
	"gorm.io/gorm"
	"github.com/trailmarks-io/trailmarks/backend/models"
)

var DB *gorm.DB

// ConnectDatabase initializes the database connection
func ConnectDatabase() error {
	// Check if we should use SQLite for demo purposes
	if getEnv("USE_SQLITE", "false") == "true" || getEnv("DB_HOST", "") == "" {
		return connectSQLite()
	}
	return connectPostgres()
}

// connectSQLite connects to SQLite database (for demo)
func connectSQLite() error {
	db, err := gorm.Open(sqlite.Open("trailmarks.db"), &gorm.Config{})
	if err != nil {
		return fmt.Errorf("failed to connect to SQLite database: %v", err)
	}
	
	DB = db
	
	// Auto-migrate the schema
	err = DB.AutoMigrate(&models.Wanderstein{})
	if err != nil {
		return fmt.Errorf("failed to migrate database: %v", err)
	}
	
	return nil
}

// connectPostgres connects to PostgreSQL database
func connectPostgres() error {
	// Default database configuration
	host := getEnv("DB_HOST", "localhost")
	user := getEnv("DB_USER", "postgres")
	password := getEnv("DB_PASSWORD", "postgres")
	dbname := getEnv("DB_NAME", "trailmarks")
	port := getEnv("DB_PORT", "5432")
	sslmode := getEnv("DB_SSLMODE", "disable")

	dsn := fmt.Sprintf("host=%s user=%s password=%s dbname=%s port=%s sslmode=%s TimeZone=Europe/Berlin",
		host, user, password, dbname, port, sslmode)

	db, err := gorm.Open(postgres.Open(dsn), &gorm.Config{})
	if err != nil {
		return fmt.Errorf("failed to connect to database: %v", err)
	}

	DB = db

	// Auto-migrate the schema
	err = DB.AutoMigrate(&models.Wanderstein{})
	if err != nil {
		return fmt.Errorf("failed to migrate database: %v", err)
	}

	return nil
}

// getEnv gets an environment variable with a fallback value
func getEnv(key, fallback string) string {
	if value, exists := os.LookupEnv(key); exists {
		return value
	}
	return fallback
}

// SeedDatabase adds sample data for development
func SeedDatabase() error {
	// Check if data already exists
	var count int64
	DB.Model(&models.Wanderstein{}).Count(&count)
	if count > 0 {
		return nil // Data already exists
	}

	sampleStones := []models.Wanderstein{
		{
			Name:        "Schwarzwaldstein",
			UniqueID:    "WS-2024-001",
			PreviewURL:  "https://picsum.photos/300/200?random=1",
			Description: "Ein historischer Wanderstein im Herzen des Schwarzwaldes",
			Location:    "Schwarzwald, Baden-Württemberg",
		},
		{
			Name:        "Alpenblick",
			UniqueID:    "WS-2024-002",
			PreviewURL:  "https://picsum.photos/300/200?random=2",
			Description: "Wanderstein mit herrlichem Blick auf die Alpen",
			Location:    "Allgäu, Bayern",
		},
		{
			Name:        "Rheintalweg",
			UniqueID:    "WS-2024-003",
			PreviewURL:  "https://picsum.photos/300/200?random=3",
			Description: "Markanter Stein am Rheintalweg",
			Location:    "Rheintal, Baden-Württemberg",
		},
		{
			Name:        "Berggipfel",
			UniqueID:    "WS-2024-004",
			PreviewURL:  "https://picsum.photos/300/200?random=4",
			Description: "Wanderstein auf dem höchsten Punkt der Route",
			Location:    "Harz, Niedersachsen",
		},
		{
			Name:        "Waldlichtung",
			UniqueID:    "WS-2024-005",
			PreviewURL:  "https://picsum.photos/300/200?random=5",
			Description: "Ruhiger Wanderstein in einer schönen Waldlichtung",
			Location:    "Eifel, Nordrhein-Westfalen",
		},
		{
			Name:        "Seeufer",
			UniqueID:    "WS-2024-006",
			PreviewURL:  "https://picsum.photos/300/200?random=6",
			Description: "Wanderstein direkt am malerischen Seeufer",
			Location:    "Chiemsee, Bayern",
		},
	}

	for _, stone := range sampleStones {
		if err := DB.Create(&stone).Error; err != nil {
			return fmt.Errorf("failed to seed database: %v", err)
		}
	}

	return nil
}