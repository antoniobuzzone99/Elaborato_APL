package main

import (
	"fmt"
	"net/http"

	"github.com/labstack/echo/v4"
)

func userLogin(c echo.Context) error {
	var data map[string]string
	if err := c.Bind(&data); err != nil {
		return c.JSON(http.StatusBadRequest, map[string]string{"state": "fault"})
	}

	// Logica di login
	token, err := login(data)
	if err != nil {
		return c.JSON(http.StatusUnauthorized, map[string]string{"state": "fault"})
	}
	return c.JSON(http.StatusOK, map[string]string{"state": "success", "token": token})
}

func userRegister(c echo.Context) error {
	var data map[string]string
	if err := c.Bind(&data); err != nil {
		// Aggiungi logging per vedere l'errore di binding
		fmt.Println("Errore nel binding dei dati:", err)
		return c.JSON(http.StatusBadRequest, map[string]string{"state": "fault"})
	}

	// Aggiungi logging per vedere i dati ricevuti
	fmt.Println("Dati ricevuti:", data)

	// Logica di registrazione
	token, err := register(data)
	if err != nil {
		// Aggiungi logging per vedere cosa non va nel processo di registrazione
		fmt.Println("Errore nella registrazione:", err)
		return c.JSON(http.StatusInternalServerError, map[string]string{"state": "fault"})
	}

	return c.JSON(http.StatusOK, map[string]string{"state": "success", "token": token})
}

func userLogout(c echo.Context) error {
	var data map[string]string
	if err := c.Bind(&data); err != nil {
		return c.JSON(http.StatusBadRequest, map[string]string{"state": "fault"})
	}

	// Logica di logout
	err := logout(data)
	if err != nil {
		return c.JSON(http.StatusUnauthorized, map[string]string{"state": "fault"})
	}
	return c.JSON(http.StatusOK, map[string]string{"state": "success"})
}

func updateData(c echo.Context) error {
	var data map[string]string
	if err := c.Bind(&data); err != nil {
		return c.JSON(http.StatusBadRequest, map[string]string{"state": "fault"})
	}

	state, err := updateUserData(data)
	if err != nil {
		return c.JSON(http.StatusInternalServerError, map[string]string{"state": "fault"})
	}
	return c.JSON(http.StatusOK, map[string]int{"state": state})
}

func updatePassword(c echo.Context) error {
	var data map[string]string
	if err := c.Bind(&data); err != nil {
		return c.JSON(http.StatusBadRequest, map[string]string{"state": "fault"})
	}

	state, err := changeUserPassword(data)
	if err != nil {
		return c.JSON(http.StatusInternalServerError, map[string]string{"state": "fault"})
	}
	return c.JSON(http.StatusOK, map[string]int{"state": state})
}
