package main

import (
	"log"
	"time"
)

type User struct {
	ID       uint   `gorm:"primaryKey"`
	Username string `gorm:"type:text;not null"`
	Password string `gorm:"type:text;not null"`
	Age      int    `gorm:"not null"`
	Weight   int    `gorm:"not null"`
}

// TableName permette di specificare un nome personalizzato per la tabella
func (User) TableName() string {
	return "user" // Nome della tabella al singolare
}

type AvanzamentoPeso struct {
	ID     uint      `gorm:"primaryKey"`
	IdUser uint      `gorm:"not null;index"`
	Peso   int       `gorm:"not null"`
	Data   time.Time `gorm:"type:date;not null"`
	User   User      `gorm:"foreignKey:IdUser"`
}

func (AvanzamentoPeso) TableName() string {
	return "avanzamentoPeso"
}

func init() {
	if db != nil {
		db.AutoMigrate(&User{})
	} else {
		log.Println("Database is not initialized")
	}
}
