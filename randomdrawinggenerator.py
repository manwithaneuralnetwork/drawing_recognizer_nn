import json
import random

location = "C:/Users/dyrek/OneDrive/Documents/Code Projects/Doodle Neural Network/"
drawingPath = location + "drawing.json"

drawing = [random.randint(0, 255) for _ in range(784)]

with open(drawingPath, 'r') as file:
    data = json.load(file)

data["drawing"] = drawing

with open(drawingPath, 'w') as file:
    json.dump(data, file)