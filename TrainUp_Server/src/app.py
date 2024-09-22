from flask import request, Flask
from Favorites import  add_favorite_card, remove_favorite_card
from TrainUp_Server.src.Statistiche import cards_most_used, numberExe, averageAge, avanzamento
from home import home_card_displayer, Load_exercise, LoadCardFromDb
from NewCard import add_exercise, confirm_creation_card, delete_trainingCard, clear_list_exercise
from models.user import db

SECRET_KEY = "mysecretkey"

app = Flask(__name__)
app.config['SECRET_KEY'] = SECRET_KEY
app.config['SQLALCHEMY_DATABASE_URI'] = 'mysql://root:12345@localhost:3309/TrainUp'
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False
app.config['SESSION_TYPE'] = 'filesystem'
db.init_app(app)


######################## NewCard.py #######################

@app.route("/add_exe_card", methods=['GET', 'POST'])
def addExe():
    data = request.get_json()
    return add_exercise(data)

@app.route("/confirm_creation_card", methods=['GET', 'POST'])
def confrim():
    data = request.get_json()
    return confirm_creation_card(data)

@app.route("/delete_trainingCard", methods=['GET', 'POST'])
def delete():
    data = request.get_json()
    return delete_trainingCard(data)

@app.route("/clear_list", methods=['GET', 'POST'])
def clear():
    return clear_list_exercise()


######################## home.py #######################

@app.route("/home", methods=['GET', 'POST'])
def home():
    return home_card_displayer()

@app.route("/LoadCardFromDb", methods=['GET', 'POST'])
def LoadCard():
    data = request.get_json()
    return LoadCardFromDb(data)

@app.route("/loadExer", methods=['GET', 'POST'])
def loadExer():
    return Load_exercise()



######################## Favorites.py #######################

@app.route("/add_favorite_card", methods=['GET', 'POST'])
def addFav():
    data = request.get_json()
    return add_favorite_card(data)

@app.route("/remove_favorite_card", methods=['GET', 'POST'])
def removeFav():
    data = request.get_json()
    return remove_favorite_card(data)


######################## Statistiche.py #######################

@app.route("/cards_most_used", methods=['GET', 'POST'])
def statFav():
    return cards_most_used()

@app.route("/numberExe", methods=['GET', 'POST'])
def num_Exe():
    return numberExe()

@app.route("/ave_age", methods=['GET', 'POST'])
def ave_Age():
    return averageAge()

@app.route("/avanz_peso", methods=['GET', 'POST'])
def ava_Peso():
    data = request.get_json()
    return avanzamento(data)

################################################################

#inizializzazione db
@app.before_request
def init_db():
    with app.app_context():
        db.create_all()
        db.session.commit()


if __name__ == '__main__':
    app.run(host="0.0.0.0", port=5000)