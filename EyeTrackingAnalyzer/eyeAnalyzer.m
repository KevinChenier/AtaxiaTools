% Start the timer
tic

dirContents = dir('Data');
isDir = [dirContents.isdir];
dirNames = {dirContents(isDir).name};
participants = dirNames(~ismember(dirNames, {'.', '..'}));
tools = ["EyeTrackingFollow"];

for i=1:length(participants)
    for j=1:length(tools)
        analyze(tools{j}, participants{i})
    end
end

% Stop the timer and calculate elapsed time
elapsedTime = toc;
formattedTime = formatTime(elapsedTime);
disp("Elapsed Time: " + formattedTime);

function formattedTime = formatTime(timeInSeconds)
    hours = floor(timeInSeconds / 3600);
    minutes = floor(mod(timeInSeconds, 3600) / 60);
    seconds = mod(timeInSeconds, 60);
    
    formattedTime = sprintf('%02d:%02d:%02d', hours, minutes, seconds);
end

function analyze(arg1, arg2)
    tool = arg1;
    participant = arg2;
    
    disp(tool)
    disp(participant)

    fileToAnalyze = strcat("Data/",participant,"/",tool,'.csv');
    
    T = readtable(fileToAnalyze);
    
    disp(strcat("File to analyze: ", fileToAnalyze));

    %  Salman, Michael. (2008) Square Wave Jerks in Children and Adolescents
    % A square wave jerk was defined as a 
    % horizontal saccade 0.5 degrees in amplitude that was directed away from 
    % the fixation target, followed by a second saccade in the opposite direction 
    % that refixated the target, after a period ranging from 100-500 milliseconds. 
    % The duration of square wave jerks was defined as the time interval
    % between the onset of the error-producing (initial) saccade and the
    % completion of the error-correcting saccade. Peak velocity was the
    % maximum saccadic velocity of each square wave jerk saccade. Square
    % wave jerk saccades were defined as fast horizontal eye movements that
    % had peak velocities 20 degrees/second. The beginnings and ends of
    % saccades were marked when eye velocity crossed 20 degrees/second.
    
    % Anagnostoua, Evangelos. (2020) A Cortical Substrate for Square-Wave Jerks in Progressive Supranuclear Palsy
    % SWJs were defined
    % as a saccade with an amplitude of ≤4°, taking the gaze away
    % from the current position, followed within 300 ms by another
    % saccade with an amplitude similar to the first (difference
    % in amplitude between saccades <30%), which takes the gaze
    % back toward the initial position.18 The onset of a SWJ was the
    % time at which the eye velocity exceeded 10°/s and continued to
    % increase for at least three frames (6 ms).
    
    % Millan et al. (2011) Saccades during Attempted Fixation in Parkinsonian Disorders and Recessive Ataxia: From Microsaccades to Square-Wave Jerks
    % We identified SWJs using the algorithm developed in
    % [12] This method measures the similarity between a given saccade pair (thatis, a pair of consecutive
    % saccades) and ideal SWJ. In an "ideal SWJ" the two saccades are separated by a short interval
    % (i.e. 200ms), have the same magnitudes, and their directions are exactly opposite [12]
    % . We calculated an SWJ index based on the threedefining SWJ characteristics described above:
    % a) the direction dissimilarity of first and second saccade,
    % b) the magnitude similarityof first and second saccade, and
    % c) the temporal proximity of first and second saccade.
    
    timeThreshold = 500;
    velocityThreshold = 20;
    amplitudeThreshold = 0.5;
    
    initialPositionError = 0.35; % HTC Vive accuracy of 0.5 to 1.1 degrees as reported by the company. It has to be limited by amplitude, because the SJW algo would not work properly for initial position go back
    
    % We calculated the speed of movement and differentiated two types of visual behaviors 
    % by setting a threshold at 5 times the median speed and by setting a duration threshold: 
    % saccades (duration less than or equal to 50ms) and fixations (duration greater than 100ms)  
    % (Holmqvist & Andersson, 2017).
    saccadeDurationThreshold = 100;
    fixationDurationThreshold = 150;
    
    %% Table new variables %%
    % New variables init
    newVars = {'Value_LostFocus', 'Value_LeftEyeDirectionDegrees_x', 'Value_LeftEyeDirectionDegrees_y', 'Value_RightEyeDirectionDegrees_x', 'Value_RightEyeDirectionDegrees_y', ...
        'Value_CombinedEyesDirectionDegrees_x', 'Value_CombinedEyesDirectionDegrees_y', 'Value_CombinedEyesVelocityDegrees_x', 'Value_CombinedEyesVelocityDegrees_y', 'Value_CombinedEyesVelocityDegrees'...
        'Value_LeftEyeVelocityDegrees_x', 'Value_LeftEyeVelocityDegrees_y', 'Value_LeftEyeVelocityDegrees', 'Value_RightEyeVelocityDegrees_x', 'Value_RightEyeVelocityDegrees_y', ...
        'Value_RightEyeVelocityDegrees', 'Value_LeftEyeSquareWaveJerk_x', 'Value_LeftEyeSquareWaveJerk_x_amplitude', 'Value_LeftEyeSquareWaveJerk_x_initialVelocity', ...
        'Value_LeftEyeSquareWaveJerk_x_peakVelocity', 'Value_LeftEyeSquareWaveJerk_x_time', 'Value_LeftEyeSquareWaveJerk_y', 'Value_LeftEyeSquareWaveJerk_y_amplitude', ...
        'Value_LeftEyeSquareWaveJerk_y_initialVelocity', 'Value_LeftEyeSquareWaveJerk_y_peakVelocity', 'Value_LeftEyeSquareWaveJerk_y_time', 'Value_RightEyeSquareWaveJerk_x', ...
        'Value_RightEyeSquareWaveJerk_x_amplitude', 'Value_RightEyeSquareWaveJerk_x_initialVelocity', 'Value_RightEyeSquareWaveJerk_x_peakVelocity', 'Value_RightEyeSquareWaveJerk_x_time', ...
        'Value_RightEyeSquareWaveJerk_y', 'Value_RightEyeSquareWaveJerk_y_amplitude', 'Value_RightEyeSquareWaveJerk_y_initialVelocity', 'Value_RightEyeSquareWaveJerk_y_peakVelocity', ...
        'Value_RightEyeSquareWaveJerk_y_time', 'Value_LeftEyeSaccade_x', 'Value_LeftEyeSaccade_x_initialVelocity', 'Value_LeftEyeSaccade_x_peakVelocity', 'Value_LeftEyeSaccade_x_amplitude'...
        'Value_LeftEyeSaccade_y', 'Value_LeftEyeSaccade_y_initialVelocity', 'Value_LeftEyeSaccade_y_peakVelocity', 'Value_LeftEyeSaccade_y_amplitude', 'Value_RightEyeSaccade_x', ...
        'Value_RightEyeSaccade_x_initialVelocity', 'Value_RightEyeSaccade_x_peakVelocity', 'Value_RightEyeSaccade_x_amplitude', 'Value_RightEyeSaccade_y', 'Value_RightEyeSaccade_y_initialVelocity', ...
        'Value_RightEyeSaccade_y_peakVelocity', 'Value_RightEyeSaccade_y_amplitude', 'Value_SquareWaveJerk', 'Value_SquareWaveJerk_amplitude', 'Value_SquareWaveJerk_initialVelocity', ...
        'Value_SquareWaveJerk_peakVelocity', 'Value_SquareWaveJerk_time', 'Value_SquareWaveJerkBinocularity', 'Value_LeftSaccade', 'Value_RightSaccade', ...
        'Value_SaccadeBinocularity', 'Value_Saccade', 'Value_Saccade_initialVelocity', 'Value_Saccade_peakVelocity', 'Value_Saccade_amplitude', 'Value_Fixation'};
    
    % Preallocate a cell array for storing new variables
    newVarData = cell(1, numel(newVars));
    
    % Loop through each new variable
    for i = 1:numel(newVars)
        newVarData{i} = zeros(height(T), 1);
    end

    % Add all new variables to the table at once
    T = addvars(T, newVarData{:}, 'NewVariableNames', newVars);
    disp("Variables initiated");
    % New variables init

    % Lost Focus
    % Vectorized assignment
    T.Value_LostFocus = (T.Value_CombinedEyesGazeDirectionNormalized_x == -1);
    
    % Remove rows using logical indexing
    T(T.Value_LostFocus == 1, :) = [];
    T.Value_LostFocus = [];
    disp("Lost focus done");
    % Lost Focus
    
    % Left Gaze Direction Degrees x
    % Formula source: Imaoka, Yu. (2020). Assessing Saccadic Eye Movements With Head-Mounted Display Virtual Reality Technology
    T.Value_LeftEyeDirectionDegrees_x = atan(T.Value_LeftEyeGazeDirectionNormalized_x ./ T.Value_LeftEyeGazeDirectionNormalized_z) * 180 / pi;
    disp("Left Gaze Direction Degrees x done");
    % Left Gaze Direction Degrees x
    
    % Left Gaze Direction Degrees y
    T.Value_LeftEyeDirectionDegrees_y = atan(T.Value_LeftEyeGazeDirectionNormalized_y ./ T.Value_LeftEyeGazeDirectionNormalized_z) * 180 / pi;
    disp("Left Gaze Direction Degrees y done");
    % Left Gaze Direction Degrees y
    
    % Right Gaze Direction Degrees x
    T.Value_RightEyeDirectionDegrees_x = atan(T.Value_RightEyeGazeDirectionNormalized_x ./ T.Value_RightEyeGazeDirectionNormalized_z) * 180 / pi;
    disp("Right Gaze Direction Degrees x done");
    % Right Gaze Direction Degrees x
    
    % Right Gaze Direction Degrees y
    T.Value_RightEyeDirectionDegrees_y = atan(T.Value_RightEyeGazeDirectionNormalized_y ./ T.Value_RightEyeGazeDirectionNormalized_z) * 180 / pi;
    disp("Right Gaze Direction Degrees y done");
    % Right Gaze Direction Degrees y
    
    % Combined Gaze Direction Degrees x
    T.Value_CombinedEyesDirectionDegrees_x = atan(T.Value_CombinedEyesGazeDirectionNormalized_x ./ T.Value_CombinedEyesGazeDirectionNormalized_z) * 180 / pi;
    disp("Combined Gaze Direction Degrees x done");
    % Combined Gaze Direction Degrees x
    
    % Combined Gaze Direction Degrees y
    T.Value_CombinedEyesDirectionDegrees_y = atan(T.Value_CombinedEyesGazeDirectionNormalized_y ./ T.Value_CombinedEyesGazeDirectionNormalized_z) * 180 / pi;
    disp("Combined Gaze Direction Degrees y done");
    % Combined Gaze Direction Degrees y
    
    % We filter the gaze direction
    % Filter data. Filter source: Imaoka, Yu. (2020). Assessing Saccadic Eye Movements With Head-Mounted Display Virtual Reality Technology
    dataToFilter = ["Value_LeftEyeDirectionDegrees_x", "Value_LeftEyeDirectionDegrees_y", "Value_RightEyeDirectionDegrees_x",...
        "Value_RightEyeDirectionDegrees_y", "Value_CombinedEyesDirectionDegrees_x", "Value_CombinedEyesDirectionDegrees_y"];
    
    for i = 1:length(dataToFilter)
        oldData = T{:, dataToFilter{i}};
        filteredData = movmean(oldData, 10); % Validated moving average filter with a window of 10 with https://goodcalculators.com/simple-moving-average-calculator/
        T{:, dataToFilter{i}} = filteredData;
    end
    disp("Filter done");
    % We filter the gaze direction

    % Velocities
    T = calculateVelocities(T, T.Value_CombinedEyesDirectionDegrees_x, T.Value_ElapsedTime, "Value_CombinedEyesVelocityDegrees_x", "Combined Eyes Velocity Degrees x done");
    T = calculateVelocities(T, T.Value_CombinedEyesDirectionDegrees_y, T.Value_ElapsedTime, "Value_CombinedEyesVelocityDegrees_y", "Combined Eyes Velocity Degrees y done");
    T = calculateVelocities(T, T.Value_LeftEyeDirectionDegrees_x, T.Value_ElapsedTime, "Value_LeftEyeVelocityDegrees_x", "Left Eye Velocity Degrees x done");
    T = calculateVelocities(T, T.Value_LeftEyeDirectionDegrees_y, T.Value_ElapsedTime, "Value_LeftEyeVelocityDegrees_y", "Left Eye Velocity Degrees y done");
    T = calculateVelocities(T, T.Value_RightEyeDirectionDegrees_x, T.Value_ElapsedTime, "Value_RightEyeVelocityDegrees_x", "Left Eye Velocity Degrees x done");
    T = calculateVelocities(T, T.Value_RightEyeDirectionDegrees_y, T.Value_ElapsedTime, "Value_RightEyeVelocityDegrees_y", "Right Eye Velocity Degrees y done");
    
    T = calculateVelocities_united(T, T.Value_CombinedEyesVelocityDegrees_x, T.Value_CombinedEyesVelocityDegrees_y, "Value_CombinedEyesVelocityDegrees", "Combined Eyes Velocity Degrees done");
    T = calculateVelocities_united(T, T.Value_LeftEyeVelocityDegrees_x, T.Value_LeftEyeVelocityDegrees_y, "Value_LeftEyeVelocityDegrees", "Left Eye Velocity Degrees done");
    T = calculateVelocities_united(T, T.Value_RightEyeVelocityDegrees_x, T.Value_RightEyeVelocityDegrees_y, "Value_RightEyeVelocityDegrees", "Right Eye Velocity Degrees done");
    % Velocities

    % "They are also typically accompanied by a brief period of visual
    % suppression, during which visual processing is inhibited, in order to
    % prevent motion blur. It is important to note that changes in pupil size may not 
    % directly reflect changes in visual processing during saccades, as they are 
    % thought to be primarily mediated by changes in arousal and attention."
    % -CHATGPT. 
    % 
    % Maybe we have no way to know for sure if it was a saccade, because we
    % cannot detect visual suppression with HTC Vive Pro Eye. 
    
    %{ 
    % Old way to quantify saccades, we now assume that if there wasn't a SWJ a 
    % certain time, it was a saccade.-
    for i=1:(height(T))
        T{i, "Value_LeftSaccade"} = double((T{i, "Value_LeftEyeVelocityDegrees"} > saccadeVelocityThreshold));
    end 
    % Left Saccade
    
    % Right Saccade
    for i=1:(height(T))
        T{i, "Value_RightSaccade"} = double((T{i, "Value_RightEyeVelocityDegrees"} > saccadeVelocityThreshold));
    end 
    % Right Saccade
    
    % Left Fixation
    for i=1:(height(T))
        T{i, "Value_LeftFixation"} = double(~(T{i, "Value_LeftSaccade"}));
    end 
    % Left Fixation
    
    % Right Fixation
    for i=1:(height(T))
        T{i, "Value_RightFixation"} = double(~(T{i, "Value_RightSaccade"}));
    end 
    % Right Fixation
    %}
    
    % Square Wave Jerks coordinates
    T = squareWaveJerkCoordinateCalculation(T, velocityThreshold, amplitudeThreshold, timeThreshold, initialPositionError, "Value_LeftEyeVelocityDegrees_x", "Value_LeftEyeDirectionDegrees_x", "Value_LeftEyeSquareWaveJerk_x", ...
        "Value_LeftEyeSquareWaveJerk_x_amplitude", "Value_LeftEyeSquareWaveJerk_x_initialVelocity", "Value_LeftEyeSquareWaveJerk_x_peakVelocity", "Value_LeftEyeSquareWaveJerk_x_time");
    
    T = squareWaveJerkCoordinateCalculation(T, velocityThreshold, amplitudeThreshold, timeThreshold, initialPositionError,"Value_LeftEyeVelocityDegrees_y", "Value_LeftEyeDirectionDegrees_y", "Value_LeftEyeSquareWaveJerk_y", ...
        "Value_LeftEyeSquareWaveJerk_y_amplitude", "Value_LeftEyeSquareWaveJerk_y_initialVelocity", "Value_LeftEyeSquareWaveJerk_y_peakVelocity", "Value_LeftEyeSquareWaveJerk_y_time");
    
    T = squareWaveJerkCoordinateCalculation(T, velocityThreshold, amplitudeThreshold, timeThreshold, initialPositionError, "Value_RightEyeVelocityDegrees_x", "Value_RightEyeDirectionDegrees_x", "Value_RightEyeSquareWaveJerk_x", ...
        "Value_RightEyeSquareWaveJerk_x_amplitude", "Value_RightEyeSquareWaveJerk_x_initialVelocity", "Value_RightEyeSquareWaveJerk_x_peakVelocity", "Value_RightEyeSquareWaveJerk_x_time");
    
    T = squareWaveJerkCoordinateCalculation(T, velocityThreshold, amplitudeThreshold, timeThreshold, initialPositionError, "Value_RightEyeVelocityDegrees_y", "Value_RightEyeDirectionDegrees_y", "Value_RightEyeSquareWaveJerk_y", ...
        "Value_RightEyeSquareWaveJerk_y_amplitude", "Value_RightEyeSquareWaveJerk_y_initialVelocity", "Value_RightEyeSquareWaveJerk_y_peakVelocity", "Value_RightEyeSquareWaveJerk_y_time");
    disp("SWJ coordinates done");
    % Square Wave Jerks coordinates
    
    % Square Wave Jerks
    for i=1:(height(T) - 1)
        % Check binocularity
        if(T{i, "Value_LeftEyeSquareWaveJerk_x"} == 1)
            for j=max(i-6,1):(height(T) - 1)
                if(T{j, "Value_RightEyeSquareWaveJerk_x"} == 1)
                    T{i, "Value_SquareWaveJerk"} = 1;
                    T{i, "Value_SquareWaveJerk_amplitude"} = mean(nonzeros([T{i, "Value_LeftEyeSquareWaveJerk_x_amplitude"}, T{j, "Value_RightEyeSquareWaveJerk_x_amplitude"}]));
                    T{i, "Value_SquareWaveJerk_initialVelocity"} = mean(nonzeros([T{i, "Value_LeftEyeSquareWaveJerk_x_initialVelocity"}, T{j, "Value_RightEyeSquareWaveJerk_x_initialVelocity"}]));
                    T{i, "Value_SquareWaveJerk_peakVelocity"} = mean(nonzeros([T{i, "Value_LeftEyeSquareWaveJerk_x_peakVelocity"}, T{j, "Value_RightEyeSquareWaveJerk_x_peakVelocity"}]));
                    T{i, "Value_SquareWaveJerk_time"} = mean(nonzeros([T{i, "Value_LeftEyeSquareWaveJerk_x_time"}, T{j, "Value_RightEyeSquareWaveJerk_x_time"}]));
                    T{i, "Value_SquareWaveJerkBinocularity"} = 1;
                    break;
                end
                if(j >= i + 6)
                    T{i, "Value_SquareWaveJerk"} = 1;
                    T{i, "Value_SquareWaveJerk_amplitude"} = T{i, "Value_LeftEyeSquareWaveJerk_x_amplitude"};
                    T{i, "Value_SquareWaveJerk_initialVelocity"} = T{i, "Value_LeftEyeSquareWaveJerk_x_initialVelocity"};
                    T{i, "Value_SquareWaveJerk_peakVelocity"} = T{i, "Value_LeftEyeSquareWaveJerk_x_peakVelocity"};
                    T{i, "Value_SquareWaveJerk_time"} = T{i, "Value_LeftEyeSquareWaveJerk_x_time"};
                    T{i, "Value_SquareWaveJerkBinocularity"} = 0;
                    break;
                end
            end
        end
    end
    
    for i=1:(height(T) - 1)
        % Check binocularity
        if(T{i, "Value_RightEyeSquareWaveJerk_x"} == 1)
            for j=max(i-6,1):(height(T) - 1)
                
                % Binocularity was already tested, so we only check for right
                % swj solo
                if(T{j, "Value_LeftEyeSquareWaveJerk_x"} == 1)
                    break;
                end
    
                if(j >= i + 6)
                    T{i, "Value_SquareWaveJerk"} = 1;
                    T{i, "Value_SquareWaveJerk_amplitude"} = T{i, "Value_RightEyeSquareWaveJerk_x_amplitude"};
                    T{i, "Value_SquareWaveJerk_initialVelocity"} = T{i, "Value_RightEyeSquareWaveJerk_x_initialVelocity"};
                    T{i, "Value_SquareWaveJerk_peakVelocity"} = T{i, "Value_RightEyeSquareWaveJerk_x_peakVelocity"};
                    T{i, "Value_SquareWaveJerk_time"} = T{i, "Value_RightEyeSquareWaveJerk_x_time"};
                    T{i, "Value_SquareWaveJerkBinocularity"} = 0;
                    break;
                end
            end
        end
    end
    disp("SWJ characteristics done");
    % Square Wave Jerks
    
    % Calculate the median for each participant
    columnData = abs(T.Value_CombinedEyesVelocityDegrees);
    columnDataWithoutNaN = columnData(~isnan(columnData));
    combinedEyesSaccade_median = 10 * median(columnDataWithoutNaN);
    
    % columnData = abs(T.Value_LeftEyeVelocityDegrees_x);
    % columnDataWithoutNaN = columnData(~isnan(columnData));
    % leftEyeSaccade_x_median = 5 * median(columnDataWithoutNaN);
    % 
    % columnData = abs(T.Value_LeftEyeVelocityDegrees_y);
    % columnDataWithoutNaN = columnData(~isnan(columnData));
    % leftEyeSaccade_y_median = 5 * median(columnDataWithoutNaN);
    % 
    % columnData = abs(T.Value_RightEyeVelocityDegrees_x);
    % columnDataWithoutNaN = columnData(~isnan(columnData));
    % rightEyeSaccade_x_median = 5 * median(columnDataWithoutNaN);
    % 
    % columnData = abs(T.Value_RightEyeVelocityDegrees_y);
    % columnDataWithoutNaN = columnData(~isnan(columnData));
    % rightEyeSaccade_y_median = 5 * median(columnDataWithoutNaN);
    % 
    % % Saccades coordinates
    % T = saccadeCoordinateCalculation(T, leftEyeSaccade_x_median, saccadeDurationThreshold, fixationDurationThreshold, "Value_LeftEyeVelocityDegrees_x", "Value_LeftEyeDirectionDegrees_x", ...
    %     "Value_LeftEyeSaccade_x", "Value_LeftEyeSaccade_x_initialVelocity", "Value_LeftEyeSaccade_x_peakVelocity", "Value_LeftEyeSaccade_x_amplitude");
    % T = saccadeCoordinateCalculation(T, leftEyeSaccade_y_median, saccadeDurationThreshold, fixationDurationThreshold, "Value_LeftEyeVelocityDegrees_y", "Value_LeftEyeDirectionDegrees_y", ...
    %     "Value_LeftEyeSaccade_y", "Value_LeftEyeSaccade_y_initialVelocity", "Value_LeftEyeSaccade_y_peakVelocity", "Value_LeftEyeSaccade_y_amplitude");
    % T = saccadeCoordinateCalculation(T, rightEyeSaccade_x_median, saccadeDurationThreshold, fixationDurationThreshold, "Value_RightEyeVelocityDegrees_x", "Value_RightEyeDirectionDegrees_x", ...
    %     "Value_RightEyeSaccade_x", "Value_RightEyeSaccade_x_initialVelocity", "Value_RightEyeSaccade_x_peakVelocity", "Value_RightEyeSaccade_x_amplitude");
    % T = saccadeCoordinateCalculation(T, rightEyeSaccade_y_median, saccadeDurationThreshold, fixationDurationThreshold, "Value_RightEyeVelocityDegrees_y", "Value_RightEyeDirectionDegrees_y", ...
    %     "Value_RightEyeSaccade_y", "Value_RightEyeSaccade_y_initialVelocity", "Value_RightEyeSaccade_y_peakVelocity", "Value_RightEyeSaccade_y_amplitude");
    % % Saccades coordinates
    
    % Saccades coordinates
    T = saccadeCoordinateCalculation(T, combinedEyesSaccade_median, saccadeDurationThreshold, fixationDurationThreshold, "Value_CombinedEyesVelocityDegrees", "Value_CombinedEyesDirectionDegrees_x", ...
        "Value_CombinedEyesDirectionDegrees_y", "Value_Saccade", "Value_Saccade_initialVelocity", "Value_Saccade_peakVelocity", "Value_Saccade_amplitude");
    disp("Saccades coordinates done");
    % Saccades coordinates
    
    % 
    % % Left Saccade
    % for i=1:(height(T) - 1)
    %     if(T{i, "Value_LeftEyeSaccade_x"})
    %         for j=max(i-6,1):(height(T) - 1)
    %             if(T{j, "Value_LeftEyeSaccade_y"})
    %                 T{i, "Value_LeftSaccade"} = 1;
    %                 T{i, "Value_LeftEyeSaccade_initialVelocity"} = sqrt(T{i, "Value_LeftEyeSaccade_x_initialVelocity"}^2 + T{j, "Value_LeftEyeSaccade_y_initialVelocity"}^2);
    %                 T{i, "Value_LeftEyeSaccade_peakVelocity"} = sqrt(T{i, "Value_LeftEyeSaccade_x_peakVelocity"}^2 + T{j, "Value_LeftEyeSaccade_y_peakVelocity"}^2);
    %                 break;
    %             end
    %             if(j >= i + 6)
    %                 T{i, "Value_LeftSaccade"} = 1;
    %                 T{i, "Value_LeftEyeSaccade_initialVelocity"} = T{i, "Value_LeftEyeSaccade_x_initialVelocity"};
    %                 T{i, "Value_LeftEyeSaccade_peakVelocity"} = T{i, "Value_LeftEyeSaccade_x_peakVelocity"};
    %                 break;
    %             end
    %         end
    %     end
    % end
    % 
    % for i=1:(height(T) - 1)
    %     if(T{i, "Value_LeftEyeSaccade_y"})
    %         for j=max(i-6,1):(height(T) - 1)
    %             
    %             % X and Y was already tested, so we only check for right
    %             % solo saccade in y
    %             if(T{j, "Value_LeftEyeSaccade_x"})
    %                 break;
    %             end
    % 
    %             if(j >= i + 6)
    %                 T{i, "Value_LeftSaccade"} = 1;
    %                 T{i, "Value_LeftEyeSaccade_initialVelocity"} = T{i, "Value_LeftEyeSaccade_y_initialVelocity"};
    %                 T{i, "Value_LeftEyeSaccade_peakVelocity"} = T{i, "Value_LeftEyeSaccade_y_peakVelocity"};
    %                 break;
    %             end
    %         end
    %     end
    % end
    % % Left Saccade
    % 
    % % Right Saccade
    % for i=1:(height(T) - 1)
    %     if(T{i, "Value_RightEyeSaccade_x"})
    %         for j=max(i-6,1):(height(T) - 1)
    %             if(T{j, "Value_RightEyeSaccade_y"})
    %                 T{i, "Value_RightSaccade"} = 1;
    %                 T{i, "Value_RightEyeSaccade_initialVelocity"} = sqrt(T{i, "Value_RightEyeSaccade_x_initialVelocity"}^2 + T{j, "Value_RightEyeSaccade_y_initialVelocity"}^2);
    %                 T{i, "Value_RightEyeSaccade_peakVelocity"} = sqrt(T{i, "Value_RightEyeSaccade_x_peakVelocity"}^2 + T{j, "Value_RightEyeSaccade_y_peakVelocity"}^2);
    %                 break;
    %             end
    %             if(j >= i + 6)
    %                 T{i, "Value_RightSaccade"} = 1;
    %                 T{i, "Value_RightEyeSaccade_initialVelocity"} = T{i, "Value_RightEyeSaccade_x_initialVelocity"};
    %                 T{i, "Value_RightEyeSaccade_peakVelocity"} = T{i, "Value_RightEyeSaccade_x_peakVelocity"};
    %                 break;
    %             end
    %         end
    %     end
    % end
    % 
    % for i=1:(height(T) - 1)
    %     if(T{i, "Value_RightEyeSaccade_y"})
    %         for j=max(i-6,1):(height(T) - 1)
    %             
    %             % X and Y was already tested, so we only check for right
    %             % solo saccade in y
    %             if(T{j, "Value_RightEyeSaccade_x"})
    %                 break;
    %             end
    % 
    %             if(j >= i + 6)
    %                 T{i, "Value_RightSaccade"} = 1;
    %                 T{i, "Value_RightEyeSaccade_initialVelocity"} = T{i, "Value_RightEyeSaccade_y_initialVelocity"};
    %                 T{i, "Value_RightEyeSaccade_peakVelocity"} = T{i, "Value_RightEyeSaccade_y_peakVelocity"};
    %                 break;
    %             end
    %         end
    %     end
    % end
    % % Right Saccade
    
    % % Saccades
    % for i=1:(height(T) - 1)
    %     if(T{i, "Value_LeftSaccade"})
    %         for j=max(i-6,1):(height(T) - 1)
    %             if(T{j, "Value_RightSaccade"})
    %                 T{i, "Value_Saccade"} = 1;
    %                 T{i, "Value_Saccade_initialVelocity"} = mean(nonzeros([T{i, "Value_LeftEyeSaccade_initialVelocity"}, T{j, "Value_RightEyeSaccade_initialVelocity"}]));
    %                 T{i, "Value_Saccade_peakVelocity"} = mean(nonzeros([T{i, "Value_LeftEyeSaccade_peakVelocity"}, T{j, "Value_RightEyeSaccade_peakVelocity"}]));
    %                 T{i, "Value_SaccadeBinocularity"} = 1;
    %                 break;
    %             end
    %             if(j >= i + 6)
    %                 T{i, "Value_Saccade"} = 1;
    %                 T{i, "Value_Saccade_initialVelocity"} = T{i, "Value_LeftEyeSaccade_initialVelocity"};
    %                 T{i, "Value_Saccade_peakVelocity"} = T{i, "Value_LeftEyeSaccade_peakVelocity"};
    %                 T{i, "Value_SaccadeBinocularity"} = 0;
    %                 break;
    %             end
    %         end
    %     end
    % end
    % 
    % for i=1:(height(T) - 1)
    %     if(T{i, "Value_RightSaccade"})
    %         for j=max(i-6,1):(height(T) - 1)
    %             
    %             % X and Y was already tested, so we only check for right
    %             % solo saccade in y
    %             if(T{j, "Value_LeftSaccade"})
    %                 break;
    %             end
    % 
    %             if(j >= i + 6)
    %                 T{i, "Value_Saccade"} = 1;
    %                 T{i, "Value_Saccade_initialVelocity"} = T{i, "Value_RightEyeSaccade_initialVelocity"};
    %                 T{i, "Value_Saccade_peakVelocity"} = T{i, "Value_RightEyeSaccade_peakVelocity"};
    %                 T{i, "Value_SaccadeBinocularity"} = 0;
    %                 break;
    %             end
    %         end
    %     end
    % end
    % % Saccades
    
    % % Left Fixation
    % for i=1:(height(T))
    %     T{i, "Value_LeftFixation"} = double(~(T{i, "Value_LeftSaccade"}));
    % end 
    % % Left Fixation
    % 
    % % Right Fixation
    % for i=1:(height(T))
    %     T{i, "Value_RightFixation"} = double(~(T{i, "Value_RightSaccade"}));
    % end 
    % % Right Fixation
    
    % Fixation
    saccadeValue = T.Value_Saccade;
    squareWaveJerkValue = T.Value_SquareWaveJerk;
    fixationValue = ~(saccadeValue | squareWaveJerkValue);
    
    T.Value_Fixation = double(fixationValue);
    disp("Fixation done");
    % Fixation
    
    %% Write in Excel
    
    excelFile = 'RV_Data.xlsx';
    sheet = 'Metrics EyeTracking';
    
    % Read the data from the specified sheet in the Excel file
    sheetData = readcell(excelFile, 'Sheet', sheet);
    
    dataToInsert = {%'Square Wave Jerk Instances', ... 
        %'Square Wave Jerk Amplitude', ...
        %'Square Wave Jerk Initial Velocity', ...
        %'Square Wave Jerk Peak Velocity', ...
        %'Square Wave Jerk Rate', ...
        %'Square Wave Jerk Time', ...
        %'Saccade Instances', ...
        %'Saccade Initial Velocity', ...
        %'Saccade Peak Velocity', ...
        %'Saccade Rate', ...
        %'Saccade Amplitude', ...
        'Velocities'
        };
    
    SquareWaveJerksNonZeroIndices = T.Value_SquareWaveJerk ~= 0;
    SaccadesNonZeroIndices = T.Value_Saccade ~= 0;
    ToolEndedIndices = find(strcmp(T.Value_ToolEnded, 'true'));
    nonNaNIndices = ~isnan(T.Value_CombinedEyesVelocityDegrees);
    
    indexesToRemove = toRemove(length(ToolEndedIndices));
    
    ToolEndedIndices = ToolEndedIndices(setdiff(1:numel(ToolEndedIndices), indexesToRemove));
    
    for i = 1:length(dataToInsert)
        switch dataToInsert{i}
            case 'Square Wave Jerk Instances'
                % Get the indices of the first occurrence of the specific string
                writeInExcel(excelFile, sheet, sheetData, ['Square Wave Jerk Instances', ' ', participant, ' ', tool], sum(T.Value_SquareWaveJerk(SquareWaveJerksNonZeroIndices)));
    
            case 'Square Wave Jerk Amplitude'
                writeInExcel(excelFile, sheet, sheetData, ['Square Wave Jerk Amplitude', ' ', participant, ' ', 'Total', ' ', tool], sum(T.Value_SquareWaveJerk_amplitude(SquareWaveJerksNonZeroIndices)));
                writeInExcel(excelFile, sheet, sheetData, ['Square Wave Jerk Amplitude', ' ', participant, ' ', 'Average', ' ', tool], mean(T.Value_SquareWaveJerk_amplitude(SquareWaveJerksNonZeroIndices)));
                writeInExcel(excelFile, sheet, sheetData, ['Square Wave Jerk Amplitude', ' ', participant, ' ', 'Median', ' ', tool], median(T.Value_SquareWaveJerk_amplitude(SquareWaveJerksNonZeroIndices)));
    
            case 'Square Wave Jerk Initial Velocity'
                writeInExcel(excelFile, sheet, sheetData, ['Square Wave Jerk Initial Velocity', ' ', participant, ' ', 'Total', ' ', tool], sum(T.Value_SquareWaveJerk_initialVelocity(SquareWaveJerksNonZeroIndices)));
                writeInExcel(excelFile, sheet, sheetData, ['Square Wave Jerk Initial Velocity', ' ', participant, ' ', 'Average', ' ', tool], mean(T.Value_SquareWaveJerk_initialVelocity(SquareWaveJerksNonZeroIndices)));
                writeInExcel(excelFile, sheet, sheetData, ['Square Wave Jerk Initial Velocity', ' ', participant, ' ', 'Median', ' ', tool], median(T.Value_SquareWaveJerk_initialVelocity(SquareWaveJerksNonZeroIndices)));
    
            case 'Square Wave Jerk Peak Velocity'
                writeInExcel(excelFile, sheet, sheetData, ['Square Wave Jerk Peak Velocity', ' ', participant, ' ', 'Total', ' ', tool], sum(T.Value_SquareWaveJerk_peakVelocity(SquareWaveJerksNonZeroIndices)));
                writeInExcel(excelFile, sheet, sheetData, ['Square Wave Jerk Peak Velocity', ' ', participant, ' ', 'Average', ' ', tool], mean(T.Value_SquareWaveJerk_peakVelocity(SquareWaveJerksNonZeroIndices)));
                writeInExcel(excelFile, sheet, sheetData, ['Square Wave Jerk Peak Velocity', ' ', participant, ' ', 'Median', ' ', tool], median(T.Value_SquareWaveJerk_peakVelocity(SquareWaveJerksNonZeroIndices)));
    
            case 'Square Wave Jerk Time'
                writeInExcel(excelFile, sheet, sheetData, ['Square Wave Jerk Time', ' ', participant, ' ', 'Total', ' ', tool], sum(T.Value_SquareWaveJerk_time(SquareWaveJerksNonZeroIndices)));
                writeInExcel(excelFile, sheet, sheetData, ['Square Wave Jerk Time', ' ', participant, ' ', 'Average', ' ', tool], mean(T.Value_SquareWaveJerk_time(SquareWaveJerksNonZeroIndices)));
                writeInExcel(excelFile, sheet, sheetData, ['Square Wave Jerk Time', ' ', participant, ' ', 'Median', ' ', tool], median(T.Value_SquareWaveJerk_time(SquareWaveJerksNonZeroIndices)));
    
            case 'Square Wave Jerk Rate'
                writeInExcel(excelFile, sheet, sheetData, ['Square Wave Jerk Rate', ' ', participant, ' ', 'Total time (s)', ' ', tool], sum(T.Value_ElapsedTime(ToolEndedIndices))/1000);
    
            case 'Saccade Instances'
                % Get the indices of the first occurrence of the specific string
                writeInExcel(excelFile, sheet, sheetData, ['Saccade Instances', ' ', participant, ' ', tool], sum(T.Value_Saccade(SaccadesNonZeroIndices)));
    
            case 'Saccade Initial Velocity'
                writeInExcel(excelFile, sheet, sheetData, ['Saccade Initial Velocity', ' ', participant, ' ', 'Total', ' ', tool], sum(T.Value_Saccade_initialVelocity(SaccadesNonZeroIndices)));
                writeInExcel(excelFile, sheet, sheetData, ['Saccade Initial Velocity', ' ', participant, ' ', 'Average', ' ', tool], mean(T.Value_Saccade_initialVelocity(SaccadesNonZeroIndices)));
                writeInExcel(excelFile, sheet, sheetData, ['Saccade Initial Velocity', ' ', participant, ' ', 'Median', ' ', tool], median(T.Value_Saccade_initialVelocity(SaccadesNonZeroIndices)));
    
            case 'Saccade Peak Velocity'
                writeInExcel(excelFile, sheet, sheetData, ['Saccade Peak Velocity', ' ', participant, ' ', 'Total', ' ', tool], sum(T.Value_Saccade_peakVelocity(SaccadesNonZeroIndices)));
                writeInExcel(excelFile, sheet, sheetData, ['Saccade Peak Velocity', ' ', participant, ' ', 'Average', ' ', tool], mean(T.Value_Saccade_peakVelocity(SaccadesNonZeroIndices)));
                writeInExcel(excelFile, sheet, sheetData, ['Saccade Peak Velocity', ' ', participant, ' ', 'Median', ' ', tool], median(T.Value_Saccade_peakVelocity(SaccadesNonZeroIndices)));
    
            case 'Saccade Amplitude'
                writeInExcel(excelFile, sheet, sheetData, ['Saccade Amplitude', ' ', participant, ' ', 'Total', ' ', tool], sum(T.Value_Saccade_amplitude(SaccadesNonZeroIndices)));
                writeInExcel(excelFile, sheet, sheetData, ['Saccade Amplitude', ' ', participant, ' ', 'Average', ' ', tool], mean(T.Value_Saccade_amplitude(SaccadesNonZeroIndices)));
                writeInExcel(excelFile, sheet, sheetData, ['Saccade Amplitude', ' ', participant, ' ', 'Median', ' ', tool], median(T.Value_Saccade_amplitude(SaccadesNonZeroIndices)));
    
            case 'Saccade Rate'
                writeInExcel(excelFile, sheet, sheetData, ['Saccade Rate', ' ', participant, ' ', 'Total time (s)', ' ', tool], sum(T.Value_ElapsedTime(ToolEndedIndices))/1000);
    
            case 'Velocities'
                writeInExcel(excelFile, sheet, sheetData, ['Velocities', ' ', participant, ' ', 'Average', ' ', tool], mean(T.Value_CombinedEyesVelocityDegrees(nonNaNIndices)));
                writeInExcel(excelFile, sheet, sheetData, ['Velocities', ' ', participant, ' ', 'Median', ' ', tool], median(T.Value_CombinedEyesVelocityDegrees(nonNaNIndices)));
                writeInExcel(excelFile, sheet, sheetData, ['Velocities', ' ', participant, ' ', 'Maximum', ' ', tool], max(T.Value_CombinedEyesVelocityDegrees(nonNaNIndices)));
                writeInExcel(excelFile, sheet, sheetData, ['Velocities', ' ', participant, ' ', 'Standard Deviation', ' ', tool], std(T.Value_CombinedEyesVelocityDegrees(nonNaNIndices)));
    
            otherwise
                % Code for handling unrecognized case
                disp('Unrecognized case.');
        end
    end
    
    %% STOP THE SCRIPT
    %error('Stopping script execution.'); % Stop script execution
    return;
    
    %% Graphs %%
    figure(1)
    set(gcf, 'Position', get(0, 'Screensize'));
    % Left Eye Pupil Diameter
    subplot(6, 5, 1);
        x1 = T{:, "Value_ElapsedTime"};
        y1 = T{:, "Value_LeftEyePupilDiameter"};
        title('Left Eye Pupil Diameter');
        xlabel('Time (ms)');
        ylabel('Diameter (mm)');
        h1 = animatedline('Color', 'r');
        ylim([-1 inf])
    % Left Eye Pupil Diameter
    
    % Right Eye Pupil Diameter
    subplot(6, 5, 2);
        x2 = T{:, "Value_ElapsedTime"};
        y2 = T{:, "Value_RightEyePupilDiameter"};
        title('Right Eye Pupil Diameter');
        xlabel('Time (ms)');
        ylabel('Diameter (mm)');
        h2 = animatedline('Color', 'g');
        ylim([-1 inf])
    % Right Eye Pupil Diameter
    
    % Left Eye Openness
    subplot(6, 5, 3);
        x3 = T{:, "Value_ElapsedTime"};
        y3 = T{:, "Value_LeftEyeOpenness"};
        title('Left Eye Openness');
        xlabel('Time (ms)');
        ylabel('Openness');
        h3 = animatedline('Color', 'r');
    % Left Eye Openness
        
    % Right Eye Openness
    subplot(6, 5, 4);
        x4 = T{:, "Value_ElapsedTime"};
        y4 = T{:, "Value_RightEyeOpenness"};
        title('Right Eye Openness');
        xlabel('Time (ms)');
        ylabel('Openness');
        h4 = animatedline('Color', 'g');
    % Right Eye Openness
        
    % Left Eye Velocity Degrees
    subplot(6, 5, 5);
        x5 = T{:, "Value_ElapsedTime"};
        y5 = T{:, "Value_LeftEyeVelocityDegrees"};
        title('Left Eye Velocity');
        xlabel('Time (ms)');
        ylabel('Velocity (Degrees/s)');
        h5 = animatedline('Color', 'r');
    % Left Eye Velocity Degrees
    
    % Right Eye Velocity Degrees
    subplot(6, 5, 6);
        x6 = T{:, "Value_ElapsedTime"};
        y6 = T{:, "Value_RightEyeVelocityDegrees"};
        xlabel('Time (ms)');
        title('Right Eye Velocity');
        ylabel('Velocity (Degrees/s)');
        h6 = animatedline('Color', 'g');
    % Right Eye Velocity Degrees
    
    % Combined Eyes Velocity Degrees
    subplot(6, 5, 7);
        x7 = T{:, "Value_ElapsedTime"};
        y7 = T{:, "Value_CombinedEyesVelocityDegrees"};
        xlabel('Time (ms)');
        title('Combined Eyes Velocity');
        ylabel('Velocity (Degrees/s)');
        h7 = animatedline('Color', 'b');
    % Combined Eyes Velocity Degrees
    
    % Combined Eyes Direction Degrees x
    subplot(6, 5, 8);
        x8 = T{:, "Value_ElapsedTime"};
        y8 = T{:, "Value_CombinedEyesDirectionDegrees_x"};
        title('Combined Eyes Direction Degrees x');
        xlabel('Time (ms)');
        ylabel('Degrees');
        h8 = animatedline('Color', 'g');
    % Combined Eyes Direction Degrees x
    
    % Combined Eyes Direction Degrees y
    subplot(6, 5, 9);
        x9 = T{:, "Value_ElapsedTime"};
        y9 = T{:, "Value_CombinedEyesDirectionDegrees_y"};
        title('Combined Eyes Direction Degrees y');
        xlabel('Time (ms)');
        ylabel('Degrees');
        h9 = animatedline('Color', 'r');
    % Combined Eyes Direction Degrees y
    
    % Fixation
    subplot(6, 5, 10);
        x10 = T{:, "Value_ElapsedTime"};
        y10 = T{:, "Value_Fixation"};
        title('Fixation');
        xlabel('Time (ms)');
        ylabel('Fixation');
        h10 = animatedline('Color', 'b');
    % Fixation
    
    % % Left Saccade
    % subplot(6, 5, 7);
    %     x7 = T{:, "Value_ElapsedTime"};
    %     y7 = T{:, "Value_LeftSaccade"};
    %     title('Left Saccade');
    %     xlabel('Time (ms)');
    %     ylabel('Saccade');
    %     h7 = animatedline('Color', 'r');
    % % Left Saccade
    % 
    % % Right Saccade
    % subplot(6, 5, 8);
    %     x8 = T{:, "Value_ElapsedTime"};
    %     y8 = T{:, "Value_RightSaccade"};
    %     title('Right Saccade');
    %     xlabel('Time (ms)');
    %     ylabel('Saccade');
    %     h8 = animatedline('Color', 'g');
    % % Right Saccade
    
    % % Left Fixation
    % subplot(6, 5, 9);
    %     x9 = T{:, "Value_ElapsedTime"};
    %     y9 = T{:, "Value_LeftFixation"};
    %     title('Left Fixation');
    %     xlabel('Time (ms)');
    %     ylabel('Fixation');
    %     h9 = animatedline('Color', 'r');
    % % Left Fixation
    % 
    % % Right Fixation
    % subplot(6, 5, 10);
    %     x10 = T{:, "Value_ElapsedTime"};
    %     y10 = T{:, "Value_RightFixation"};
    %     title('Right Fixation');
    %     xlabel('Time (ms)');
    %     ylabel('Fixation');
    %     h10 = animatedline('Color', 'g');
    % % Right Fixation
    
    % Left Eye Pupil Position in Sensor Area
        x11 = T{:, "Value_LeftEyePupilPositionInSensorArea_x"};
        y11 = T{:, "Value_LeftEyePupilPositionInSensorArea_y"};
        pts = linspace(0, 1, 70);
        N = histcounts2(y11(:), x11(:), pts, pts);
        
    subplot(6, 5, 11);
        imagesc(pts, pts, N);
        xlabel('x');
        ylabel('y');
        title('Left Eye Pupil Position In Sensor Area');
        axis equal;
        set(gca, 'XLim', pts([1 end]), 'YLim', pts([1 end]), 'YDir', 'normal');
    
    subplot(6, 5, 12);
        xlabel('x');
        ylabel('y');
        title('Left Eye Pupil Position In Sensor Area');
        h11 = animatedline('Color', 'r', 'MaximumNumPoints', 25, 'LineWidth', 3);
        axis equal
        xlim([0 1])
        ylim([0 1])
    % Left Eye Pupil Position in Sensor Area
    
    % Right Eye Pupil Position in Sensor Area
        x12 = T{:, "Value_RightEyePupilPositionInSensorArea_x"};
        y12 = T{:, "Value_RightEyePupilPositionInSensorArea_y"};
        pts = linspace(0, 1, 70);
        N = histcounts2(y12(:), x12(:), pts, pts);
        
    subplot(6, 5, 13);
        imagesc(pts, pts, N);
        xlabel('x');
        ylabel('y');
        title('Right Eye Pupil Position In Sensor Area');
        axis equal;
        set(gca, 'XLim', pts([1 end]), 'YLim', pts([1 end]), 'YDir', 'normal');
    
    subplot(6, 5, 14);
        xlabel('x');
        ylabel('y');
        title('Right Eye Pupil Position In Sensor Area');
        h12 = animatedline('Color', 'g', 'MaximumNumPoints', 25, 'LineWidth', 3);
        axis equal
        xlim([0 1])
        ylim([0 1])
    % Right Eye Pupil Position in Sensor Area
    
    % Combined Eyes Gaze Directions Normalized
        x13 = T{:, "Value_CombinedEyesGazeDirectionNormalized_x"};
        y13 = T{:, "Value_CombinedEyesGazeDirectionNormalized_y"};
        pts = linspace(-0.99, 1, 70);
        N = histcounts2(y13(:), x13(:), pts, pts);
        
    subplot(6, 5, 15);
        imagesc(pts, pts, N);
        xlabel('x');
        ylabel('y');
        title('Combined Eyes Gaze Direction');
        axis equal;
        set(gca, 'XLim', pts([1 end]), 'YLim', pts([1 end]), 'YDir', 'normal');
    
    subplot(6, 5, 16);
        xlabel('x');
        ylabel('y');
        title('Combined Eyes Gaze Direction');
        h13 = animatedline('Color', 'b', 'MaximumNumPoints', 25, 'LineWidth', 3);
        axis equal
        xlim([-1 1])
        ylim([-1 1])
    % Combined Eyes Gaze Directions Normalized
    
    % Left Eye Direction Degrees x
    subplot(6, 5, 17);
        x14 = T{:, "Value_ElapsedTime"};
        y14 = T{:, "Value_LeftEyeDirectionDegrees_x"};
        title('Left Eye Direction Degrees X');
        xlabel('Time (ms)');
        ylabel('Degrees');
        h14 = animatedline('Color', 'r');
    % Left Eye Direction Degrees x
    
    % Right Eye Direction Degrees x
    subplot(6, 5, 18);
        x15 = T{:, "Value_ElapsedTime"};
        y15 = T{:, "Value_RightEyeDirectionDegrees_x"};
        title('Right Eye Direction Degrees X');
        xlabel('Time (ms)');
        ylabel('Degrees');
        h15 = animatedline('Color', 'g');
    % Right Eye Direction Degrees x
    
    % Left Eye Direction Degrees y
    subplot(6, 5, 19);
        x16 = T{:, "Value_ElapsedTime"};
        y16 = T{:, "Value_LeftEyeDirectionDegrees_y"};
        title('Left Eye Direction Degrees Y');
        xlabel('Time (ms)');
        ylabel('Degrees');
        h16 = animatedline('Color', 'r');
    % Left Eye Direction Degrees y
    
    % Right Eye Direction Degrees y
    subplot(6, 5, 20);
        x17 = T{:, "Value_ElapsedTime"};
        y17 = T{:, "Value_RightEyeDirectionDegrees_y"};
        title('Right Eye Direction Degrees Y');
        xlabel('Time (ms)');
        ylabel('Degrees');
        h17 = animatedline('Color', 'g');
    % Right Eye Direction Degrees y
    
    % Left Eye Horizontal Square Wave Jerk
    subplot(6, 5, 22);
        x18 = T{:, "Value_ElapsedTime"};
        y18 = T{:, "Value_LeftEyeSquareWaveJerk_x"};
        title('Left Eye Horizontal Square Wave Jerk');
        xlabel('Time (ms)');
        ylabel('SWJ');
        h18 = animatedline('Color', 'r');
    % Left Eye Horizontal Square Wave Jerk
    
    % Right Eye Horizontal Square Wave Jerk
    subplot(6, 5, 23);
        x19 = T{:, "Value_ElapsedTime"};
        y19 = T{:, "Value_RightEyeSquareWaveJerk_x"};
        title('Right Eye Horizontal Square Wave Jerk');
        xlabel('Time (ms)');
        ylabel('SWJ');
        h19 = animatedline('Color', 'g');
    % Right Eye Horizontal Square Wave Jerk
    
    % Left Eye Vertical Square Wave Jerk
    subplot(6, 5, 24);
        x20 = T{:, "Value_ElapsedTime"};
        y20 = T{:, "Value_LeftEyeSquareWaveJerk_y"};
        title('Left Eye Vertical Square Wave Jerk');
        xlabel('Time (ms)');
        ylabel('SWJ');
        h20 = animatedline('Color', 'r');
    % Left Eye Vertical Square Wave Jerk
    
    % Right Eye Vertical Square Wave Jerk
    subplot(6, 5, 25);
        x21 = T{:, "Value_ElapsedTime"};
        y21 = T{:, "Value_RightEyeSquareWaveJerk_y"};
        title('Right Eye Vertical Square Wave Jerk');
        xlabel('Time (ms)');
        ylabel('SWJ');
        h21 = animatedline('Color', 'g');
    % Right Eye Vertical Square Wave Jerk
    
    % Square Wave Jerk
    subplot(6, 5, 26);
        x22 = T{:, "Value_ElapsedTime"};
        y22 = T{:, "Value_SquareWaveJerk"};
        title('Square Wave Jerk');
        xlabel('Time (ms)');
        ylabel('SWJ');
        h22 = animatedline('Color', 'b');
    % Square Wave Jerk
    
    % Saccade
    subplot(6, 5, 27);
        x23 = T{:, "Value_ElapsedTime"};
        y23 = T{:, "Value_Saccade"};
        title('Saccade');
        xlabel('Time (ms)');
        ylabel('Saccade');
        h23 = animatedline('Color', 'b');
    % Saccade
    
    %% Animation
    video = VideoWriter(strcat(tool,'.avi'), 'MPEG-4');
    open(video)
    
    a = tic; % start timer
    for i = 1:length(x9)
        addpoints(h1, x1(i), y1(i));
        addpoints(h2, x2(i), y2(i));
        addpoints(h3, x3(i), y3(i));
        addpoints(h4, x4(i), y4(i));
        addpoints(h5, x5(i), y5(i));
        addpoints(h6, x6(i), y6(i));
        addpoints(h7, x7(i), y7(i));
        addpoints(h8, x8(i), y8(i));
        addpoints(h9, x9(i), y9(i));
        addpoints(h10, x10(i), y10(i));
    
        if (x11(i) ~= -1)
            addpoints(h11, x11(i), y11(i));
        end
        if (x12(i) ~= -1)
            addpoints(h12, x12(i), y12(i));
        end
        if (x13(i) ~= -1)
            addpoints(h13, x13(i), y13(i));
        end
        if (x14(i) ~= -1)
            addpoints(h14, x14(i), y14(i));
        end
        if (x15(i) ~= -1)
            addpoints(h15, x15(i), y15(i));
        end
        if (x16(i) ~= -1)
            addpoints(h16, x16(i), y16(i));
        end
        
        addpoints(h17, x17(i), y17(i));
        addpoints(h18, x18(i), y18(i));
        addpoints(h19, x19(i), y19(i));
        addpoints(h20, x20(i), y20(i));
        addpoints(h21, x21(i), y21(i));
        addpoints(h22, x22(i), y22(i));
        addpoints(h23, x23(i), y23(i));
    
        if(T{i, "Value_ToolEnded"} == "true" || T{i, "Value_ToolEnded"} == "TRUE")
            clearpoints(h1);
            clearpoints(h2);
            clearpoints(h3);
            clearpoints(h4);
            clearpoints(h5);
            clearpoints(h6);
            clearpoints(h7);
            clearpoints(h8);
            clearpoints(h9);
            clearpoints(h10);
            clearpoints(h11);
            clearpoints(h12);
            clearpoints(h13);
            clearpoints(h14);
            clearpoints(h15);
            clearpoints(h16);
            clearpoints(h17);
            clearpoints(h18);
            clearpoints(h19);
            clearpoints(h20);
            clearpoints(h21);
            clearpoints(h22);
            clearpoints(h23);
        end
    
        b = toc(a); % check timer
        if b > (1/200)
            F = getframe(gcf);
            writeVideo(video, F);
            drawnow % update screen every 1/200 seconds
            a = tic; % reset timer after updating
        end
    end
    close(video)

    function newTableWithVelocities = calculateVelocities(T, CombinedEyeDirectionDegrees, ElapsedTime, VelocityDegrees, Message)
        % Calculate the time differences
        VelocityTime = max(diff(ElapsedTime), 1);
        
        % Calculate the velocities
        Velocities = diff(CombinedEyeDirectionDegrees) ./ VelocityTime;
        
        % Convert velocities to degrees per second
        Velocities = Velocities * 1000;
        
        % Assign velocities to the output column
        T{:, VelocityDegrees}(2:end) = Velocities;
        disp(Message);

        newTableWithVelocities = T;
    end

    function newTableWithVelocities_united = calculateVelocities_united(T, EyeVelocityDegrees_x, EyeVelocityDegrees_y, EyeVelocityDegrees, Message)
        Velocity = sqrt(EyeVelocityDegrees_x.^2 + EyeVelocityDegrees_y.^2);
        
        T{:, EyeVelocityDegrees} = Velocity;
        
        disp(Message);

        newTableWithVelocities_united = T;
     end
    
    function newTableWithSaccades = saccadeCoordinateCalculation(T, velocityThreshold, saccadeDurationThreshold, fixationDurationThreshold, Value_EyeVelocityDegrees, Value_EyeDirectionDegrees_x, Value_EyeDirectionDegrees_y, Value_Saccade, ...
        Value_Saccade_initialVelocity, Value_Saccade_peakVelocity, Value_Saccade_amplitude)
    
        breakNestedLoopFlag = false;
        peakAmplitude = 0;
        peakVelocity = 0;
        
        skipIndex = 1;
    
        for i=1:(height(T) - 1)
            if(i < skipIndex)
                continue
            end
            if (abs(T{i, Value_EyeVelocityDegrees}) >= velocityThreshold)
                peakAmplitude = 0;
                initialTime = T{i-1, "Value_ElapsedTime"};
                initialPosition = [T{i-1, Value_EyeDirectionDegrees_x}, T{i-1, Value_EyeDirectionDegrees_y}];
                initialVelocity = abs(T{i, Value_EyeVelocityDegrees});
                peakVelocity = initialVelocity;
                for k=i+1:(height(T) - 1)
                    currentPosition = [T{k, Value_EyeDirectionDegrees_x}, T{k, Value_EyeDirectionDegrees_y}];
                    currentVelocity = abs(T{k, Value_EyeVelocityDegrees});
                    peakVelocity = max(currentVelocity, peakVelocity);
                    peakAmplitude = max(norm(initialPosition - currentPosition), peakAmplitude);
                    % TODO: Calculate the duration of saccade
                    if((T{k, "Value_ElapsedTime"} - initialTime) < saccadeDurationThreshold && abs(T{k, Value_EyeVelocityDegrees}) < velocityThreshold)
                        %regarder pour fixation pendant 100ms
                        for j=k+1:(height(T) - 1)
                            if((T{j, "Value_ElapsedTime"} - initialTime) < fixationDurationThreshold && abs(T{j, Value_EyeVelocityDegrees}) < velocityThreshold)
                                continue;
                            elseif (abs(T{j, Value_EyeVelocityDegrees}) >= velocityThreshold)
                                skipIndex=j;
                                breakNestedLoopFlag = true;
                            elseif((T{j, "Value_ElapsedTime"} - initialTime) >= fixationDurationThreshold)
                                T{i, Value_Saccade} = 1;
                                T{i, Value_Saccade_initialVelocity} = initialVelocity;
                                T{i, Value_Saccade_peakVelocity} = peakVelocity;
                                T{i, Value_Saccade_amplitude} = peakAmplitude;
                                skipIndex=j;
                                breakNestedLoopFlag = true;
                            end
                            if(breakNestedLoopFlag)
                                break;
                            end
                        end
                    elseif ((T{k, "Value_ElapsedTime"} - initialTime) >= saccadeDurationThreshold)
                        skipIndex=k;
                        breakNestedLoopFlag = true;
                        break;
                    end
                    if(breakNestedLoopFlag)
                        breakNestedLoopFlag = false;
                        break;
                    end
                end
            end
        end
        newTableWithSaccades = T;
    end
    
    function newTableWithSWJ = squareWaveJerkCoordinateCalculation(T, velocityThreshold, amplitudeThreshold, timeThreshold, initialPositionError, Value_EyeVelocityDegrees, Value_EyeDirectionDegrees, Value_SquareWaveJerk, ...
        Value_SquareWaveJerk_amplitude, Value_SquareWaveJerk_initialVelocity, Value_SquareWaveJerk_peakVelocity, Value_SquareWaveJerk_time)
        
        breakNestedLoopFlag = false;
        peakAmplitude = 0;
        peakVelocity = 0;
        
        skipIndex = 1;
    
        for i=1:(height(T) - 1)
            if(i < skipIndex)
                continue
            end
            if (abs(T{i, Value_EyeVelocityDegrees}) >= velocityThreshold)
                peakAmplitude = 0;
                initialTime = T{i-1, "Value_ElapsedTime"};
                initialPosition = T{i-1, Value_EyeDirectionDegrees};
                initialVelocity = abs(T{i, Value_EyeVelocityDegrees});
                peakVelocity = initialVelocity;
                for k=i+1:(height(T) - 1)
                    currentPosition = T{k, Value_EyeDirectionDegrees};
                    currentVelocity = abs(T{k, Value_EyeVelocityDegrees});
                    peakVelocity = max(currentVelocity, peakVelocity);
                    peakAmplitude = max(abs(initialPosition - currentPosition), peakAmplitude);
                    if(abs(initialPosition - currentPosition) > amplitudeThreshold)
                        for j=k+1:(height(T) - 1)
                            currentPosition = T{j, Value_EyeDirectionDegrees};
                            currentVelocity = abs(T{j, Value_EyeVelocityDegrees});
                            peakVelocity = max(currentVelocity, peakVelocity);
                            peakAmplitude = max(abs(initialPosition - currentPosition), peakAmplitude);
                            if (currentPosition >= (initialPosition - initialPositionError) && currentPosition <= (initialPosition + initialPositionError))
                                for l=j+1:(height(T) - 1)
                                    currentPosition = T{l, Value_EyeDirectionDegrees};
                                    if (currentPosition >= (initialPosition - initialPositionError) && currentPosition <= (initialPosition + initialPositionError) && l < j+6)
                                        continue
                                    elseif (l >= j+6)
                                        T{i, Value_SquareWaveJerk} = 0.5;
                                        T{j, Value_SquareWaveJerk} = 1;
                                        T{j, Value_SquareWaveJerk_amplitude} = peakAmplitude;
                                        T{j, Value_SquareWaveJerk_initialVelocity} = initialVelocity;
                                        T{j, Value_SquareWaveJerk_peakVelocity} = peakVelocity;
                                        T{j, Value_SquareWaveJerk_time} = abs(T{j, "Value_ElapsedTime"} - initialTime); 
                                        skipIndex=j;
                                        breakNestedLoopFlag = true;
                                        break;
                                    else
                                        % T{i, Value_EyeSaccade} = 1;
                                        % T{i, Value_EyeSaccade_initialVelocity} = initialVelocity;
                                        % T{i, Value_EyeSaccade_peakVelocity} = peakVelocity;
                                        skipIndex=j;
                                        breakNestedLoopFlag = true;
                                        break
                                    end
                                end
                                if(breakNestedLoopFlag)
                                    break;
                                end
                            elseif ((T{j, "Value_ElapsedTime"} - initialTime) > timeThreshold)
                                % T{i, Value_EyeSaccade} = 1;
                                % T{i, Value_EyeSaccade_initialVelocity} = initialVelocity;
                                % T{i, Value_EyeSaccade_peakVelocity} = peakVelocity;
                                skipIndex=j;
                                breakNestedLoopFlag = true;
                                break;
                            end
                        end
                    elseif ((T{k, "Value_ElapsedTime"} - initialTime) > timeThreshold)
                        % T{i, Value_EyeSaccade} = 1;
                        % T{i, Value_EyeSaccade_initialVelocity} = initialVelocity;
                        % T{i, Value_EyeSaccade_peakVelocity} = peakVelocity;
                        skipIndex=k;
                        break;
                    end
                    if(breakNestedLoopFlag)
                        breakNestedLoopFlag = false;
                        break;
                    end
                end
            end
        end
        newTableWithSWJ = T;
    end
    
    function colLetter = xlscol(column)
        letters = 'A':'Z';
        base = 26;
    
        if column <= base
            colLetter = letters(column);
        else
            colLetter = [xlscol(ceil(column / base) - 1) xlscol(mod(column - 1, base) + 1)];
        end
    end
    
    function writeInExcel(excelFile, sheet, sheetData, stringToFind, data)
        % Get the indices of the first occurrence of the specific string
        [row, col] = find(strcmp(sheetData, stringToFind));
        
        columnLetter = xlscol(col);  % Convert column index to letter
        
        % Write the data to the specified sheet and cell range
        cellReference = [columnLetter num2str(row)];  % Combine column letter and row number
        writematrix(data, excelFile, 'Sheet', sheet, 'Range', cellReference);

        %cellReference = [col, row];
        %command = sprintf('python write_excel.py "%s" "%s" "%s" "%s"', excelFile, sheet, mat2str(cellReference), num2str(data));
        %system(command);
        
        disp(strcat(stringToFind, " ", num2str(data)));
    end
    
    function indexes = toRemove(length)
        array = [];
        for i = 2:2:length
            array = [array, i];
        end
        indexes = array;
    end
end