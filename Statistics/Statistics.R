library(ggplot2)

timing_result <- system.time({
  # Define the groups as specified
  patients <- c(1, 2, 5, 6, 7, 10, 11, 12, 13, 16, 17, 18)
  controls <- c(3, 4, 8, 9, 14, 15, 19, 20, 21, 22, 23, 24)
  
  # Create an empty list to store plot objects
  plots <- list()
  
  # Create an empty list to store scatter plots
  scatter_plots <- list()
  
  # Create an empty data frame to store correlation results
  data_analysis <- data.frame(
    Title = character(),
    Correlation = numeric(),
    P_Value = numeric(),
    Normality = numeric()
  )
  
  # Iterate through each column
  for (col in 1:ncol(StatisticsAnalysis)) {
    # Perform Shapiro-Wilk test
    shapiro_test_result <- shapiro.test(StatisticsAnalysis[[col]])
    
    patientsData <- na.omit(StatisticsAnalysis[patients, col])
    controlsData <- na.omit(StatisticsAnalysis[controls, col])
    
    # Convert patientsData and controlsData to vectors
    patientsData <- unlist(patientsData)
    controlsData <- unlist(controlsData)
    
    # Convert data to numeric
    patientsData <- as.numeric(patientsData)
    controlsData <- as.numeric(controlsData)
    
    # Check if the p-value is greater than 0.05 (normal distribution)
    if (shapiro_test_result$p.value > 0.05) {
      StatisticsAnalysis[26, col] <- 1
      tryCatch({
        test_result <- t.test(patientsData, controlsData)
        p_value <- test_result$p.value
        StatisticsAnalysis[27, col] <- p_value
        
        png(filename = paste(colnames(StatisticsAnalysis)[col], paste("p=", round(p_value, 4)), ".png"), width = 800, height = 600)  # Adjust width and height as needed
        # Create a box plot for visualization
        plot_obj <- boxplot(list(Patients=patientsData, Controls=controlsData),
                            main=colnames(StatisticsAnalysis)[col],
                            xlab=paste("p ", ifelse(p_value < 0.001, "< 0.001", paste("= ", round(p_value, 4)))),
                            ylab="", col=c("red", "green"))
        dev.off()
        
        data_analysis <- rbind(data_analysis, 
           data.frame(
             Title = colnames(StatisticsAnalysis)[col],
             Correlation = -100,
             P_Value = p_value,
             Normality = 1
           )
        )
        
        plots[[col]] <- plot_obj
        
      }, error = function(e) {
        # If there's an error in the t-test, insert NaN
        StatisticsAnalysis[27, col] <- NaN
      })
    } else {
      StatisticsAnalysis[26, col] <- 0
      tryCatch({
        # Address tied values: If you have tied values in your data (i.e., multiple data points with the same value), you can either remove the ties by adding a small amount of random noise to the data (e.g., by using the jitter function) or consider using the exact = FALSE argument when calling wilcox.test. Using exact = FALSE will perform an approximate test and should not require exact ties. However, keep in mind that the p-value may be less accurate in this case.
        test_result <- wilcox.test(patientsData, controlsData, exact = FALSE)
        p_value <- test_result$p.value
        StatisticsAnalysis[27, col] <- p_value
        
        png(filename = paste(colnames(StatisticsAnalysis)[col], paste("p=", round(p_value, 4)), ".png"), width = 800, height = 600)  # Adjust width and height as needed
        # Create a box plot for visualization
        plot_obj <- boxplot(list(Patients=patientsData, Controls=controlsData),
                            main=colnames(StatisticsAnalysis)[col],
                            xlab=paste("p", ifelse(p_value < 0.001, "< 0.001", paste("= ", round(p_value, 4)))),
                            ylab="", col=c("red", "green"))
        dev.off()
        
        data_analysis <- rbind(data_analysis, 
           data.frame(
             Title = colnames(StatisticsAnalysis)[col],
             Correlation = -100,
             P_Value = p_value,
             Normality = 0
           )
        )
        
        plots[[col]] <- plot_obj
        
      }, error = function(e) {
        # If there's an error in the wilcox.test, insert NaN
        StatisticsAnalysis[27, col] <- NaN
      })
    }
    StatisticsAnalysis[28, col] <- mean(controlsData)
    StatisticsAnalysis[29, col] <- sd(controlsData)
    StatisticsAnalysis[30, col] <- mean(patientsData)
    StatisticsAnalysis[31, col] <- sd(patientsData)
  }
  
  # Calculate Pearson correlations and create scatter plots
  num_columns <- ncol(StatisticsAnalysis)  # Get the number of columns
  for (col1 in 1:(num_columns - 1)) { 
    for (col2 in (col1 + 1):num_columns) {  # Start from the next column to avoid repetition
      
      if (StatisticsAnalysis[25, col1] == StatisticsAnalysis[25, col2])
        next
      
      column1 <- StatisticsAnalysis[[col1]][1:24]
      column2 <- StatisticsAnalysis[[col2]][1:24]
      
      if (StatisticsAnalysis[26, col1] == 1 && StatisticsAnalysis[26, col2] == 1){
        test_result <- t.test(column1, column2)
        p_value <- test_result$p.value
      } else {
        test_result <- wilcox.test(column1, column2, exact = FALSE)
        p_value <- test_result$p.value
      }
      
      # Calculate Pearson correlation
      correlation <- cor(column1, column2, method = "pearson", use = "complete.obs")
      
      if (is.na(correlation) || (p_value > 0.10 && correlation < 0.2 && correlation > -0.2))
        next
      
      column1Name <- colnames(StatisticsAnalysis)[col1]
      column2Name <- colnames(StatisticsAnalysis)[col2]
      
      # Create scatter plot with a trendline
      scatter_plot <- ggplot(StatisticsAnalysis[1:24, ], aes_string(x = column1, y = column2)) +
        geom_point() +
        geom_smooth(method = "lm", se = FALSE, color = "blue") +  # Add a linear trendline
        labs(
          title = paste("Association between", column1Name, "and", column2Name),
          x = column1Name,
          y = column2Name,
          caption = paste("Pearson Correlation =", round(correlation, 3), paste(", p", ifelse(p_value < 0.001, "< 0.001", paste("= ", round(p_value, 4)))))
        ) +
        theme_minimal() +  # Set a minimal theme
        theme(
          plot.background = element_rect(fill = "white"),  # Set white background
          plot.title = element_text(hjust = 0.5, vjust = 2),  # Center the title
          plot.caption = element_text(hjust = 0.5)  # Center the caption
        )
      ggsave(paste(column1Name, "vs", column2Name, "Pearson=", correlation, paste("p=", round(p_value, 4)), ".png"),
             plot = scatter_plot, width = 8, height = 6)
      
      data_analysis <- rbind(data_analysis, 
         data.frame(
           Title = paste(column1Name, "vs", column2Name),
           Correlation = correlation,
           P_Value = p_value,
           Normality = 0
         )
      )
      
      scatter_plots[[paste(col1, col2)]] <- scatter_plot
    }
  }
  
  write.csv(StatisticsAnalysis, file = "StatisticsAnalysisResults.csv")
  
  write.csv(data_analysis, file = "DataAnalysis.csv")
  
  
  # Save the plots to separate files or display them as needed
  # for (i in 1:length(plots)) {
  #   # Save plots to files (you can change the file format as needed)
  #   # For example, to save as PNG files:
  #   # png(filename = paste("Plot_Col", i, ".png", sep=""))
  #   # print(plots[[i]])
  #   # dev.off()
  #   
  #   print(plots[[i]])
  # }
  
  # result <- wilcox.test(StatisticsAnalysis$`Average time pegboard (vr)...3`, StatisticsAnalysis$`Finger follow low jerk`, exact = FALSE)
  # print(result)
})

print(timing_result)
