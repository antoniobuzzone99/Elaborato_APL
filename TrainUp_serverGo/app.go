package main

import (
	"log"

	"github.com/labstack/echo/v4"
	"github.com/labstack/echo/v4/middleware"
	"gorm.io/driver/mysql"
	"gorm.io/gorm"
)

var db *gorm.DB
var err error

func setupRoutes(e *echo.Echo) {
	e.POST("/login", userLogin)
	e.POST("/register", userRegister)
	e.POST("/logout", userLogout)
	e.POST("/update/data", updateData)
	e.POST("/update/password", updatePassword)
}

func main() {
	// Connessione al database
	dsn := "root:12345@tcp(localhost:3309)/TrainUp?charset=utf8mb4&parseTime=True&loc=Local"
	db, err = gorm.Open(mysql.Open(dsn), &gorm.Config{})
	if err != nil {
		log.Fatalf("Errore di connessione al database: %v", err)
	}

	// Esegui le migrazioni
	err = db.AutoMigrate(&User{})
	if err != nil {
		log.Fatalf("Errore durante la migrazione: %v", err)
	}

	// Crea una nuova istanza di Echo
	e := echo.New()

	// Middleware
	e.Use(middleware.Logger())
	e.Use(middleware.Recover())

	// Rotte
	setupRoutes(e)

	// Avvio del server
	e.Logger.Fatal(e.Start(":5002"))
}
