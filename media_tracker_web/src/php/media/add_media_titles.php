<?php
require 'db.php';

if (isset($_POST['platform_id']) &&
    isset($_POST['media_type_id']) &&
    isset($_POST['media_plat_id']) &&
    isset($_POST['title']) &&
    isset($_POST['album']) &&
    isset($_POST['artist'])) {

    $data = [
        (int)$_POST['platform_id'],
        (int)$_POST['media_type_id'],
        $_POST['media_plat_id'],
        $_POST['title'],
        $_POST['album'],
        $_POST['artist']
    ];

    $stmt = $pdo->prepare("SELECT AddMediaTitles(?, ?, ?, ?, ?, ?)");
    $stmt->execute($data);

    echo "Media added!";
} else {
    echo "Required fields are missing.";
}
?>
