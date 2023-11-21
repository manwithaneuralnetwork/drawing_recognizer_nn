import tensorflow as tf
import numpy as np

categories = ["anvil", "book", "door", "hat", "lollipop", "rake"]
x_train = []

for cat in categories:
    data = np.load("dataset/" + cat + ".npy")[:20000, :]
    x_train.append(data)

x_train = np.concatenate(x_train, axis=0)

category_mapping = {cat: i for i, cat in enumerate(categories)}
y_train = np.array([category_mapping[cat] for cat in categories for _ in range(20000)])

indices = np.arange(len(x_train))
np.random.shuffle(indices)
x_train = x_train[indices]
y_train = y_train[indices]

x_train = x_train.reshape(-1, 28, 28)

print(x_train.shape)
print(y_train.shape)

model = tf.keras.models.Sequential()
model.add(tf.keras.layers.Flatten(input_shape=(28, 28)))
model.add(tf.keras.layers.Dense(64, activation='relu'))
model.add(tf.keras.layers.Dense(64, activation='relu'))
model.add(tf.keras.layers.Dense(len(categories), activation='softmax'))

model.compile(optimizer='adam', loss='sparse_categorical_crossentropy', metrics='accuracy')

model.fit(x_train, y_train, epochs=10)

model.save('handwrittendigits.model')