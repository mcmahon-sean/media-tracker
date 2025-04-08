<?php
require '../db.php';

try {
    // Check if all required POST data is set
    if (isset($_POST['platform_id']) &&
        isset($_POST['media_type_id']) &&
        isset($_POST['media_plat_id']) &&
        isset($_POST['title']) &&
        isset($_POST['album']) &&
        isset($_POST['artist'])) {

        // // Assign POST variables individually
        $platform_id = $_POST['platform_id'];
        $media_type_id = $_POST['media_type_id'];
        $media_plat_id = $_POST['media_plat_id'];
        $title = $_POST['title'];
        $album = $_POST['album'];
        $artist = $_POST['artist'];

        // Prepare the SQL statement to call the function
        $stmt = $pdo->prepare("SELECT public.\"AddMediaTitles\"(?, ?, ?, ?, ?, ?)");

        // Execute the prepared statement with the data array
        $stmt->execute([$platform_id, $media_type_id, $media_plat_id, $title, $album, $artist]);

        // Fetch the result which will be either true or false
        $result = $stmt->fetchColumn();

        // Check if the function returned true or false and display the corresponding message
        if ($result) {
            echo "Media added successfully!";
        } else {
            echo "Failed to add media. Please check your input data.";
        }

    } else {
        // Output if required fields are missing
        echo "Required fields are missing.";
    }

} catch (PDOException $e) {
    // Handle errors by catching the exception and outputting the error message
    echo "Error: " . $e->getMessage();
}

// Close the database connection
$pdo = null;
?>
