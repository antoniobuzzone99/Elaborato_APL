import logging
from pipes import quote
import jwt
from flask import jsonify
from FitnessApp import FitnessApp
from models.user import Exercise, db, TrainingCard_, ExercisesCards_, cardsFavoreites

SECRET_KEY = "mysecretkey"
fitness_app_singleton = FitnessApp()


def home_card_displayer():
    # caricamento schede statiche
    user_cards = fitness_app_singleton.get_user_training_cards(user_id=1)

    # Controllare se ci sono già elementi con id 1 e 2 nelle schede esistenti
    ids_to_check = {1, 2, 3}
    if not any(card['card_id'] in ids_to_check for card in user_cards):
        # Creare una nuova scheda di allenamento per un utente
        new_card = fitness_app_singleton.create_training_card(user_id=1)
        # Aggiungere esercizi alla scheda di allenamento
        fitness_app_singleton.add_exercise_to_card(new_card.card_id, "Squat", sets=3, reps=5, day="Monday")
        fitness_app_singleton.add_exercise_to_card(new_card.card_id, "Step-up", sets=4, reps=5, day="Monday")
        fitness_app_singleton.add_exercise_to_card(new_card.card_id, "Leg Extension", sets=3, reps=10, day="Monday")
        fitness_app_singleton.add_exercise_to_card(new_card.card_id, "Leg Curl", sets=3, reps=15, day="Monday")
        fitness_app_singleton.add_exercise_to_card(new_card.card_id, "Calf machine", sets=4, reps=20, day="Monday")
        fitness_app_singleton.add_exercise_to_card(new_card.card_id, "Panca piana", sets=5, reps=5, day="Wednesday")
        fitness_app_singleton.add_exercise_to_card(new_card.card_id, "Military-Press", sets=6, reps=6, day="Wednesday")
        fitness_app_singleton.add_exercise_to_card(new_card.card_id, "spinte panca inclinata", sets=4, reps=8,
                                                   day="Wednesday")
        fitness_app_singleton.add_exercise_to_card(new_card.card_id, "Push-down", sets=4, reps=8, day="Wednesday")
        fitness_app_singleton.add_exercise_to_card(new_card.card_id, "Alzate laterali", sets=4, reps=12,
                                                   day="Wednesday")
        fitness_app_singleton.add_exercise_to_card(new_card.card_id, "Stacco da terra", sets=5, reps=3, day="Friday")
        fitness_app_singleton.add_exercise_to_card(new_card.card_id, "Trazioni", sets=5, reps=3, day="Friday")
        fitness_app_singleton.add_exercise_to_card(new_card.card_id, "Lat-machine", sets=4, reps=5, day="Friday")
        fitness_app_singleton.add_exercise_to_card(new_card.card_id, "Pull down", sets=3, reps=12, day="Friday")
        fitness_app_singleton.add_exercise_to_card(new_card.card_id, "Curl con bilancere", sets=4, reps=8, day="Friday")

        new_card1 = fitness_app_singleton.create_training_card(user_id=1)
        fitness_app_singleton.add_exercise_to_card(new_card1.card_id, "Panca piana", sets=4, reps=5, day="Monday")
        fitness_app_singleton.add_exercise_to_card(new_card1.card_id, "Lento dietro", sets=4, reps=6, day="Monday")
        fitness_app_singleton.add_exercise_to_card(new_card1.card_id, "Lat Machine", sets=4, reps=5, day="Monday")
        fitness_app_singleton.add_exercise_to_card(new_card1.card_id, "Pulley basso", sets=4, reps=6, day="Monday")
        fitness_app_singleton.add_exercise_to_card(new_card1.card_id, "Spinte avanti con corda", sets=4, reps=8,
                                                   day="Monday")
        fitness_app_singleton.add_exercise_to_card(new_card1.card_id, "Stacco da terra", sets=5, reps=3,
                                                   day="Wednesday")
        fitness_app_singleton.add_exercise_to_card(new_card1.card_id, "Trazioni", sets=5, reps=3, day="Wednesday")
        fitness_app_singleton.add_exercise_to_card(new_card1.card_id, "Rematore con bilancere", sets=4, reps=6,
                                                   day="Wednesday")
        fitness_app_singleton.add_exercise_to_card(new_card1.card_id, "Pull-down", sets=3, reps=12, day="Wednesday")
        fitness_app_singleton.add_exercise_to_card(new_card1.card_id, "Posteriori a busto flesso", sets=3, reps=12,
                                                   day="Wednesday")
        fitness_app_singleton.add_exercise_to_card(new_card1.card_id, "Squat", sets=5, reps=3, day="Friday")
        fitness_app_singleton.add_exercise_to_card(new_card1.card_id, "Step up", sets=4, reps=5, day="Friday")
        fitness_app_singleton.add_exercise_to_card(new_card1.card_id, "Stacchi a gambe tese", sets=4, reps=6,
                                                   day="Friday")
        fitness_app_singleton.add_exercise_to_card(new_card1.card_id, "Leg extension", sets=3, reps=10, day="Friday")
        fitness_app_singleton.add_exercise_to_card(new_card1.card_id, "Leg Curl", sets=3, reps=10, day="Friday")

        new_card2 = fitness_app_singleton.create_training_card(user_id=1)
        fitness_app_singleton.add_exercise_to_card(new_card2.card_id, "Leg press inclinata", sets=4, reps=8,
                                                   day="Monday")
        fitness_app_singleton.add_exercise_to_card(new_card2.card_id, "Chest press", sets=4, reps=8, day="Monday")
        fitness_app_singleton.add_exercise_to_card(new_card2.card_id, "Squat", sets=4, reps=10, day="Monday")
        fitness_app_singleton.add_exercise_to_card(new_card2.card_id, "Leg Curl", sets=4, reps=8, day="Monday")
        fitness_app_singleton.add_exercise_to_card(new_card2.card_id, "Alzate laterali", sets=3, reps=15, day="Monday")
        fitness_app_singleton.add_exercise_to_card(new_card2.card_id, "Stacco da terra", sets=5, reps=3,
                                                   day="Wednesday")
        fitness_app_singleton.add_exercise_to_card(new_card2.card_id, "Trazioni", sets=5, reps=6, day="Wednesday")
        fitness_app_singleton.add_exercise_to_card(new_card2.card_id, "Lat Machine", sets=4, reps=8, day="Wednesday")
        fitness_app_singleton.add_exercise_to_card(new_card2.card_id, "Curl con bilancere", sets=5, reps=5,
                                                   day="Wednesday")
        fitness_app_singleton.add_exercise_to_card(new_card2.card_id, "Curl ai cavi alti", sets=2, reps=12,
                                                   day="Wednesday")
        fitness_app_singleton.add_exercise_to_card(new_card2.card_id, "Panca piana", sets=4, reps=6, day="Friday")
        fitness_app_singleton.add_exercise_to_card(new_card2.card_id, "Spinte panca inclinata", sets=4, reps=8,
                                                   day="Friday")
        fitness_app_singleton.add_exercise_to_card(new_card2.card_id, "Push-down", sets=3, reps=15, day="Friday")
        fitness_app_singleton.add_exercise_to_card(new_card2.card_id, "Dips", sets=3, reps=15, day="Friday")
        fitness_app_singleton.add_exercise_to_card(new_card2.card_id, "Alzate laterali", sets=4, reps=12, day="Friday")

    updated_user_cards = fitness_app_singleton.get_user_training_cards(user_id=1)
    return jsonify({'user_cards': updated_user_cards})


