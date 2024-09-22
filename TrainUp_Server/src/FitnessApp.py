from TrainingCard import TrainingCard
class FitnessApp:
    #singleton
    _instance = None
    def __new__(cls):
        if not cls._instance:
            cls._instance = super(FitnessApp, cls).__new__(cls)
            cls._instance.training_cards = []

        return cls._instance

    def create_training_card(self, user_id, card_id=None):
        if card_id is None:
            card_id = len(self.training_cards) + 1
        new_card = TrainingCard(card_id, user_id)
        self.training_cards.append(new_card)
        return new_card

    def add_exercise_to_card(self, card_id, exercise_name, sets, reps, day):
        card = next((card for card in self.training_cards if card.card_id == card_id), None)
        if card:
            card.add_exercise(exercise_name, sets, reps, day)
            return True
        else:
            return False

    def get_user_training_cards(self, user_id):
        user_cards = [card.get_card_details() for card in self.training_cards if card.user_id == user_id]
        return user_cards