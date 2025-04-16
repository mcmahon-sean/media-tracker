<?php

// Required
require_once "../configs/supabase-config.php";

if (isset($_POST['username']) && 
    isset($_POST['password'])) {

    // Initialize variables
    $username = $_POST['username'];
    $password = $_POST['password'];

    $url = SUPABASE_URL . "/rest/v1/rpc/auth_user";
    $apiKey = SUPABASE_API_KEY;

    $params = [
        'username_input' => $username,
        'password_input' => $password
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

    // Check for returned token
    if (!empty($data)) {
        session_start();
        $_SESSION['username'] = $username;
        $_SESSION['signed-in'] = true;
        $_SESSION['message'] = "Logged in successfully! ";

        // Redirect to home page
        header("Location: ../../index.php");
        exit;
    } else {
        session_start();
        $_SESSION['message'] = "Sign in failed. Invalid username or password. ";

        // Redirect to home page
        header("Location: ../../index.php");
        exit;
    }
}

?>
