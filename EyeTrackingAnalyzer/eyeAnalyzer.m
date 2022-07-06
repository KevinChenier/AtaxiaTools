T = readtable('events.csv');

%% Table new variables %%
% Lost Focus
for i=1:(height(T))
    T{i, "Value_LostFocus"} = (T{i, "Value_CombinedEyesGazeDirectionNormalized_x"} == -1);
end
% Lost Focus

% Left Gaze Direction Degrees x
for i=1:(height(T))
    if (T{i, "Value_LostFocus"})
        T{i, "Value_LeftEyeDirectionDegrees_x"} = 0;
    else
        T{i, "Value_LeftEyeDirectionDegrees_x"} = (atan(T{i, "Value_LeftEyeGazeDirectionNormalized_x"} / T{i, "Value_LeftEyeGazeDirectionNormalized_z"}) / pi) * 180;
    end
end
% Left Gaze Direction Degrees x

% Left Gaze Direction Degrees y
for i=1:(height(T))
    if (T{i, "Value_LostFocus"})
        T{i, "Value_LeftEyeDirectionDegrees_y"} = 0;
    else
        T{i, "Value_LeftEyeDirectionDegrees_y"} = (atan(T{i, "Value_LeftEyeGazeDirectionNormalized_y"} / T{i, "Value_LeftEyeGazeDirectionNormalized_z"}) / pi) * 180;
    end
end
% Left Gaze Direction Degrees y

% Right Gaze Direction Degrees x
for i=1:(height(T))
    if (T{i, "Value_LostFocus"})
        T{i, "Value_RightEyeDirectionDegrees_x"} = 0;
    else
        T{i, "Value_RightEyeDirectionDegrees_x"} = (atan(T{i, "Value_RightEyeGazeDirectionNormalized_x"} / T{i, "Value_RightEyeGazeDirectionNormalized_z"}) / pi) * 180;
    end 
end
% Right Gaze Direction Degrees x

% Right Gaze Direction Degrees y
for i=1:(height(T))
    if (T{i, "Value_LostFocus"})
        T{i, "Value_RightEyeDirectionDegrees_y"} = 0;
    else
        T{i, "Value_RightEyeDirectionDegrees_y"} = (atan(T{i, "Value_RightEyeGazeDirectionNormalized_y"} / T{i, "Value_RightEyeGazeDirectionNormalized_z"}) / pi) * 180;
    end
end
% Right Gaze Direction Degrees y

% Left Eye Velocity Degrees
for i=1:(height(T) - 1)
    LeftGazeDirectionDegrees_x1 = T{i, "Value_LeftEyeDirectionDegrees_x"};
    LeftGazeDirectionDegrees_y1 = T{i, "Value_LeftEyeDirectionDegrees_y"};

    LeftGazeDirectionDegrees_x2 = T{i + 1, "Value_LeftEyeDirectionDegrees_x"};
    LeftGazeDirectionDegrees_y2 = T{i + 1, "Value_LeftEyeDirectionDegrees_y"};

    Time1 = T{i, "Value_Time"};
    Time2 = T{i + 1, "Value_Time"};

    if (T{i + 1, "Value_LostFocus"} || T{i, "Value_LostFocus"})
        Velocity = 0;
    else
        Velocity = sqrt( ...
                        ((LeftGazeDirectionDegrees_x2 - LeftGazeDirectionDegrees_x1) / (Time2 - Time1))^2 ...
                        + ...
                        ((LeftGazeDirectionDegrees_y2 - LeftGazeDirectionDegrees_y1) / (Time2 - Time1))^2 ...
                       );
    end

    T{i + 1, "Value_LeftEyeVelocityDegrees"} = Velocity * 1000;

end 
% Left Eye Velocity Degrees

