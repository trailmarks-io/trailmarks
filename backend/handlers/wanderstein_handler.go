package handlers

import (
	"net/http"
	"github.com/labstack/echo/v4"
	"github.com/trailmarks-io/trailmarks/backend/database"
	"github.com/trailmarks-io/trailmarks/backend/models"
)

// GetRecentWandersteine returns the 5 most recently added Wandersteine
// @Summary Get recent Wandersteine
// @Description Get the 5 most recently added hiking stones
// @Tags wandersteine
// @Accept json
// @Produce json
// @Success 200 {array} models.WandersteinResponse
// @Failure 500 {object} map[string]string
// @Router /api/wandersteine/recent [get]
func GetRecentWandersteine(c echo.Context) error {
	var wandersteine []models.Wanderstein
	
	// Query the 5 most recent wandersteine ordered by creation date
	result := database.DB.Order("created_at DESC").Limit(5).Find(&wandersteine)
	if result.Error != nil {
		return c.JSON(http.StatusInternalServerError, map[string]string{
			"error": "Failed to fetch wandersteine",
		})
	}

	// Convert to response format
	var responses []models.WandersteinResponse
	for _, stone := range wandersteine {
		responses = append(responses, stone.ToResponse())
	}

	return c.JSON(http.StatusOK, responses)
}

// GetAllWandersteine returns all Wandersteine
// @Summary Get all Wandersteine
// @Description Get all hiking stones
// @Tags wandersteine
// @Accept json
// @Produce json
// @Success 200 {array} models.WandersteinResponse
// @Failure 500 {object} map[string]string
// @Router /api/wandersteine [get]
func GetAllWandersteine(c echo.Context) error {
	var wandersteine []models.Wanderstein
	
	result := database.DB.Order("created_at DESC").Find(&wandersteine)
	if result.Error != nil {
		return c.JSON(http.StatusInternalServerError, map[string]string{
			"error": "Failed to fetch wandersteine",
		})
	}

	// Convert to response format
	var responses []models.WandersteinResponse
	for _, stone := range wandersteine {
		responses = append(responses, stone.ToResponse())
	}

	return c.JSON(http.StatusOK, responses)
}