import torch
import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
import math
from collections import Counter
import time
from sklearn.metrics import silhouette_score


def most_common_centroid_index(centroids):
    # Convert the list of centroids to a numpy array
    centroids_array = np.array(centroids)
    coordinates_sum = []

    for centroid in centroids_array:
        coordinates_sum.append(round(sum(sum(centroid)), 4))

    most_common = Counter(coordinates_sum).most_common(1)

    return coordinates_sum.index(most_common[0][0]), most_common[0][1]


def k_means_clustering(num_clusters, num_iterations, centroids_iterations):
    start_time = time.time()

    # Set the number of clusters
    # Initialize cluster centroids with NaN values
    centroids = torch.empty(num_clusters, 2)
    centroids.fill_(math.nan)
    # Load participant data from CSV
    data = pd.read_csv('EyeTrackingMultiple.csv')

    # Extract x and y coordinates from the DataFrame
    x_values = data['Value.CombinedEyesGazeDirectionNormalized_x'].values
    y_values = data['Value.CombinedEyesGazeDirectionNormalized_y'].values

    # Remove NaN values
    valid_indices = (~np.isnan(x_values) & ~np.isnan(y_values)) & (x_values != -1) & (y_values != -1)
    x_values = x_values[valid_indices]
    y_values = y_values[valid_indices]

    # Combine x and y coordinates into a single tensor
    coordinates = torch.tensor(list(zip(x_values, y_values)), dtype=torch.float32)

    possible_centroids = []
    for i in range(centroids_iterations):
        while torch.isnan(centroids).any():
            # Initialize cluster centroids randomly within the range of data points
            x_min, x_max = torch.min(coordinates[:, 0]), torch.max(coordinates[:, 0])
            y_min, y_max = torch.min(coordinates[:, 1]), torch.max(coordinates[:, 1])
            centroids = torch.stack([
                torch.rand(num_clusters) * (x_max - x_min) + x_min,
                torch.rand(num_clusters) * (y_max - y_min) + y_min
            ], dim=1)

            print('Centroids random init:' + str(centroids))

            # Perform k-means clustering
            for _ in range(num_iterations):
                # Compute distances between data points and centroids
                distances = torch.cdist(coordinates, centroids)

                # Assign data points to the nearest cluster
                cluster_labels = torch.argmin(distances, dim=1)

                # Update cluster centroids
                for j in range(num_clusters):
                    cluster_points = coordinates[cluster_labels == j]
                    centroids[j] = cluster_points.mean(dim=0)

        possible_centroids.append(centroids)

        # Reinitialize cluster centroids with NaN values
        centroids = torch.empty(num_clusters, 2)
        centroids.fill_(math.nan)

    _most_common_centroid_index, accuracy = most_common_centroid_index(possible_centroids)
    most_common_centroid = possible_centroids[_most_common_centroid_index]

    print('Centroids found:' + str(most_common_centroid))

    # Compute the silhouette coefficient
    labels = torch.argmin(torch.cdist(coordinates, most_common_centroid), dim=1)
    silhouette = silhouette_score(coordinates.numpy(), labels.numpy())
    print('Silhouette Coefficient:', silhouette)

    print('Accuracy: ' + str(math.floor((accuracy / centroids_iterations) * 100)) + '%')

    end_time = time.time()
    # Calculate the elapsed time
    elapsed_time = end_time - start_time
    print("Elapsed time:", elapsed_time, "seconds")

    # Visualize the data and clusters
    # Assign a color to each cluster
    colors = ['red', 'blue', 'green', 'orange', 'purple']  # Add more colors if needed

    # Visualize the data and clusters
    for i in range(num_clusters):
        cluster_points = coordinates[labels == i]
        plt.scatter(cluster_points[:, 0], cluster_points[:, 1], c=colors[i], zorder=1)

    # Plot the cluster centroids
    plt.scatter(most_common_centroid[:, 0], most_common_centroid[:, 1], c='black', marker='x', zorder=2)
    plt.xlabel('x')
    plt.ylabel('y')
    plt.title('K-means Clustering')
    plt.show()


if __name__ == '__main__':
    k_means_clustering(num_clusters=5, num_iterations=50, centroids_iterations=20)
