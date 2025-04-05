<?php
require 'db.php';

if (isset($_POST['platform_id']) &&
    isset($_POST['media_type_id']) &&
    isset($_POST['media_plat_id']) &&
    isset($_POST['title']) &&
    isset($_POST['album']) &&
    isset($_POST['artist']) &&
    isset($_POST['username'])) {

    $data = [
        (int)$_POST['platform_id'],
        (int)$_POST['media_type_id'],
        $_POST['media_plat_id'],
        $_POST['title'],
        $_POST['album'],
        $_POST['artist'],
        $_POST['username']
    ];

    $stmt = $pdo->prepare("SELECT InitialMediaFav(?, ?, ?, ?, ?, ?, ?)");
    $stmt->execute($data);

    echo "Initial favorite processed!";
} else {
    echo "Required fields are missing.";
}
?>
