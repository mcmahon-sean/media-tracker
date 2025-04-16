<?php

// Required
require_once "../configs/supabase-config.php";

if (
    isset($_POST['username']) && 
    isset($_POST['firstname']) &&
    isset($_POST['email']) &&
    isset($_POST['password']) &&
    isset($_POST['cpassword'])) {

    // Initialize variables
    $password = $_POST['password'];
    $cpassword = $_POST['cpassword'];

    if ($password !== $cpassword) {
        $_SESSION['message'] = "Passwords do not match.";
        // Redirect to home page
        header("Location: ../../index.php");
        exit;
    }

    $username = $_POST['username'];
    $firstname = $_POST['firstname'];
    $lastname = isset($_POST['lastname']) ? $_POST['lastname'] : null;
    $email = $_POST['email'];

    $url = SUPABASE_URL . "/rest/v1/rpc/CreateUser";
    $apiKey = SUPABASE_API_KEY;

    $params = [
        'usernamevar' => $username,
        'firstnamevar' => $firstname,
        'lastnamevar' => $lastname,
        'emailvar' => $email,
        'passwordvar' => $password
    ];

    $ch = curl_init($url);
    curl_setopt($ch, CURLOPT_HTTPHEADER, [
        'apikey: ' . $apiKey,
        'Authorization: Bearer ' . $apiKey,
        'Content-Type: application/json',
        'Accept: application/json'
    ]);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_POST, true);
    curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($params));
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);

    $response = curl_exec($ch);

    if ($response === false) {
        echo 'cURL error: ' . curl_error($ch);
        curl_close($ch);
        exit;
    }

    $http_status = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);

    $data = json_decode($response, true);

    if ($data === true) {
        session_start();
        $_SESSION['username'] = $username;
        $_SESSION['message'] = "Account created successfully! Please sign in.";
        // Redirect to home page
        header("Location: ../../index.php");
        exit;
    } else {
        session_start();
        $_SESSION['message'] = "Account creation failed. Username may already exist or something went wrong.";
        // Redirect to home page
        header("Location: ../../index.php");
        exit;
    }
}
?>
