from PyTrack.Stimulus import Stimulus
import pandas as pd
import os

# Just a folder that contains the data
# All analysis outputs will be saved inside this folder
dfname = "events.csv"
dfpath = '{}/{}'.format(os.path.dirname(os.path.abspath(__file__)), dfname)

# Read the csv file as a pandas dataframe
df = pd.read_csv(dfpath)
print('Max gaze (height): {:.2f} - Left eye, {:.2f} - Right eye'.format(
    df['GazeLefty'].max(), df['GazeRighty'].max()))
print('Max gaze (width): {:.2f} - Left eye, {:.2f} - Right eye'.format(
    df['GazeLeftx'].max(), df['GazeRightx'].max()))

# Dictionary containing details of recording. Please change the values
# according to your experiment. If no AOI is desired, set aoi value to
# [0, 0, Display_width, Display_height]
# Only the information mentioned in this dictionary is needed, no
# additional information is needed.
sensor_dict = {
    "EyeTracker":
    {
        "Sampling_Freq": 120,
        "Display_width": 100,
        "Display_height": 100,
        "aoi": [0, 0, 100, 100]
    }
}

# Creating Stimulus object (See the documentation for advanced parameters).
stim = Stimulus(path=os.path.dirname(os.path.abspath(__file__)),
               data=df,
               sensor_names=sensor_dict)

# Some functionality usage (See documentation of Stimulus class for advanced use).
stim.findEyeMetaData()

# Getting dictionary of found metadata/features
features = stim.sensors["EyeTracker"].metadata
print(features.keys())

# The keys of the features dictionary contain all the features
# extracted. To get the feature itself, use this command:
# f = features[key]

# Extracting microsaccade features. This will also generate microsaccade
# plots and store them inside the analysis folder
MS, ms_count, ms_duration, ms_vel, ms_amp = stim.findMicrosaccades(plot_ms=True)

# Visualization of plots
stim.gazePlot(save_fig=True)
stim.gazeHeatMap(save_fig=True)
stim.visualize()