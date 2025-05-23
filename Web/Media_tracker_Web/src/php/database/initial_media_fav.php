<?php
require_once '../db.php';
require_once '../tools.php';

try {
    if (
        isset($_POST['platform_id']) &&
        isset($_POST['media_type_id']) &&
        isset($_POST['media_plat_id']) &&
        isset($_POST['title']) &&
        isset($_POST['album']) &&
        isset($_POST['artist']) &&
        isset($_POST['username'])
    ) {
        // Assign POST variables individually
        $platform_id = sanitizeInt($_POST['platform_id']);
        $media_type_id = sanitizeInt($_POST['media_type_id']);
        $media_plat_id = sanitizeInt($_POST['media_plat_id']);
        $title = sanitizeString($_POST['title']);
        $album = sanitizeString($_POST['album']);
        $artist = sanitizeString($_POST['artist']);
        $username = sanitizeString($_POST['username']);

        // Prepare and execute the stored function
        $stmt = $pdo->prepare("SELECT public.initial_media_fav(?, ?, ?, ?, ?, ?, ?)");
        $stmt->execute([
            $platform_id,
            $media_type_id,
            $media_plat_id,
            $title,
            $album,
            $artist,
            $username
        ]);

        // Fetch the result returned by the function
        $result = $stmt->fetchColumn();
        echo htmlspecialchars($result); // Escape output for safety
    } else {
        echo "Required fields are missing.";
    }
} catch (PDOException $e) {
    echo "Error: " . $e->getMessage();
}

$pdo = null;
?>
