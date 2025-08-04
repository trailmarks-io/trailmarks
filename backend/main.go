package main

import (
	"log"
	"net/http"
	"github.com/labstack/echo/v4"
	"github.com/labstack/echo/v4/middleware"
	echoSwagger "github.com/swaggo/echo-swagger"
	"github.com/trailmarks-io/trailmarks/backend/database"
	"github.com/trailmarks-io/trailmarks/backend/handlers"
	_ "github.com/trailmarks-io/trailmarks/backend/docs" // Import generated docs
)

// @title Trailmarks API
// @version 1.0
// @description API for managing Wandersteine (hiking stones)
// @termsOfService http://swagger.io/terms/

// @contact.name API Support
// @contact.url http://www.trailmarks.io/support
// @contact.email support@trailmarks.io

// @license.name MIT
// @license.url https://opensource.org/licenses/MIT

// @host localhost:8080
// @BasePath /
func main() {
	// Initialize Echo
	e := echo.New()

	// Middleware
	e.Use(middleware.Logger())
	e.Use(middleware.Recover())
	e.Use(middleware.CORS())

	// Connect to database
	if err := database.ConnectDatabase(); err != nil {
		log.Fatal("Failed to connect to database:", err)
	}

	// Seed database with sample data
	if err := database.SeedDatabase(); err != nil {
		log.Printf("Warning: Failed to seed database: %v", err)
	}

	// Health check endpoint
	e.GET("/health", func(c echo.Context) error {
		return c.JSON(http.StatusOK, map[string]string{
			"status": "healthy",
			"service": "trailmarks-backend",
		})
	})

	// API routes
	api := e.Group("/api")
	
	// Wandersteine routes
	wandersteine := api.Group("/wandersteine")
	wandersteine.GET("", handlers.GetAllWandersteine)
	wandersteine.GET("/recent", handlers.GetRecentWandersteine)

	// Swagger documentation
	e.GET("/swagger/*", echoSwagger.WrapHandler)

	// Start server
	log.Println("Starting server on :8080")
	log.Fatal(e.Start(":8080"))
}