% Left Eye Velocity Degrees x
for i=1:(height(T) - 1)
    LeftGazeDirectionDegrees_x1 = T{i, "Value_LeftEyeDirectionDegrees_x"};
    LeftGazeDirectionDegrees_x2 = T{i + 1, "Value_LeftEyeDirectionDegrees_x"};

    Time1 = T{i, "Value_Time"};
    Time2 = T{i + 1, "Value_Time"};

    if (T{i + 1, "Value_LostFocus"} || T{i, "Value_LostFocus"})
        Velocity = 0;
    else
        Velocity = (LeftGazeDirectionDegrees_x2 - LeftGazeDirectionDegrees_x1) / (Time2 - Time1);
    end

    T{i + 1, "Value_LeftEyeVelocityDegrees_x"} = Velocity * 1000;
end 
% Left Eye Velocity Degrees x

% Left Eye Velocity Degrees y
for i=1:(height(T) - 1)
    LeftGazeDirectionDegrees_y1 = T{i, "Value_LeftEyeDirectionDegrees_y"};
    LeftGazeDirectionDegrees_y2 = T{i + 1, "Value_LeftEyeDirectionDegrees_y"};

    Time1 = T{i, "Value_Time"};
    Time2 = T{i + 1, "Value_Time"};

    if (T{i + 1, "Value_LostFocus"} || T{i, "Value_LostFocus"})
        Velocity = 0;
    else
        Velocity = (LeftGazeDirectionDegrees_y2 - LeftGazeDirectionDegrees_y1) / (Time2 - Time1);
    end

    T{i + 1, "Value_LeftEyeVelocityDegrees_y"} = Velocity * 1000;
end 
% Left Eye Velocity Degrees y

% Right Eye Velocity Degrees
for i=1:(height(T) - 1)
    RightGazeDirectionDegrees_x1 = T{i, "Value_RightEyeDirectionDegrees_x"};
    RightGazeDirectionDegrees_y1 = T{i, "Value_RightEyeDirectionDegrees_y"};

    RightGazeDirectionDegrees_x2 = T{i + 1, "Value_RightEyeDirectionDegrees_x"};
    RightGazeDirectionDegrees_y2 = T{i + 1, "Value_RightEyeDirectionDegrees_y"};

    Time1 = T{i, "Value_Time"};
    Time2 = T{i + 1, "Value_Time"};

    if (T{i + 1, "Value_LostFocus"} || T{i, "Value_LostFocus"})
        Velocity = 0;
    else
        Velocity = sqrt( ...
                        ((RightGazeDirectionDegrees_x2 - RightGazeDirectionDegrees_x1) / (Time2 - Time1))^2 ...
                        + ...
                        ((RightGazeDirectionDegrees_y2 - RightGazeDirectionDegrees_y1) / (Time2 - Time1))^2 ...
                       );
    end

    T{i + 1, "Value_RightEyeVelocityDegrees"} = Velocity * 1000;
end 
% Right Eye Velocity Degrees

% Right Eye Velocity Degrees x
for i=1:(height(T) - 1)
    RightGazeDirectionDegrees_x1 = T{i, "Value_RightEyeDirectionDegrees_x"};
    RightGazeDirectionDegrees_x2 = T{i + 1, "Value_RightEyeDirectionDegrees_x"};

    Time1 = T{i, "Value_Time"};
    Time2 = T{i + 1, "Value_Time"};

    if (T{i + 1, "Value_LostFocus"} || T{i, "Value_LostFocus"})
        Velocity = 0;
    else
        Velocity = (RightGazeDirectionDegrees_x2 - RightGazeDirectionDegrees_x1) / (Time2 - Time1);
    end

    T{i + 1, "Value_RightEyeVelocityDegrees_x"} = Velocity * 1000;
end 
% Right Eye Velocity Degrees x

% Right Eye Velocity Degrees y
for i=1:(height(T) - 1)
    RightGazeDirectionDegrees_y1 = T{i, "Value_RightEyeDirectionDegrees_y"};
    RightGazeDirectionDegrees_y2 = T{i + 1, "Value_RightEyeDirectionDegrees_y"};

    Time1 = T{i, "Value_Time"};
    Time2 = T{i + 1, "Value_Time"};

    if (T{i + 1, "Value_LostFocus"} || T{i, "Value_LostFocus"})
        Velocity = 0;
    else
        Velocity = (RightGazeDirectionDegrees_y2 - RightGazeDirectionDegrees_y1) / (Time2 - Time1);
    end

    T{i + 1, "Value_RightEyeVelocityDegrees_y"} = Velocity * 1000;
