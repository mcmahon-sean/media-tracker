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

        // Assign POST variables individually
        $platform_id = $_POST['platform_id'];
        $media_type_id = $_POST['media_type_id'];
        $media_plat_id = $_POST['media_plat_id'];
        $title = $_POST['title'];
        $album = $_POST['album'];
        $artist = $_POST['artist'];

        // Prepare the SQL statement to call the function
        $stmt = $pdo->prepare("SELECT public.add_media_titles(?, ?, ?, ?, ?, ?)");

        // Execute the prepared statement
        $stmt->execute([$platform_id, $media_type_id, $media_plat_id, $title, $album, $artist]);

        // Fetch the returned message from the function
        $result = $stmt->fetchColumn();

        // Output the function's return message directly
        echo $result;

    } else {
        echo "Required fields are missing.";
    }

} catch (PDOException $e) {
    echo "Error: " . $e->getMessage();
}

$pdo = null;
?>
