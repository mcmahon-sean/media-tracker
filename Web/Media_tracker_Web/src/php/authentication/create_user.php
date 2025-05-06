<?php
    require '../db.php';
    
    try{
        if (isset($_POST['username']) &&
        isset($_POST['first_name']) &&
        isset($_POST['email']) &&
        isset($_POST['password'])) {

        $username = $_POST['username'];
        $first = $_POST['first_name'];
        $last = $_POST['last_name'] ?? null;
        $email = $_POST['email'];
        $password = $_POST['password'];

        $stmt = $pdo->prepare("SELECT public.\"CreateUser\"(?, ?, ?, ?, ?)");
        $stmt->execute([$username, $first, $last, $email, $password]);

        echo "User created!";
        header("Location: ../../../index.php");
        } else {
            echo "Required fields are missing.";
        }
    } catch (PDOException $e) {
        // Handle errors by catching the exception
        echo "Error: " . $e->getMessage();
    }

    $pdo = null;
?>