end 
% Right Eye Velocity Degrees y

% Left Saccade
for i=1:(height(T))
    T{i, "Value_LeftSaccade"} = double((T{i, "Value_LeftEyeVelocityDegrees"} > 400));
end 
% Left Saccade

% Right Saccade
for i=1:(height(T))
    T{i, "Value_RightSaccade"} = double((T{i, "Value_RightEyeVelocityDegrees"} > 400));
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

timeThreshold = 500;
velocityThreshold = 400;

% Left Eye Square Wave Jerk x
for i=1:(height(T) - 1)
    if (T{i, "Value_LeftEyeVelocityDegrees_x"} >= velocityThreshold)
        initialTime = T{i, "Value_Time"};
        initialPosition = T{i, "Value_LeftEyeDirectionDegrees_x"};
        for j=i+1:(height(T) - 1)
            currentPosition = T{j, "Value_LeftEyeDirectionDegrees_x"};
            if (currentPosition <= (initialPosition + 0.5) && currentPosition >= (initialPosition - 0.5) && ~T{j, "Value_LostFocus"})
                T{j, "Value_LeftEyeSquareWaveJerk_x"} = 1;
                i=j;
                break;
            elseif ((T{j, "Value_Time"} - initialTime) > timeThreshold)
                i=j;
                break;
            end
        end
    end
end
% Left Eye Square Wave Jerk x

% Left Eye Square Wave Jerk y
for i=1:(height(T) - 1)
    if (T{i, "Value_LeftEyeVelocityDegrees_y"} >= velocityThreshold)
        initialTime = T{i, "Value_Time"};
        initialPosition = T{i, "Value_LeftEyeDirectionDegrees_y"};
        for j=i+1:(height(T) - 1)
            currentPosition = T{j, "Value_LeftEyeDirectionDegrees_y"};
            if (currentPosition <= (initialPosition + 0.5) && currentPosition >= (initialPosition - 0.5) && ~T{j, "Value_LostFocus"})
                T{j, "Value_LeftEyeSquareWaveJerk_y"} = 1;
                i=j;
                break;
            elseif ((T{j, "Value_Time"} - initialTime) > timeThreshold)
                i=j;
                break;
            end
        end
    end
end
% Left Eye Square Wave Jerk y

% Right Eye Square Wave Jerk x
for i=1:(height(T) - 1)
    if (T{i, "Value_RightEyeVelocityDegrees_x"} >= velocityThreshold)
        initialTime = T{i, "Value_Time"};
        initialPosition = T{i, "Value_RightEyeDirectionDegrees_x"};
        for j=i+1:(height(T) - 1)
            currentPosition = T{j, "Value_RightEyeDirectionDegrees_x"};
            if (currentPosition <= (initialPosition + 0.5) && currentPosition >= (initialPosition - 0.5) && ~T{j, "Value_LostFocus"})
                T{j, "Value_RightEyeSquareWaveJerk_x"} = 1;
                i=j;
                break;
            elseif ((T{j, "Value_Time"} - initialTime) > timeThreshold)
                i=j;
                break;
            end
        end
    end
end
% Right Eye Square Wave Jerk x

% Right Eye Square Wave Jerk y
for i=1:(height(T) - 1)
    if (T{i, "Value_RightEyeVelocityDegrees_y"} >= velocityThreshold)
        initialTime = T{i, "Value_Time"};
        initialPosition = T{i, "Value_RightEyeDirectionDegrees_y"};
        for j=i+1:(height(T) - 1)
            currentPosition = T{j, "Value_RightEyeDirectionDegrees_y"};
            if (currentPosition <= (initialPosition + 0.5) && currentPosition >= (initialPosition - 0.5) && ~T{j, "Value_LostFocus"})
                T{j, "Value_RightEyeSquareWaveJerk_y"} = 1;
                i=j;
                break;
            elseif ((T{j, "Value_Time"} - initialTime) > timeThreshold)
                i=j;
                break;
            end
        end
    end
end
% Right Eye Square Wave Jerk y

