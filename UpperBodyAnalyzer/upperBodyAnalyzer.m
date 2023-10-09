% Start the timer
tic

dirContents = dir('Data');
isDir = [dirContents.isdir];
dirNames = {dirContents(isDir).name};
participants = dirNames(~ismember(dirNames, {'.', '..'}));
tools = ["FingerFollow", "Pegboard", "FingerNose", "EverydayTask"];

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

    if ~exist(fileToAnalyze, 'file') || participant == "CHUM-RV-016"
        disp(strcat("File: ", fileToAnalyze, " doesn't exist, skipping..."));
        return;
    end
    
    T = readtable(fileToAnalyze);
    
    disp(strcat("File to analyze: ", fileToAnalyze));
   
    format long
    
    %% Table new variables %%
    % Left Controller Speed
    for i=1:(height(T))
        if(i + 2) > height(T)
            break;
        end
        if (T{i, "Value_Type"} == "LeftControllerPosition" && T{i + 2, "Value_Type"} == "LeftControllerPosition")
            x1 = T{i, "Value_x"};
            y1 = T{i, "Value_y"};
            z1 = T{i, "Value_z"};
    
            Time1 = T{i, "Value_ElapsedTime"} / 1000;
    
            x2 = T{i + 2, "Value_x"};
            y2 = T{i + 2, "Value_y"};
            z2 = T{i + 2, "Value_z"};
    
            Time2 = T{i + 2, "Value_ElapsedTime"} / 1000;
    
            if((Time2 - Time1) == 0)
                T{i + 2, "Value_LeftControllerSpeed"} = 0;
                continue
            end
    
            Distance = sqrt((x2 - x1)^2 + (y2 - y1)^2 + (z2- z1)^2);
            Speed = Distance / (Time2 - Time1);
    
            T{i + 2, "Value_LeftControllerSpeed"} = abs(Speed);
        end
    end
    % Left Controller Speed
    
    disp("Left Controller Speed done");
    
    % Right Controller Speed
    for i=1:(height(T))
        if(i + 2) > height(T)
            break;
        end
        if (T{i, "Value_Type"} == "RightControllerPosition" && T{i + 2, "Value_Type"} == "RightControllerPosition")
            x1 = T{i, "Value_x"};
            y1 = T{i, "Value_y"};
            z1 = T{i, "Value_z"};
    
            Time1 = T{i, "Value_ElapsedTime"} / 1000;
    
            x2 = T{i + 2, "Value_x"};
            y2 = T{i + 2, "Value_y"};
            z2 = T{i + 2, "Value_z"};
    
            Time2 = T{i + 2, "Value_ElapsedTime"} / 1000;
    
            if((Time2 - Time1) == 0)
                T{i + 2, "Value_RightControllerSpeed"} = 0;
                continue
            end
    
            Distance = sqrt((x2 - x1)^2 + (y2 - y1)^2 + (z2- z1)^2);
            Speed = Distance / (Time2 - Time1);
    
            T{i + 2, "Value_RightControllerSpeed"} = abs(Speed);
        end
    end
    % Right Controller Speed
    
    disp("Right Controller Speed done");
    
    % Left Controller Acceleration
    for i=1:(height(T))
        if(i + 2) > height(T)
            break;
        end
        if (T{i, "Value_Type"} == "LeftControllerPosition" && T{i + 2, "Value_Type"} == "LeftControllerPosition")
            speed1 = T{i, "Value_LeftControllerSpeed"};
            speed2 = T{i + 2, "Value_LeftControllerSpeed"}; 
    
            Time1 = T{i, "Value_ElapsedTime"} / 1000;
            Time2 = T{i + 2, "Value_ElapsedTime"} / 1000;
    
            if((Time2 - Time1) == 0)
                T{i + 2, "Value_LeftControllerAcceleration"} = 0;
                continue
            end
    
            Acceleration = (speed2 - speed1) / (Time2 - Time1);
    
            T{i + 2, "Value_LeftControllerAcceleration"} = abs(Acceleration);
        end
    end
    % Left Controller Acceleration
    
    disp("Left Controller Acceleration done");
    
    % Right Controller Acceleration
    for i=1:(height(T))
        if(i + 2) > height(T)
            break;
        end
        if (T{i, "Value_Type"} == "RightControllerPosition" && T{i + 2, "Value_Type"} == "RightControllerPosition")
            speed1 = T{i, "Value_RightControllerSpeed"};
            speed2 = T{i + 2, "Value_RightControllerSpeed"}; 
    
            Time1 = T{i, "Value_ElapsedTime"} / 1000;
            Time2 = T{i + 2, "Value_ElapsedTime"} / 1000;
    
            if((Time2 - Time1) == 0)
                T{i + 2, "Value_RightControllerAcceleration"} = 0;
                continue
            end
    
            Acceleration = (speed2 - speed1) / (Time2 - Time1);
    
            T{i + 2, "Value_RightControllerAcceleration"} = abs(Acceleration);
        end
    end
    % Right Controller Acceleration
    
    disp("Right Controller Acceleration done");
    
    % Left Controller Jerk
    for i=1:(height(T))
        if(i + 2) > height(T)
            break;
        end
        if (T{i, "Value_Type"} == "LeftControllerPosition" && T{i + 2, "Value_Type"} == "LeftControllerPosition")
            acceleration1 = T{i, "Value_LeftControllerAcceleration"};
            acceleration2 = T{i + 2, "Value_LeftControllerAcceleration"}; 
    
            Time1 = T{i, "Value_ElapsedTime"} / 1000;
            Time2 = T{i + 2, "Value_ElapsedTime"} / 1000;
    
            if((Time2 - Time1) == 0)
                T{i + 2, "Value_LeftControllerJerk"} = 0;
                continue
            end
    
            Jerk = (acceleration2 - acceleration1) / (Time2 - Time1);
    
            T{i + 2, "Value_LeftControllerJerk"} = abs(Jerk);
        end
    end
    % Left Controller Jerk
    
    disp("Left Controller Jerk done");
    
    % Right Controller Jerk
    for i=1:(height(T))
        if(i + 2) > height(T)
            break;
        end
        if (T{i, "Value_Type"} == "RightControllerPosition" && T{i + 2, "Value_Type"} == "RightControllerPosition")
            acceleration1 = T{i, "Value_RightControllerAcceleration"};
            acceleration2 = T{i + 2, "Value_RightControllerAcceleration"}; 
    
            Time1 = T{i, "Value_ElapsedTime"} / 1000;
            Time2 = T{i + 2, "Value_ElapsedTime"} / 1000;
    
            if((Time2 - Time1) == 0)
                T{i + 2, "Value_RightControllerJerk"} = 0;
                continue
            end
    
            Jerk = (acceleration2 - acceleration1) / (Time2 - Time1);
    
            T{i + 2, "Value_RightControllerJerk"} = abs(Jerk);
        end
    end
    % Right Controller Jerk
    
    disp("Right Controller Jerk done");

    skipRMSE = true;
    
    if (ismember("Value_TrajectoryRepetitionStart", T.Properties.VariableNames) && ismember("Value_TrajectoryRepetitionEnd", T.Properties.VariableNames) && ~skipRMSE)
       
        % Trajectory Start and End Estimation
        % Find the rows in the table where Value.TrajectoryRepetitionStart and Value.TrajectoryRepetitionEnd exist
        idx = ~isnan(T.Value_TrajectoryRepetitionStart) & T.Value_Type == "RightControllerPosition";
        
        % Save the data from the selected rows to a new table called TrajectoryTable
        TrajectoryTable = T(idx, :);
        % Trajectory Start and End Estimation
        
        % Try number
            TryNumber = 1;
            for i=1:(height(TrajectoryTable))
                TrajectoryTable{i, "Value_TryNumber"} = TryNumber;
        
                if(TrajectoryTable{i, "Value_TrajectoryRepetitionStart"} == 10)
                    TryNumber = TryNumber + 1;
                end
            end
        % Try number
            
        %RMSE Estimation
        currentTrajectoryRepetitionStart_p1 = [];
        currentTrajectoryRepetitionStart_p2 = [];
        currentTrajectoryRepetitionVector = [];
        for i=1:(height(T))
            if(i + 2) > height(T)
                break;
            end
            
            if(~isempty(currentTrajectoryRepetitionVector) && T{i, "Value_Type"} == "RightControllerPosition")
                point = [T{i, "Value_x"} T{i, "Value_y"} T{i, "Value_z"}];
                
                % Compute the vector from p1 to 3D point
                computedVector = point - currentTrajectoryRepetitionStart_p1;
        
                % Project computed Vector onto currentTrajectoryRepetitionVector to get the distance between 3D point and the nearest point on the line
                d = dot(computedVector, currentTrajectoryRepetitionVector);
        
                % Compute the coordinates of the nearest point on the line
                nearestPoint = currentTrajectoryRepetitionStart_p1 + d*currentTrajectoryRepetitionVector;
        
                % Compute the distance between 3D point and nearest point
                dist = norm(point - nearestPoint);
        
                % Compute the RMSE of 3D point in relation to nearest point
                rmse = sqrt(dist^2);
        
                T{i, "Value_RMSE"} = rmse;
            end
        
            % We change the vector for RMSE calculations
            if (~ismissing(T{i, "Value_TrajectoryRepetitionStart"}) && T{i, "Value_Type"} == "RightControllerPosition")
                
                % Reset variables when we ended the try
                if(T{i, "Value_TrajectoryRepetitionStart"} == 10)
                    currentTrajectoryRepetitionStart_p1 = [];
                    currentTrajectoryRepetitionStart_p2 = [];
                    currentTrajectoryRepetitionVector = [];
                    continue;
                end
        
                currentTrajectoryRepetitionStart = T{i, "Value_TrajectoryRepetitionStart"};
        
                currentTrajectoryRepetitionStartRow = find(TrajectoryTable.Value_ElapsedTime == T{i, "Value_ElapsedTime"} & TrajectoryTable.Value_TrajectoryRepetitionStart == currentTrajectoryRepetitionStart & TrajectoryTable.Value_x == T{i, "Value_x"});
                currentTrajectoryRepetitionEndRow = find(TrajectoryTable.Value_TrajectoryRepetitionEnd == currentTrajectoryRepetitionStart & TrajectoryTable.Value_TryNumber == TrajectoryTable{currentTrajectoryRepetitionStartRow, "Value_TryNumber"});
        
                if (~isempty(currentTrajectoryRepetitionStartRow) && ~isempty(currentTrajectoryRepetitionEndRow))
                    currentTrajectoryRepetitionStart_p1 = [TrajectoryTable{currentTrajectoryRepetitionStartRow, "Value_x"} TrajectoryTable{currentTrajectoryRepetitionStartRow, "Value_y"} TrajectoryTable{currentTrajectoryRepetitionStartRow, "Value_z"}];
                    currentTrajectoryRepetitionStart_p2 = [TrajectoryTable{currentTrajectoryRepetitionEndRow, "Value_x"} TrajectoryTable{currentTrajectoryRepetitionEndRow, "Value_y"} TrajectoryTable{currentTrajectoryRepetitionEndRow, "Value_z"}];
                    
                    % Compute the direction vector of the line segment
                    currentTrajectoryRepetitionVector = (currentTrajectoryRepetitionStart_p2 - currentTrajectoryRepetitionStart_p1) / norm(currentTrajectoryRepetitionStart_p2 - currentTrajectoryRepetitionStart_p1);
                end
            end
                
        end
        %RMSE Estimation
    end

    disp("RMSE done");
    
    %% Write in Excel
        
        excelFile = 'RV_Data.xlsx';
        sheet = 'Metrics';
        
        % Read the data from the specified sheet in the Excel file
        sheetData = readcell(excelFile, 'Sheet', sheet);
        
        dataToInsert = {'Speed', ... 
            'Acceleration', ...
            'Jerk'
            };
        
        for i = 1:length(dataToInsert)
            switch dataToInsert{i}    
                case 'Speed'
                    if ismember("Value_LeftControllerSpeed", T.Properties.VariableNames)
                        LeftControllerSpeedNonZeroIndices = T.Value_LeftControllerSpeed ~= 0;
                        writeInExcel(excelFile, sheet, sheetData, ['Left Controller Speed', ' ', participant, ' ', 'Total', ' ', tool], sum(T.Value_LeftControllerSpeed(LeftControllerSpeedNonZeroIndices)));
                        writeInExcel(excelFile, sheet, sheetData, ['Left Controller Speed', ' ', participant, ' ', 'Average', ' ', tool], mean(T.Value_LeftControllerSpeed(LeftControllerSpeedNonZeroIndices)));
                        writeInExcel(excelFile, sheet, sheetData, ['Left Controller Speed', ' ', participant, ' ', 'Median', ' ', tool], median(T.Value_LeftControllerSpeed(LeftControllerSpeedNonZeroIndices)));
                        writeInExcel(excelFile, sheet, sheetData, ['Left Controller Speed', ' ', participant, ' ', 'Standard Deviation', ' ', tool], std(T.Value_LeftControllerSpeed(LeftControllerSpeedNonZeroIndices)));
                    end
    
                    RightControllerSpeedNonZeroIndices = T.Value_RightControllerSpeed ~= 0;
                    writeInExcel(excelFile, sheet, sheetData, ['Right Controller Speed', ' ', participant, ' ', 'Total', ' ', tool], sum(T.Value_RightControllerSpeed(RightControllerSpeedNonZeroIndices)));
                    writeInExcel(excelFile, sheet, sheetData, ['Right Controller Speed', ' ', participant, ' ', 'Average', ' ', tool], mean(T.Value_RightControllerSpeed(RightControllerSpeedNonZeroIndices)));
                    writeInExcel(excelFile, sheet, sheetData, ['Right Controller Speed', ' ', participant, ' ', 'Median', ' ', tool], median(T.Value_RightControllerSpeed(RightControllerSpeedNonZeroIndices)));
                    writeInExcel(excelFile, sheet, sheetData, ['Right Controller Speed', ' ', participant, ' ', 'Standard Deviation', ' ', tool], std(T.Value_RightControllerSpeed(RightControllerSpeedNonZeroIndices)));
    
                case 'Acceleration'
                    if ismember("Value_LeftControllerAcceleration", T.Properties.VariableNames)
                        LeftControllerAccelerationNonZeroIndices = T.Value_LeftControllerAcceleration ~= 0;
                        writeInExcel(excelFile, sheet, sheetData, ['Left Controller Acceleration', ' ', participant, ' ', 'Total', ' ', tool], sum(T.Value_LeftControllerAcceleration(LeftControllerAccelerationNonZeroIndices)));
                        writeInExcel(excelFile, sheet, sheetData, ['Left Controller Acceleration', ' ', participant, ' ', 'Average', ' ', tool], mean(T.Value_LeftControllerAcceleration(LeftControllerAccelerationNonZeroIndices)));
                        writeInExcel(excelFile, sheet, sheetData, ['Left Controller Acceleration', ' ', participant, ' ', 'Median', ' ', tool], median(T.Value_LeftControllerAcceleration(LeftControllerAccelerationNonZeroIndices)));
                        writeInExcel(excelFile, sheet, sheetData, ['Left Controller Acceleration', ' ', participant, ' ', 'Standard Deviation', ' ', tool], std(T.Value_LeftControllerAcceleration(LeftControllerAccelerationNonZeroIndices)));
                    end
    
                    RightControllerAccelerationNonZeroIndices = T.Value_RightControllerAcceleration ~= 0;
                    writeInExcel(excelFile, sheet, sheetData, ['Right Controller Acceleration', ' ', participant, ' ', 'Total', ' ', tool], sum(T.Value_RightControllerAcceleration(RightControllerAccelerationNonZeroIndices)));
                    writeInExcel(excelFile, sheet, sheetData, ['Right Controller Acceleration', ' ', participant, ' ', 'Average', ' ', tool], mean(T.Value_RightControllerAcceleration(RightControllerAccelerationNonZeroIndices)));
                    writeInExcel(excelFile, sheet, sheetData, ['Right Controller Acceleration', ' ', participant, ' ', 'Median', ' ', tool], median(T.Value_RightControllerAcceleration(RightControllerAccelerationNonZeroIndices)));
                    writeInExcel(excelFile, sheet, sheetData, ['Right Controller Acceleration', ' ', participant, ' ', 'Standard Deviation', ' ', tool], std(T.Value_RightControllerAcceleration(RightControllerAccelerationNonZeroIndices)));
        
                case 'Jerk'
                    if ismember("Value_LeftControllerJerk", T.Properties.VariableNames)
                        LeftControllerJerkNonZeroIndices = T.Value_LeftControllerJerk ~= 0;
                        writeInExcel(excelFile, sheet, sheetData, ['Left Controller Jerk', ' ', participant, ' ', 'Total', ' ', tool], sum(T.Value_LeftControllerJerk(LeftControllerJerkNonZeroIndices)));
                        writeInExcel(excelFile, sheet, sheetData, ['Left Controller Jerk', ' ', participant, ' ', 'Average', ' ', tool], mean(T.Value_LeftControllerJerk(LeftControllerJerkNonZeroIndices)));
                        writeInExcel(excelFile, sheet, sheetData, ['Left Controller Jerk', ' ', participant, ' ', 'Median', ' ', tool], median(T.Value_LeftControllerJerk(LeftControllerJerkNonZeroIndices)));
                        writeInExcel(excelFile, sheet, sheetData, ['Left Controller Jerk', ' ', participant, ' ', 'Standard Deviation', ' ', tool], std(T.Value_LeftControllerJerk(LeftControllerJerkNonZeroIndices)));
                        writeInExcel(excelFile, sheet, sheetData, ['Left Controller Jerk', ' ', participant, ' ', 'High jerks', ' ', tool], sum(T.Value_LeftControllerJerk(LeftControllerJerkNonZeroIndices) > 1000));
                        writeInExcel(excelFile, sheet, sheetData, ['Left Controller Jerk', ' ', participant, ' ', 'Moderate jerks', ' ', tool], sum(T.Value_LeftControllerJerk(LeftControllerJerkNonZeroIndices) >= 500 & T.Value_LeftControllerJerk(LeftControllerJerkNonZeroIndices) <= 1000));
                        writeInExcel(excelFile, sheet, sheetData, ['Left Controller Jerk', ' ', participant, ' ', 'Low jerks', ' ', tool], sum(T.Value_LeftControllerJerk(LeftControllerJerkNonZeroIndices) < 500));
                    end

                    RightControllerJerkNonZeroIndices = T.Value_RightControllerJerk ~= 0;
                    writeInExcel(excelFile, sheet, sheetData, ['Right Controller Jerk', ' ', participant, ' ', 'Total', ' ', tool], sum(T.Value_RightControllerJerk(RightControllerJerkNonZeroIndices)));
                    writeInExcel(excelFile, sheet, sheetData, ['Right Controller Jerk', ' ', participant, ' ', 'Average', ' ', tool], mean(T.Value_RightControllerJerk(RightControllerJerkNonZeroIndices)));
                    writeInExcel(excelFile, sheet, sheetData, ['Right Controller Jerk', ' ', participant, ' ', 'Median', ' ', tool], median(T.Value_RightControllerJerk(RightControllerJerkNonZeroIndices)));
                    writeInExcel(excelFile, sheet, sheetData, ['Right Controller Jerk', ' ', participant, ' ', 'Standard Deviation', ' ', tool], std(T.Value_RightControllerJerk(RightControllerJerkNonZeroIndices)));
                    writeInExcel(excelFile, sheet, sheetData, ['Right Controller Jerk', ' ', participant, ' ', 'High jerks', ' ', tool], sum(T.Value_RightControllerJerk(RightControllerJerkNonZeroIndices) > 1000));
                    writeInExcel(excelFile, sheet, sheetData, ['Right Controller Jerk', ' ', participant, ' ', 'Moderate jerks', ' ', tool], sum(T.Value_RightControllerJerk(RightControllerJerkNonZeroIndices) >= 500 & T.Value_RightControllerJerk(RightControllerJerkNonZeroIndices) <= 1000));
                    writeInExcel(excelFile, sheet, sheetData, ['Right Controller Jerk', ' ', participant, ' ', 'Low jerks', ' ', tool], sum(T.Value_RightControllerJerk(RightControllerJerkNonZeroIndices) < 500));
    
                otherwise
                    % Code for handling unrecognized case
                    disp('Unrecognized case.');
            end
        end
        
        %% STOP THE SCRIPT
        %error('Stopping script execution.'); % Stop script execution
        return;
    
    %% Graphs %%
    scale_factor = 1;
    
    left_obj = read_wobj('left_quest.obj');
    left_obj_vertices = left_obj.vertices;
    center = mean(left_obj_vertices, 1);
    left_obj_vertices = (left_obj_vertices - center) * scale_factor + center;
    left_obj_faces = left_obj.objects(4).data.vertices;
    
    right_obj = read_wobj('right_quest.obj');
    right_obj_vertices = right_obj.vertices;
    center = mean(right_obj_vertices, 1);
    right_obj_vertices = (right_obj_vertices - center) * scale_factor + center;
    right_obj_faces = right_obj.objects(4).data.vertices;

    figure(1)
    
    set(gcf, 'WindowState', 'normal', 'Position', get(0, 'ScreenSize')* 9/10);
    
    type = T{:, "Value_Type"};
    
    % Controller Positions
    x1 = T{:, "Value_x"};
    y1 = T{:, "Value_y"};
    z1 = T{:, "Value_z"};
    
    x2 = T{:, "Value_x"};
    y2 = T{:, "Value_y"};
    z2 = T{:, "Value_z"};
    
    subplot(2,2,1);
        title('Controller Positions 3D');
        h1 = animatedline('Color', 'r', 'LineWidth', 1, 'LineStyle','--');
        h2 = animatedline('Color', 'g', 'LineWidth', 1, 'LineStyle','--');
        h12 = animatedline('Color', 'black', 'LineWidth', 1, 'Marker','x', MarkerSize=10, MaximumNumPoints=1);
        h22 = animatedline('Color', 'black', 'LineWidth', 1, 'Marker','x', MarkerSize=10, MaximumNumPoints=1);
        h13 = animatedline('Color', 'black', 'LineWidth', 1, 'Marker','*', MarkerSize=10, MaximumNumPoints=1);
        h23 = animatedline('Color', 'black', 'LineWidth', 1, 'Marker','*', MarkerSize=10, MaximumNumPoints=1);
        set(gca, 'XLim', [-1.5 1.5], 'YLim', [-1.5 1.5], 'ZLim', [-1.5 1.5]);
        grid on
        view(43,24);
        xlabel('x');
        ylabel('y');
        zlabel('z');
    
    subplot(2,2,2);
        title('Controller Positions 2D');
        h3 = animatedline('Color', 'r', 'LineWidth', 1, 'LineStyle','--');
        h4 = animatedline('Color', 'g', 'LineWidth', 1, 'LineStyle','--');
        h32 = animatedline('Color', 'black', 'LineWidth', 1, 'Marker','x', MarkerSize=10, MaximumNumPoints=1);
        h42 = animatedline('Color', 'black', 'LineWidth', 1, 'Marker','x', MarkerSize=10, MaximumNumPoints=1);
        h33 = animatedline('Color', 'black', 'LineWidth', 1, 'Marker','*', MarkerSize=10, MaximumNumPoints=1);
        h43 = animatedline('Color', 'black', 'LineWidth', 1, 'Marker','*', MarkerSize=10, MaximumNumPoints=1);
        set(gca, 'XLim', [-1.5 1.5], 'YLim', [-1.5 1.5]);
        xlabel('x');
        ylabel('z');
        % Controller Positions
    
    subplot(2,2,3);
    if ismember("Value_LeftControllerSpeed", T.Properties.VariableNames)
        title('Left Controller Speed');
        x3 = T{:, "Value_ElapsedTime"};
        y3 = T{:, "Value_LeftControllerSpeed"};
        title('Left controller speed');
        xlabel('Time (ms)');
        ylabel('Speed (m/s)');
        h5 = animatedline('Color', 'r');
    end
    
    subplot(2,2,4);
        title('Right Controller Speed');
        x4 = T{:, "Value_ElapsedTime"};
        y4 = T{:, "Value_RightControllerSpeed"};
        title('Right controller speed');
        xlabel('Time (ms)');
        ylabel('Speed (m/s)');
        h6 = animatedline('Color', 'g');
    
    %% Animation %%
    video = VideoWriter(strcat(s,'.avi'), 'Uncompressed AVI');
    video.FrameRate = 30;
    open(video)
    
    a1 = [0 0 0];
    line1 = [];
    arrow1 = [];
    
    a2 = [0 0 0];
    line2 = [];
    arrow2 = [];
    
    a3 = [0 0];
    line3 = [];
    arrow3 = [];
    
    a4 = [0 0];
    line4 = [];
    arrow4 = [];
    
    lastElapsedTime = 0;
    minSpeed = 0.05;
    
    a = tic; % start timer
    for i = 1:length(type)
        if(strcmp(type(i), 'LeftControllerPosition'))
            addpoints(h1, x1(i), z1(i), y1(i));
            addpoints(h12, x1(i), z1(i), y1(i));
            subplot(2,2,1);
            hold on
    
            if exist('left_patch', 'var')
                delete(left_patch);
            end
    
            % Calculate the centroid of the object
            centroid = mean(left_obj_vertices);
            
            % Define the desired point to center the object at
            center_point = [x1(i), z1(i), y1(i)];
            
            % Calculate the translation vector to move the centroid to the center point
            translation = center_point - centroid;
            
            % Apply the translation to the object vertices
            left_obj_vertices = left_obj_vertices + repmat(translation, size(left_obj_vertices, 1), 1);
            
            % Create the patch object using the vertices and faces arrays
            left_patch = patch('Faces', left_obj_faces, 'Vertices', left_obj_vertices, 'FaceColor', 'white');
    
            if(a1 == [0 0 0])
                a1 = [x1(i) z1(i) y1(i)];
            else
                delete([line1, arrow1]);
                clearpoints(h13);
                if(T{i, "Value_LeftControllerSpeed"} > minSpeed)
                    directionVector = [x1(i) z1(i) y1(i)] - a1;
                    initial_point = [x1(i) z1(i) y1(i)];
                    terminal_point = initial_point + 0.5 * directionVector/norm(directionVector);
                    [line1, arrow1] = vectarrow(initial_point, terminal_point,1,'black');
                    addpoints(h13, terminal_point(1), terminal_point(2), terminal_point(3));
                    a1 = [x1(i) z1(i) y1(i)];
                end
            end
            hold off
        end
    
        if(strcmp(type(i), 'RightControllerPosition'))
            addpoints(h2, x2(i), z2(i), y2(i));
            addpoints(h22, x2(i), z2(i), y2(i));
            subplot(2,2,1);
            hold on
    
            if exist('right_patch', 'var')
                delete(right_patch);
            end
    
            % Calculate the centroid of the object
            centroid = mean(right_obj_vertices);
            
            % Define the desired point to center the object at
            center_point = [x1(i), z1(i), y1(i)];
            
            % Calculate the translation vector to move the centroid to the center point
            translation = center_point - centroid;
            
            % Apply the translation to the object vertices
            right_obj_vertices = right_obj_vertices + repmat(translation, size(right_obj_vertices, 1), 1);
            
            % Create the patch object using the vertices and faces arrays
            right_patch = patch('Faces', right_obj_faces, 'Vertices', right_obj_vertices, 'FaceColor', 'white');
    
    
            if(a2 == [0 0 0])
                a2 = [x2(i) z2(i) y2(i)];
            else
                delete([line2, arrow2]);
                clearpoints(h23);
                if(T{i, "Value_RightControllerSpeed"} > minSpeed)
                    directionVector = [x2(i) z2(i) y2(i)] - a2;
                    initial_point = [x2(i) z2(i) y2(i)];
                    terminal_point = initial_point + 0.5 * directionVector/norm(directionVector);
                    [line2, arrow2] = vectarrow(initial_point, terminal_point,1,'black');
                    addpoints(h23, terminal_point(1), terminal_point(2), terminal_point(3));
                    a2 = [x2(i) z2(i) y2(i)];
                end
            end
            hold off
        end
    
        if(strcmp(type(i), 'LeftControllerPosition'))
            addpoints(h3, x1(i), y1(i));
            addpoints(h32, x1(i), y1(i));
            subplot(2,2,2);
            hold on
            if(a3 == [0 0])
                a3 = [x1(i) y1(i)];
            else
                delete([line3, arrow3]);
                clearpoints(h33);
                if(T{i, "Value_LeftControllerSpeed"} > minSpeed)
                    directionVector = [x1(i) y1(i)] - a3;
                    initial_point = [x1(i) y1(i)];
                    terminal_point = initial_point + 0.5 * directionVector/norm(directionVector);
                    [line3, arrow3] = vectarrow(initial_point, terminal_point,1,'black');
                    addpoints(h33, terminal_point(1), terminal_point(2));
                    a3 = [x1(i) y1(i)];
                end
            end
            hold off
        end
    
        if(strcmp(type(i), 'RightControllerPosition'))
            addpoints(h4, x2(i), y2(i));
            addpoints(h42, x2(i), y2(i));
            subplot(2,2,2);
            hold on
            if(a4 == [0 0])
                a4 = [x2(i) y2(i)];
            else
                delete([line4, arrow4]);
                clearpoints(h43);
                if(T{i, "Value_RightControllerSpeed"} > minSpeed)
                    directionVector = [x2(i) y2(i)] - a4;
                    initial_point = [x2(i) y2(i)];
                    terminal_point = initial_point + 0.5 * directionVector/norm(directionVector);
                    [line4, arrow4] = vectarrow(initial_point, terminal_point,1,'black');
                    addpoints(h43, terminal_point(1), terminal_point(2));
                    a4 = [x2(i) y2(i)];
                end
            end
            hold off
        end
    
        if ismember("Value_LeftControllerSpeed", T.Properties.VariableNames)
            if(strcmp(type(i), 'LeftControllerPosition') && y3(i) ~= 0)
                addpoints(h5, x3(i), y3(i));
            end
        end
    
        if(strcmp(type(i), 'RightControllerPosition') && y4(i) ~= 0)
            addpoints(h6, x4(i), y4(i));
        end
    
        [x, y, z] = getpoints(h1); % Call getpoints with three output arguments
        if(((T{i, "Value_ToolEnded"} == "FALSE" || T{i, "Value_ToolEnded"} == "false") && ~isempty(x)) || T{i, "Value_ToolEnded"} == "true" || T{i, "Value_ToolEnded"} == "TRUE")
            if exist('left_patch', 'var')
                delete(left_patch);
            end
            if exist('right_patch', 'var')
                delete(right_patch);
            end
    
            if ismember("Value_LeftControllerSpeed", T.Properties.VariableNames)
                [x,y,z] = getpoints(h1);
                clearpoints(h1);
                subplot(2,2,1);
                hold on
                h1 = plot3(x, y, z, 'r', 'LineWidth', 2);
                % modified jet-colormap
                cd = [uint8(jet(length(x))*255) uint8(ones(length(x),1))].';
                drawnow;
                set(h1.Edge, 'ColorBinding','interpolated', 'ColorData',cd)
                hold off
            end
    
            [x,y,z] = getpoints(h2);
            clearpoints(h2);
            subplot(2,2,1);
            hold on
            h2 = plot3(x, y, z, 'r', 'LineWidth', 2);
            % modified jet-colormap
            cd = [uint8(jet(length(x))*255) uint8(ones(length(x),1))].';
            drawnow;
            set(h2.Edge, 'ColorBinding','interpolated', 'ColorData',cd)
            hold off
    
            % create an empty array to store the handles
            hPlots = [];
            
            if (ismember("Value_TrajectoryRepetitionStart", T.Properties.VariableNames) && ismember("Value_TrajectoryRepetitionEnd", T.Properties.VariableNames))
                subplot(2,2,1)
                hold on
                for i = 1:10
                    if(TrajectoryTable{i, "Value_TrajectoryRepetitionStart"} == i - 1)
                        TrajectoryTable_x1 = TrajectoryTable{i, "Value_x"};
                        TrajectoryTable_y1 = TrajectoryTable{i, "Value_y"};
                        TrajectoryTable_z1 = TrajectoryTable{i, "Value_z"};
                    end
                    if( TrajectoryTable{i + 1, "Value_TrajectoryRepetitionEnd"} == i - 1)
                        TrajectoryTable_x2 = TrajectoryTable{i + 1, "Value_x"};
                        TrajectoryTable_y2 = TrajectoryTable{i + 1, "Value_y"};
                        TrajectoryTable_z2 = TrajectoryTable{i + 1, "Value_z"};
                    end
                    TrajectoryTable_Plot = plot3([TrajectoryTable_x1 TrajectoryTable_x2], [TrajectoryTable_z1 TrajectoryTable_z2], [TrajectoryTable_y1 TrajectoryTable_y2], 'Color', 'black', 'LineWidth', 1);
                    hPlots = [hPlots TrajectoryTable_Plot];
                end
                TrajectoryTable(1:11,:) = [];
                hold off
            end
    
            if ismember("Value_LeftControllerSpeed", T.Properties.VariableNames)
                [x,z] = getpoints(h3);
                clearpoints(h3);
                subplot(2,2,2);
                hold on
                h3 = plot(x, z, 'r', 'LineWidth', 2);
                % modified jet-colormap
                cd = [uint8(jet(length(x))*255) uint8(ones(length(x),1))].';
                drawnow;
                set(h3.Edge, 'ColorBinding','interpolated', 'ColorData',cd)
                hold off
            end
    
            [x,z] = getpoints(h4);
            clearpoints(h4);
            subplot(2,2,2);
            hold on
            h4 = plot(x, z, 'r', 'LineWidth', 2);
            % modified jet-colormap
            cd = [uint8(jet(length(x))*255) uint8(ones(length(x),1))].';
            drawnow;
            set(h4.Edge, 'ColorBinding','interpolated', 'ColorData',cd)
            hold off
            while true
                waitforbuttonpress;
                % If a key is pressed, check which key was pressed
                key = get(gcf, 'CurrentCharacter');
                if key == 'c'
                    % If the 'c' key is pressed, clear the points of all animated lines
                    delete(h1);
                    delete(h2);
                    delete(h3);
                    delete(h4);
                    subplot(2,2,1);
                    h1 = animatedline('Color', 'r', 'LineWidth', 1, 'LineStyle','--');
                    h2 = animatedline('Color', 'g', 'LineWidth', 1, 'LineStyle','--');
                    subplot(2,2,2);
                    h3 = animatedline('Color', 'r', 'LineWidth', 1, 'LineStyle','--');
                    h4 = animatedline('Color', 'g', 'LineWidth', 1, 'LineStyle','--');
                    if ismember("Value_LeftControllerSpeed", T.Properties.VariableNames)
                        clearpoints(h5);
                    end
                    clearpoints(h6);
                    delete(hPlots);
                    set(gcf, 'CurrentCharacter', 'a');
                    break;
                end
            end
        end
    
        lastElapsedTime = T{i, "Value_ElapsedTime"};
    
        b = toc(a); % check timer
        if b > (1/60)
            F = getframe(gcf);
            writeVideo(video, F);
            pause(0.01);
            drawnow;
            a = tic; % reset timer after updating
        end
    end
    close(video)
    
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
end