import os
import subprocess

# TODO: the script here doesn't connect to the matlab script, so we have no way of knowing when the matlab script has ended. Instead we will do it in matlab. Use eyeAnalyzer.py instead.

tools = ["EyeTrackingFix", "EyeTrackingMultiple"]
#participants = ["CHUM-RV-001", "CHUM-RV-003"]
participants = [name for name in os.listdir("Data") if os.path.isdir(os.path.join("Data", name))]

for participant in participants:
    for tool in tools:

        # Construct the command to run the MATLAB script
        command = f"matlab -nodesktop -r \"eyeAnalyzer('{tool}', '{participant}');\""

        # Execute the command in a subprocess
        process = subprocess.Popen(command, shell=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE)
        stdout, stderr = process.communicate()

        if process.returncode == 0:
            print("MATLAB script started successfully.")
        else:
            print("MATLAB script encountered an error.")
