from urllib.parse import quote
from flask import jsonify
from sqlalchemy import func
import logging
import jwt
from home import fitness_app_singleton
from models.user import TrainingCard_, db, ExercisesCards_, User

SECRET_KEY = "mysecretkey"
exe_list = []

#FUNZIONI PER LA CREAZIONE DI UNA NUOVA TRAINING CARD DA PARTE DELL'UTENTE

def add_exercise(data=None):
    token = data.get('token')
    encoded_token = quote(token)
    decoded_token = jwt.decode(encoded_token, key=SECRET_KEY, algorithms=['HS256'])
    id = decoded_token['user_id']

    exe = data.get('dizionario')

    if exe['day'] == '' or exe['name'] == '' or exe['reps'] == 0 or exe['sets']== 0:
        return jsonify({'state': 0})
    day = exe['day']
    name = exe['name']
    reps = int(exe['reps'])
    sets = int(exe['sets'])
    exe_dict = {
        'day': day,
        'name': name,
        'sets': sets,
        'reps': reps
    }
    exe_list.append(exe_dict)
    logging.error(exe_list)
    #lista di appoggio contenente tutti gli esercizi
    logging.error(f"lista temporanea:{exe_list}")
    return jsonify({'state': 1})


def confirm_creation_card(data=None):
    token = data.get('token')
    encoded_token = quote(token)
    decoded_token = jwt.decode(encoded_token, key=SECRET_KEY, algorithms=['HS256'])
    id = decoded_token['user_id']

    if data.get('name') == '':
        return jsonify({'state': 0})
    name_card = data.get('name')

    #ottengo l'ultimo id dal db
    last_id = db.session.query(func.max(TrainingCard_.id)).scalar()
    logging.error(f"id ultima scheda {last_id}")
    # Se non ci sono schede nel database, l'ultimo ID sarà None
    if last_id is None:
        Create_new_card = fitness_app_singleton.create_training_card(user_id=id,card_id=None)

        #aggiungo al db
        _card_ = TrainingCard_(id=Create_new_card.card_id,user_id=id,name=name_card)
        db.session.add(_card_)
        db.session.commit()
    else:
        newCard_id = last_id +1
        Create_new_card = fitness_app_singleton.create_training_card(user_id=id,card_id=newCard_id)
        #aggiungo al db
        _card_ = TrainingCard_(id=Create_new_card.card_id,user_id=id,name=name_card)
        db.session.add(_card_)
        db.session.commit()

    logging.error(f"id nuova scheda: {Create_new_card.card_id}")
    logging.error(f"id user: {Create_new_card.user_id}")

    #aggiungo gli esercizi
    for exe in exe_list:
        fitness_app_singleton.add_exercise_to_card(Create_new_card.card_id, exercise_name=exe['name'], sets=exe['sets'], reps=exe['reps'], day=exe['day'])
        #aggiungo al db
        new_exercise_card = ExercisesCards_(id_train_card= Create_new_card.card_id,name=exe["name"], sets=exe["sets"], reps=exe["reps"], day=exe["day"])
        db.session.add(new_exercise_card)

    db.session.commit()
    #svuoto la lista provvisoria una volta aver aggiunto gli esercizi alla training card
    exe_list.clear()
    logging.error(f"lista temporanea:{exe_list}")
    logging.error(fitness_app_singleton.get_user_training_cards(user_id=id))
    return jsonify({'state': 1})


def delete_trainingCard(data=None):
    token = data.get('token')
    encoded_token = quote(token)
    decoded_token = jwt.decode(encoded_token, key=SECRET_KEY, algorithms=['HS256'])
    id = decoded_token['user_id']

    if id is not None:
        user= db.session.query(User).filter(User.id == id, User.password== data.get('password_')).first()
        if user:
            #Recupero la scheda da eliminare
            scheda_da_elimi = db.session.query(TrainingCard_).filter(TrainingCard_.id == data.get('id_scheda')).first()
            if scheda_da_elimi:
                #recupero tutti gli esercizi di quella scheda
                esercizi_da_elim = db.session.query(ExercisesCards_).filter(ExercisesCards_.id_train_card == data.get('id_scheda')).all()
                for ese in esercizi_da_elim:
                    db.session.delete(ese)

                db.session.delete(scheda_da_elimi)
                db.session.commit()
                return jsonify({'state': 0})
            else: return jsonify({'state': 1})
        else:return jsonify({'state': 1})


#nel caso in cui apro la pagina per creare una nuova scheda e aggiungo
#gli esercizi senza creare la scheda, una volta essere uscito dalla pagina svuto la lista di appogio
#degli esercizi
def clear_list_exercise():
    exe_list.clear()
    logging.error(f"lista temporanea:{exe_list}")
    return jsonify({"state": 0})