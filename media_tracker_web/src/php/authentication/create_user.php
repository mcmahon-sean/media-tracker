<?php
require 'db.php';

if (isset($_POST['username']) &&
    isset($_POST['first_name']) &&
    isset($_POST['email']) &&
    isset($_POST['password'])) {

    $username = $_POST['username'];
    $first = $_POST['first_name'];
    $last = $_POST['last_name'] ?? null;
    $email = $_POST['email'];
    $password = password_hash($_POST['password'], PASSWORD_DEFAULT);

    $stmt = $pdo->prepare("SELECT CreateUser(?, ?, ?, ?, ?)");
    $stmt->execute([$username, $first, $last, $email, $password]);

    echo "User created!";
} else {
    echo "Required fields are missing.";
}
?>