% Square Wave Jerk
for i=1: (height(T) - 1)
    if(T{i, "Value_LeftEyeSquareWaveJerk_x"} && T{i, "Value_RightEyeSquareWaveJerk_x"} || T{i, "Value_LeftEyeSquareWaveJerk_y"} && T{i, "Value_RightEyeSquareWaveJerk_y"})
        T{i, "Value_SquareWaveJerk"} = 1;
    end
end
% Square Wave Jerk

%% Graphs %%
figure(1)
set(gcf, 'Position', get(0, 'Screensize'));
% Left Eye Pupil Diameter
subplot(5, 5, 1);
    x1 = T{:, "Value_Time"};
    y1 = T{:, "Value_LeftEyePupilDiameter"};
    title('Left Eye Pupil Diameter');
    xlabel('Time (ms)');
    ylabel('Diameter (mm)');
    h1 = animatedline('Color', 'r');
    ylim([-1 inf])
% Left Eye Pupil Diameter

% Right Eye Pupil Diameter
subplot(5, 5, 2);
    x2 = T{:, "Value_Time"};
    y2 = T{:, "Value_RightEyePupilDiameter"};
    title('Right Eye Pupil Diameter');
    xlabel('Time (ms)');
    ylabel('Diameter (mm)');
    h2 = animatedline('Color', 'g');
    ylim([-1 inf])
% Right Eye Pupil Diameter

% Left Eye Openness
subplot(5, 5, 3);
    x3 = T{:, "Value_Time"};
    y3 = T{:, "Value_LeftEyeOpenness"};
    title('Left Eye Openness');
    xlabel('Time (ms)');
    ylabel('Openness');
    h3 = animatedline('Color', 'r');
% Left Eye Openness
    
% Right Eye Openness
subplot(5, 5, 4);
    x4 = T{:, "Value_Time"};
    y4 = T{:, "Value_RightEyeOpenness"};
    title('Right Eye Openness');
    xlabel('Time (ms)');
    ylabel('Openness');
    h4 = animatedline('Color', 'g');
% Right Eye Openness
    
% Left Eye Velocity Degrees
subplot(5, 5, 5);
    x5 = T{:, "Value_Time"};
    y5 = T{:, "Value_LeftEyeVelocityDegrees"};
    title('Left Eye Velocity');
    xlabel('Time (ms)');
    ylabel('Velocity (Degrees/s)');
    h5 = animatedline('Color', 'r');
% Left Eye Velocity Degrees

% Right Eye Velocity Degrees
subplot(5, 5, 6);
    x6 = T{:, "Value_Time"};
    y6 = T{:, "Value_RightEyeVelocityDegrees"};
    xlabel('Time (ms)');
    title('Right Eye Velocity');
    ylabel('Velocity (Degrees/s)');
    h6 = animatedline('Color', 'g');
% Right Eye Velocity Degrees

% Left Saccade
subplot(5, 5, 7);
    x7 = T{:, "Value_Time"};
    y7 = T{:, "Value_LeftSaccade"};
    title('Left Saccade');
    xlabel('Time (ms)');
    ylabel('Saccade');
    h7 = animatedline('Color', 'r');
% Left Saccade

% Right Saccade
subplot(5, 5, 8);
    x8 = T{:, "Value_Time"};
    y8 = T{:, "Value_RightSaccade"};
    title('Right Saccade');
    xlabel('Time (ms)');
    ylabel('Saccade');
    h8 = animatedline('Color', 'g');
% Right Saccade

% Left Fixation
subplot(5, 5, 9);
    x9 = T{:, "Value_Time"};
    y9 = T{:, "Value_LeftFixation"};
    title('Left Fixation');
    xlabel('Time (ms)');
    ylabel('Fixation');
    h9 = animatedline('Color', 'r');
% Left Fixation

% Right Fixation
subplot(5, 5, 10);
    x10 = T{:, "Value_Time"};
    y10 = T{:, "Value_RightFixation"};
    title('Right Fixation');
    xlabel('Time (ms)');
    ylabel('Fixation');
    h10 = animatedline('Color', 'g');
