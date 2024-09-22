from pipes import quote
import jwt
from flask import jsonify
from models.user import db, cardsFavoreites

SECRET_KEY = "mysecretkey"

def add_favorite_card(data=None):
    token = data.get('token')
    encoded_token = quote(token)
    decoded_token = jwt.decode(encoded_token, key=SECRET_KEY, algorithms=['HS256'])
    user_id = decoded_token['user_id']
    cardId = data.get('trainingCrad_id')

    cardFavorite = cardsFavoreites(id_utente=user_id, id_card=cardId)
    db.session.add(cardFavorite)
    db.session.commit()

    return jsonify({"state": 0})

def remove_favorite_card(data=None):
    token = data.get('token')
    encoded_token = quote(token)
    decoded_token = jwt.decode(encoded_token, key=SECRET_KEY, algorithms=['HS256'])
    userId = decoded_token['user_id']
    cardId = data.get('trainingCrad_id')

    card_da_elimi = db.session.query(cardsFavoreites).filter(cardsFavoreites.id_utente == userId, cardsFavoreites.id_card == cardId).first()
    db.session.delete(card_da_elimi)
    db.session.commit()

    return jsonify({"state": 0})
