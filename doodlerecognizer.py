import tensorflow as tf
import numpy as np
import json
import time

categories = ["anvil", "book", "door", "hat", "lollipop", "rake"]

location = "C:/Users/dyrek/OneDrive/Documents/Code Projects/Doodle Neural Network/"
drawingPath = location + "drawing.json"

modelPath = location + "doodlebrain.model"
model = tf.keras.models.load_model(modelPath)

prevDrawing = None
while True:
    try:
        with open(drawingPath, 'r') as file:
            data = json.load(file)
    except (json.decoder.JSONDecodeError, PermissionError):
        time.sleep(0.05)
        continue

    npDrawing = np.array(data.get("drawing"))
    npDrawing = npDrawing.reshape(-1, 28, 28)

    if np.array_equal(prevDrawing, npDrawing) or np.max(npDrawing) < 1:
        continue

    prevDrawing = npDrawing

    predictions = model.predict(npDrawing)

    sortedIndices = np.argsort(predictions.flatten())[::-1]

    sortedPredictions = predictions.flatten()[sortedIndices]
    sortedCategories = np.array(categories)[sortedIndices]

    resultDict = {category: float(value) for category, value in zip(sortedCategories, sortedPredictions)}

    with open(location + "results.json", 'w') as results:
        json.dump(resultDict, results)

    time.sleep(0.05)