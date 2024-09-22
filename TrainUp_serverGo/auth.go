package main

import (
	"fmt"
	"log"
	"strconv"
	"time"

	"github.com/golang-jwt/jwt"
)

var SECRET_KEY = "mysecretkey"
var invalidTokens = make(map[string]bool) // Mappa per memorizzare i token invalidati

func login(data map[string]string) (string, error) {
	username := data["username"]
	password := data["password"]

	if username == "" || password == "" {
		return "", fmt.Errorf("fields cannot be empty")
	}

	var user User
	// Trova l'utente con username e password
	if err := db.Where("username = ? AND password = ?", username, password).First(&user).Error; err != nil {
		return "", fmt.Errorf("invalid credentials")
	}

	// Controlla e gestisci i token invalidati
	userIDStr := strconv.Itoa(int(user.ID)) // Converti ID in stringa
	if invalidTokens[userIDStr] {
		delete(invalidTokens, userIDStr)
	}

	// Genera un nuovo token JWT
	token := jwt.NewWithClaims(jwt.SigningMethodHS256, jwt.MapClaims{
		"user_id": user.ID,
	})
	tokenString, err := token.SignedString([]byte(SECRET_KEY))
	if err != nil {
		return "", err
	}

	return tokenString, nil
}

func register(data map[string]string) (string, error) {
	// Verifica che tutti i campi siano compilati
	if data["username"] == "" || data["password"] == "" || data["confirmPassword"] == "" || data["age"] == "" || data["weight"] == "" {
		return "", fmt.Errorf("fields cannot be empty")
	}

	// Verifica che la password e la conferma della password coincidano
	if data["password"] != data["confirmPassword"] {
		return "", fmt.Errorf("passwords do not match")
	}

	// Converti age e weight da stringa a int
	age, err := strconv.Atoi(data["age"])
	if err != nil {
		return "", fmt.Errorf("invalid age format")
	}

	weight, err := strconv.Atoi(data["weight"])
	if err != nil {
		return "", fmt.Errorf("invalid weight format")
	}

	// Crea un nuovo utente
	user := User{
		Username: data["username"],
		Password: data["password"],
		Age:      age,
		Weight:   weight,
	}

	// Aggiungi l'utente al database
	if err := db.Create(&user).Error; err != nil {
		return "", fmt.Errorf("database error")
	}

	// Controlla la lista dei token invalidati
	userIDStr := strconv.Itoa(int(user.ID)) // Converti ID in stringa
	if invalidTokens[userIDStr] {
		delete(invalidTokens, userIDStr)
	}

	// Genera un nuovo token JWT
	token := jwt.NewWithClaims(jwt.SigningMethodHS256, jwt.MapClaims{
		"user_id": user.ID,
	})
	tokenString, err := token.SignedString([]byte(SECRET_KEY))
	if err != nil {
		return "", fmt.Errorf("error generating token")
	}

	return tokenString, nil
}

func logout(data map[string]string) error {
	token, ok := data["token"]
	if !ok {
		return fmt.Errorf("missing token")
	}

	// Decodifica il token JWT
	parsedToken, err := jwt.Parse(token, func(token *jwt.Token) (interface{}, error) {
		// Verifica l'algoritmo di firma
		if _, ok := token.Method.(*jwt.SigningMethodHMAC); !ok {
			return nil, fmt.Errorf("unexpected signing method")
		}
		return []byte(SECRET_KEY), nil
	})

	if err != nil {
		log.Println("Errore nella decodifica del token:", err)
		return err
	}

	claims, ok := parsedToken.Claims.(jwt.MapClaims)
	if !ok || !parsedToken.Valid {
		log.Println("Token non valido")
		return fmt.Errorf("invalid token")
	}

	userID, ok := claims["user_id"].(float64)
	if !ok {
		log.Println("ID utente mancante nei claims")
		return fmt.Errorf("user ID missing in claims")
	}

	userIDStr := strconv.FormatFloat(userID, 'f', 0, 64) // Converti l'ID utente in stringa

	// Aggiungi l'ID utente alla lista dei token invalidati
	invalidTokens[userIDStr] = true
	log.Println("Token invalidati:", invalidTokens)

	return nil
}

func updateUserData(data map[string]string) (int, error) {
	// Estrai il token dai dati e decodificalo
	token := data["token"]
	decodedToken, err := jwt.Parse(token, func(token *jwt.Token) (interface{}, error) {
		return []byte(SECRET_KEY), nil
	})
	if err != nil || !decodedToken.Valid {
		return 0, fmt.Errorf("invalid token")
	}

	claims := decodedToken.Claims.(jwt.MapClaims)
	userID := int(claims["user_id"].(float64))

	// Verifica e aggiorna i dati dell'utente
	if data["data"] == "" {
		return 0, nil
	} else if data["data1"] == "age" {
		newAge, err := strconv.Atoi(data["data"]) //Converte una stringa in un valore di tipo int.
		if err != nil {
			return 0, fmt.Errorf("invalid age format")
		}
		// Aggiorna l'età nel database
		if err := db.Model(&User{}).Where("id = ?", userID).Update("age", newAge).Error; err != nil {
			return 0, err
		}
	} else if data["data1"] == "weight" {
		newWeight, err := strconv.Atoi(data["data"])
		if err != nil {
			return 0, fmt.Errorf("invalid weight format")
		}
		// Aggiorna il peso nel database
		if err := db.Model(&User{}).Where("id = ?", userID).Update("weight", newWeight).Error; err != nil {
			return 0, err
		}

		//aggiorna la tabella avanzamento_peso
		// Parsing della data
		dateStr := data["dateString"]
		parsedDate, err := time.Parse("2006-01-02", dateStr) //2006-01-02" è una rappresentazione predefinita usata in Go per indicare la struttura della data
		if err != nil {
			return 0, fmt.Errorf("invalid date format: %v", err)
		}

		// Crea un nuovo record per la tabella avanzamento_peso
		avanzamentoP := AvanzamentoPeso{
			IdUser: uint(userID),
			Peso:   newWeight,
			Data:   parsedDate,
		}

		// Inserisci il record nella tabella avanzamento_peso
		if err := db.Create(&avanzamentoP).Error; err != nil {
			return 0, err
		}

	}
	return 1, nil
}

func changeUserPassword(data map[string]string) (int, error) {
	// Estrai il token dai dati e decodificalo
	token := data["token"]
	decodedToken, err := jwt.Parse(token, func(token *jwt.Token) (interface{}, error) {
		return []byte(SECRET_KEY), nil
	})
	if err != nil || !decodedToken.Valid {
		return 1, fmt.Errorf("invalid token")
	}

	claims := decodedToken.Claims.(jwt.MapClaims)
	userID := int(claims["user_id"].(float64))

	// Verifica le password e aggiorna
	if data["nuovaPassword"] == "" || data["vecchiaPassword"] == "" || data["confermaPassword"] == "" {
		return 1, nil
	} else if data["nuovaPassword"] == data["confermaPassword"] {
		var user User
		if err := db.Where("id = ? AND password = ?", userID, data["vecchiaPassword"]).First(&user).Error; err != nil {
			return 1, fmt.Errorf("incorrect current password")
		}
		// Aggiorna la password nel database
		user.Password = data["nuovaPassword"]
		if err := db.Save(&user).Error; err != nil {
			return 1, err
		}
		return 0, nil
	}
	return 1, nil
}
