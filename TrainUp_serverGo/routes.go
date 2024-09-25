package main

import (
	"fmt"
	"github.com/labstack/echo/v4"
	"net/http"
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
		fmt.Println("Errore nel binding dei dati:", err)
		return c.JSON(http.StatusBadRequest, map[string]string{"state": "fault"})
	}

	// Logica di registrazione
	token, err := register(data)
	if err != nil {
		fmt.Println("Errore nella registrazione:", err)

		// Gestione di errori specifici
		if err.Error() == "passwords diverse" {
			return c.JSON(http.StatusBadRequest, map[string]string{"state": "fault2"})
		} else if err.Error() == "campi vuoti" {
			return c.JSON(http.StatusBadRequest, map[string]string{"state": "fault1"})
		} else if err.Error() == "età non valida" {
			return c.JSON(http.StatusBadRequest, map[string]string{"state": "fault3"})
		} else if err.Error() == "peso non valido" {
			return c.JSON(http.StatusBadRequest, map[string]string{"state": "fault4"})
		} else {
			// Altri errori generali
			return c.JSON(http.StatusInternalServerError, map[string]string{"state": "fault1"})
		}
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
		return c.JSON(http.StatusInternalServerError, map[string]int{"state": state})
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