def Load_exercise():
    # caricicamento esercizi dal db
    exercise_list = []
    new_exercises = Exercise.query.all()
    for exercise in new_exercises:
        exercise_dict = {
            'name': exercise.name,
            'muscle_group': exercise.muscle_group
        }
        exercise_list.append(exercise_dict)
    return jsonify({'exercise_list': exercise_list})


def LoadCardFromDb(data=None):
    #caricamento schede create dagli utenti dal db

    token = data.get('token')
    encoded_token = quote(token)
    decoded_token = jwt.decode(encoded_token, key=SECRET_KEY, algorithms=['HS256'])
    userId = decoded_token['user_id']

    # caricicamento schede
    # Eseguire una query con un join tra le tabelle TrainingCard_ e ExercisesCards_
    query_result = db.session.query(
        TrainingCard_.id,
        TrainingCard_.user_id,
        ExercisesCards_.name.label('exercise_name'),
        ExercisesCards_.sets,
        ExercisesCards_.reps,
        ExercisesCards_.day
    ).join(
        ExercisesCards_,
        TrainingCard_.id == ExercisesCards_.id_train_card
    ).all()

    # Creare un dizionario per memorizzare i risultati
    training_cards_dict = {}

    # Elaborare i risultati della query
    for row in query_result:
        card_id = row.id
        user_id = row.user_id
        exercise = {
            'name': row.exercise_name,
            'sets': row.sets,
            'reps': row.reps,
            'day': row.day
        }

        # Aggiungere le informazioni alla lista di esercizi per questa carta
        if (card_id, user_id) not in training_cards_dict:
            training_cards_dict[(card_id, user_id)] = {
                'card_id': card_id,
                'user_id': user_id,
                'exercises': []
            }
        training_cards_dict[(card_id, user_id)]['exercises'].append(exercise)

    # Convertire il dizionario in una lista
    training_cards_list = list(training_cards_dict.values())

    # restituisco anche i dati relativi alle schede preferite
    favorites_list = []
    favorites_card = cardsFavoreites.query.all()
    logging.error(f"user_id:{userId}")
    for fav in favorites_card:
        if fav.id_utente == userId:
            fav_dict = {
                'user_id': str(fav.id_utente),
                'card_id': str(fav.id_card)
            }
            favorites_list.append(fav_dict)

    #logging.error(f"favorite_list{favorites_list}")
    return jsonify({'user_cards': training_cards_list,'favorites_list': favorites_list})


