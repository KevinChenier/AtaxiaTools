import os
import subprocess

# Construct the command to run the MATLAB script
command = f"matlab -nodesktop -r upperBodyAnalyzer"

# Execute the command in a subprocess
process = subprocess.Popen(command, shell=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE)
stdout, stderr = process.communicate()

if process.returncode == 0:
    print("MATLAB script started successfully.")
else:
    print("MATLAB script encountered an error.")