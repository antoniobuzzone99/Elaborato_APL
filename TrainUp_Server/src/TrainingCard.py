class TrainingCard:
    def __init__(self, card_id, user_id):
        self.card_id = card_id
        self.user_id = user_id
        self.exercises = []

    def add_exercise(self, exercise_name, sets, reps, day):
        exercise = {"name": exercise_name, "sets": sets, "reps": reps, "day": day}
        self.exercises.append(exercise)

    def get_card_details(self):
        return {
            "card_id": self.card_id,
            "user_id": self.user_id,
            "exercises": self.exercises
        }

