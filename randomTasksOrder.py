import random

tasks_MQ2 = ["Pouring", "FingerNose", "FingerFollow", "Rhythm", "Vibration", "EyeContrast", "SpeechPerception", "Pegboard"]
tasks_HTC = ["Fixation", "FollowTarget", "MultipleTargetsPattern"]

tasks_MQ2 = random.sample(tasks_MQ2, len(tasks_MQ2))
tasks_HTC = random.sample(tasks_HTC, len(tasks_HTC))

print("Tasks order for MQ2: " + str(tasks_MQ2));
print("Tasks order for HTC: " + str(tasks_HTC));