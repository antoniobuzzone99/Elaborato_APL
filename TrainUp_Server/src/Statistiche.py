from io import BytesIO
from pipes import quote

import numpy as np
from flask import send_file
import jwt
from sqlalchemy import func
from models.user import TrainingCard_, db, cardsFavoreites, ExercisesCards_, User, AvanzamentoPeso
import matplotlib.pyplot as plt
import io
SECRET_KEY = "mysecretkey"

def cards_most_used():
    # Esegui una query per ottenere il numero di volte che ogni scheda è stata salvata tra i preferiti dagli utenti
    card_usage = db.session.query(cardsFavoreites.id_card, func.count(cardsFavoreites.id_card)).group_by(cardsFavoreites.id_card).all()

    # Estrai i dati dalla query
    card_ids, usage_counts = zip(*card_usage)
    usage_counts = [int(round(count)) for count in usage_counts]

    # Crea il grafico
    plt.figure()  # Crea una nuova figura
    plt.bar(card_ids, usage_counts)
    plt.xlabel('ID Scheda di Allenamento')
    plt.ylabel('Numero di Utenti che l\'hanno Salvata')
    plt.title('Schede di Allenamento Più Usate dagli Utenti')
    plt.yticks(range(0, 21, 2))

    # Salva il grafico nell'oggetto BytesIO
    buffer = BytesIO()
    plt.savefig(buffer, format='png')
    buffer.seek(0)
    # Invia il file immagine come risposta
    return send_file(buffer, mimetype='image/png')

def numberExe():
    # Query per ottenere il numero di esercizi per ogni scheda di allenamento
    query = db.session.query(TrainingCard_.name, func.count(ExercisesCards_.id)) \
        .join(ExercisesCards_) \
        .group_by(TrainingCard_.name) \
        .all()

    # Estrai dati dalla query
    schede = [row[0] for row in query]
    num_esercizi = [row[1] for row in query]

    # Genera il grafico
    plt.figure()  # Crea una nuova figura
    plt.bar(schede, num_esercizi)
    plt.xlabel('Scheda di allenamento')
    plt.ylabel('Numero di esercizi')
    plt.title('Numero di esercizi per scheda di allenamento')
    plt.yticks(range(0, 31, 2))

    # Salva il grafico nell'oggetto BytesIO
    buffer1 = BytesIO()
    plt.savefig(buffer1, format='png')
    buffer1.seek(0)

    # Invia il file immagine come risposta
    return send_file(buffer1, mimetype='image/png')

def averageAge():
    # Eseguire la query per ottenere tutte le età degli utenti registrati
    ages = db.session.query(User.age).all()

    # Estrai le età dalla lista di tuple
    ages = [age[0] for age in ages]

    # Calcola la media delle età
    average_age = sum(ages) / len(ages)


    # Genera un grafico della distribuzione delle età
    plt.figure()  # Crea una nuova figura
    plt.hist(ages, bins=10, color='skyblue', edgecolor='black')
    plt.xlabel('Età')
    plt.ylabel('Numero di utenti')
    plt.yticks(range(0, 21, 2))
    plt.title('Distribuzione delle età degli utenti registrati')
    plt.axvline(average_age, color='red', linestyle='dashed', linewidth=1, label='Media delle età')
    plt.legend()
    plt.grid(True)
    plt.xticks(range(10,70 , 3))

    # Salva il grafico nell'oggetto BytesIO
    buffer1 = BytesIO()
    plt.savefig(buffer1, format='png')
    buffer1.seek(0)

    # Invia il file immagine come risposta
    return send_file(buffer1, mimetype='image/png')

def avanzamento(data = None):
    token = data.get('token')
    encoded_token = quote(token)
    decoded_token = jwt.decode(encoded_token, key=SECRET_KEY, algorithms=['HS256'])
    id = decoded_token['user_id']

    data = db.session.query(
        AvanzamentoPeso.data,
        AvanzamentoPeso.peso
    ).filter(AvanzamentoPeso.id_user == id).order_by(AvanzamentoPeso.data).all() #confronto solo quelli dello user interessato

    dates = [row.data for row in data]
    weights = [row.peso for row in data]

    # Configura il grafico
    plt.figure()
    plt.plot(dates, weights, marker='o', linestyle='-', color='b')

    # Aggiungi titoli e etichette
    plt.title('Miglioramento del Peso nel Tempo')
    plt.xlabel('Data')
    plt.ylabel('Peso (kg)')
    plt.grid(True)

    # Salva il grafico nell'oggetto BytesIO
    buffer1 = BytesIO()
    plt.savefig(buffer1, format='png')
    buffer1.seek(0)

    # Invia il file immagine come risposta
    return send_file(buffer1, mimetype='image/png')
