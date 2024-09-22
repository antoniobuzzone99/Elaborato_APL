from flask_sqlalchemy import SQLAlchemy
from sqlalchemy.orm import relationship

db = SQLAlchemy()

class User(db.Model):
    __tablename__ = 'user'
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.Text, nullable=False)
    password = db.Column(db.Text, nullable=False)
    age = db.Column(db.Integer, nullable=False)
    weight = db.Column(db.Integer, nullable=False)

class Exercise(db.Model):
    __tablename__ = 'exercise'
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.Text, nullable=False)
    muscle_group = db.Column(db.Text, nullable=False)

class TrainingCard_(db.Model):
    __tablename__ = 'training_card'
    id = db.Column(db.Integer, primary_key=True)
    user_id = db.Column(db.Integer, db.ForeignKey('user.id'), nullable=False)
    name = db.Column(db.Text, nullable=False)

    user = relationship('User')
class ExercisesCards_(db.Model):
    __tablename__ = 'exercises_cards'

    id = db.Column(db.Integer, primary_key=True)
    id_train_card = db.Column(db.Integer, db.ForeignKey('training_card.id'), nullable=False)
    name = db.Column(db.Text)
    sets = db.Column(db.Integer)
    reps = db.Column(db.Integer)
    day = db.Column(db.Text)

    training_card = relationship('TrainingCard_')


class cardsFavoreites(db.Model):
    __tablename__ = 'cards_favorites'
    id_favorite = db.Column(db.Integer, primary_key=True)
    id_utente = db.Column(db.Integer,db.ForeignKey('user.id'), nullable=False)
    id_card = db.Column(db.Integer,db.ForeignKey('training_card.id'), nullable=False)

    user = relationship('User')
    training_card = relationship('TrainingCard_')

class AvanzamentoPeso(db.Model):
    __tablename__ = 'avanzamentopeso'

    id = db.Column(db.Integer, primary_key=True)
    id_user = db.Column(db.Integer, db.ForeignKey('user.id'), nullable=False)
    peso = db.Column(db.Integer, nullable=False)
    data = db.Column(db.Date, nullable=False)

    # Relazione con la tabella User
    user = relationship('User')