% Right Fixation

% Left Eye Pupil Position in Sensor Area
    x11 = T{:, "Value_LeftEyePupilPositionInSensorArea_x"};
    y11 = T{:, "Value_LeftEyePupilPositionInSensorArea_y"};
    pts = linspace(0, 1, 70);
    N = histcounts2(y11(:), x11(:), pts, pts);
    
subplot(5, 5, 11);
    imagesc(pts, pts, N);
    xlabel('x');
    ylabel('y');
    title('Left Eye Pupil Position In Sensor Area');
    axis equal;
    set(gca, 'XLim', pts([1 end]), 'YLim', pts([1 end]), 'YDir', 'normal');

subplot(5, 5, 12);
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
    
subplot(5, 5, 13);
    imagesc(pts, pts, N);
    xlabel('x');
    ylabel('y');
    title('Right Eye Pupil Position In Sensor Area');
    axis equal;
    set(gca, 'XLim', pts([1 end]), 'YLim', pts([1 end]), 'YDir', 'normal');

subplot(5, 5, 14);
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
    
subplot(5, 5, 15);
    imagesc(pts, pts, N);
    xlabel('x');
    ylabel('y');
    title('Combined Eyes Gaze Direction');
    axis equal;
    set(gca, 'XLim', pts([1 end]), 'YLim', pts([1 end]), 'YDir', 'normal');

subplot(5, 5, 16);
    xlabel('x');
    ylabel('y');
    title('Combined Eyes Gaze Direction');
    h13 = animatedline('Color', 'b', 'MaximumNumPoints', 25, 'LineWidth', 3);
    axis equal
    xlim([-1 1])
    ylim([-1 1])
% Combined Eyes Gaze Directions Normalized

% Left Eye Direction Degrees x
subplot(5, 5, 17);
    x14 = T{:, "Value_Time"};
    y14 = T{:, "Value_LeftEyeDirectionDegrees_x"};
    title('Left Eye Direction Degrees X');
    xlabel('Time (ms)');
    ylabel('Degrees');
    h14 = animatedline('Color', 'r');
% Left Eye Direction Degrees x

% Right Eye Direction Degrees x
subplot(5, 5, 18);
    x15 = T{:, "Value_Time"};
    y15 = T{:, "Value_RightEyeDirectionDegrees_x"};
    title('Right Eye Direction Degrees X');
    xlabel('Time (ms)');
    ylabel('Degrees');
    h15 = animatedline('Color', 'g');
% Right Eye Direction Degrees x

% Left Eye Direction Degrees y
subplot(5, 5, 19);
    x16 = T{:, "Value_Time"};
    y16 = T{:, "Value_LeftEyeDirectionDegrees_y"};
    title('Left Eye Direction Degrees Y');
    xlabel('Time (ms)');
    ylabel('Degrees');
    h16 = animatedline('Color', 'r');
% Left Eye Direction Degrees y

% Right Eye Direction Degrees y
subplot(5, 5, 20);
    x17 = T{:, "Value_Time"};
    y17 = T{:, "Value_RightEyeDirectionDegrees_y"};
    title('Right Eye Direction Degrees Y');
    xlabel('Time (ms)');
    ylabel('Degrees');
    h17 = animatedline('Color', 'g');
% Right Eye Direction Degrees y

% Square Wave Jerk
subplot(5, 5, 23);
    x18 = T{:, "Value_Time"};
    y18 = T{:, "Value_SquareWaveJerk"};
    title('Square Wave Jerk');
    xlabel('Time (ms)');
    ylabel('SWJ');
    h18 = animatedline('Color', 'b');
% Square Wave Jerk

%% Animation
video = VideoWriter('Eye-TrackingAnalysis.avi', 'Uncompressed AVI');
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
    if (x17(i) ~= -1)
        addpoints(h17, x17(i), y17(i));
    end
    addpoints(h18, x18(i), y18(i));

    b = toc(a); % check timer
    if b > (1/200)
        F = getframe(gcf)
        writeVideo(video, F);
        drawnow % update screen every 1/200 seconds
        a = tic; % reset timer after updating
    end
end
close(video)