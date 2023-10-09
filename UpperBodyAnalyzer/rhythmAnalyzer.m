s = 'Rhythm';

T = readtable(strcat(s,'.csv'));

%% Table new variables %%
% Rhythm BPM

BeatPerSecond = 60000 / T{1, "Value_BPM"};

for i=1:(height(T))
    if (T{i, "Value_NoteType"} == "hit" || T{i, "Value_NoteType"} == "spam")
        elapsedTime1 = T{i, "Value_ElapsedTime"};

        for j=(i + 1): (height(T))
            if (T{j, "Value_Type"} == "RhythmConfig" && (T{j, "Value_ToolEnded"} == "FALSE" || T{j, "Value_ToolEnded"} == "false"))
                elapsedTime1 = 0;
                i = j;
                break;
            end
            if (T{j, "Value_NoteType"} == "hit" || T{j, "Value_NoteType"} == "spam")
                elapsedTime2 = T{j, "Value_ElapsedTime"};
                Beat = elapsedTime2 - elapsedTime1;
                T{j, "Value_RhythmBPM"} = Beat;
                T{j, "Value_Error"} = abs(BeatPerSecond - Beat);
                i = j;
                break
            end
        end
    end 
end
% Rhythm BPM