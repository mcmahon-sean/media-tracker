<?php
require 'db.php';

if (isset($_POST['username']) &&
    isset($_POST['media_id'])) {

    $username = $_POST['username'];
    $mediaID = (int)$_POST['media_id'];

    $stmt = $pdo->prepare("SELECT UpdateFav(?, ?)");
    $stmt->execute([$username, $mediaID]);

    echo "Favorite updated!";
} else {
    echo "Required fields are missing.";
}
?>
