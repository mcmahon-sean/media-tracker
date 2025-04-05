<?php
require 'db.php';

if (isset($_POST['username']) &&
    isset($_POST['platform_id']) &&
    isset($_POST['user_plat_id'])) {

    $data = [
        $_POST['username'],
        (int)$_POST['platform_id'],
        $_POST['user_plat_id']
    ];

    $stmt = $pdo->prepare("SELECT Add3rdPartyID(?, ?, ?)");
    $stmt->execute($data);

    echo "3rd party ID saved!";
} else {
    echo "Required fields are missing.";
}
?>
