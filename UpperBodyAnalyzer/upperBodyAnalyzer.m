%analyze('EverydayTask')
%analyze('FingerFollow')
%analyze('FingerNose')
analyze('Pegboard')

function analyze(s)
    T = readtable(strcat(s,'.csv'));
  
    %% Graphs %%
    figure(1)
    set(gcf, 'Position', get(0, 'Screensize'));

    type = T{:, "Value_Type"};
    
    % Controller Positions
    x1 = T{:, "Value_x"};
    y1 = T{:, "Value_y"};
    z1 = T{:, "Value_z"};

    x2 = T{:, "Value_x"};
    y2 = T{:, "Value_y"};
    z2 = T{:, "Value_z"};

    subplot(1,2,1);
        title('Controller Positions 3D');
        h1 = animatedline('Color', 'r', 'LineWidth', 2);
        h2 = animatedline('Color', 'g', 'LineWidth', 2);
        set(gca, 'XLim', [-1.5 1.5], 'YLim', [-1.5 1.5], 'ZLim', [-1.5 1.5]);
        grid on
        view(43,24);
        xlabel('x');
        ylabel('y');
        zlabel('z');

    subplot(1,2,2);
        title('Controller Positions 2D');
        h3 = animatedline('Color', 'r', 'LineWidth', 2);
        h4 = animatedline('Color', 'g', 'LineWidth', 2);
        set(gca, 'XLim', [-1.5 1.5], 'YLim', [-1.5 1.5]);
        xlabel('x');
        ylabel('z');
        % Controller Positions

    %% Animation
    video = VideoWriter(strcat(s,'.avi'), 'Uncompressed AVI');
    open(video)
    
    a = tic; % start timer
    for i = 1:length(type)
        if(strcmp(type(i), 'LeftControllerPosition'))
            addpoints(h1, x1(i), z1(i), y1(i));
        end

        if(strcmp(type(i), 'RightControllerPosition'))
            addpoints(h2, x2(i), z2(i), y2(i));
        end

        if(strcmp(type(i), 'LeftControllerPosition'))
            addpoints(h3, x1(i), y1(i));
        end

        if(strcmp(type(i), 'RightControllerPosition'))
            addpoints(h4, x2(i), y2(i));
        end

        b = toc(a); % check timer
        if b > (1/1000)
            F = getframe(gcf);
            writeVideo(video, F);
            pause(0.5);
            drawnow; % update screen every 1/200 seconds
            a = tic; % reset timer after updating
        end
    end
    close(video)
end