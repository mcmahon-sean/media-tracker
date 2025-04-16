<?php

// Start $_SESSION
session_start();

// Required
require_once "../configs/supabase-config.php";

if (isset($_POST['platform']) && 
    isset($_POST['platformid'])) {

    // Initialize variables
    $username = $_SESSION['username'];
    $platform = $_POST['platform'];
    $platformid = $_POST['platformid'];

    $url = SUPABASE_URL . "/rest/v1/rpc/add_3rd_party_id";
    $apiKey = SUPABASE_API_KEY;

    // Set parameters
    $params = [
        'username_input' => $username,
        'platform_id_input' => $platform,
        'user_plat_id_input' => $platformid
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

    // Check for successful authentication (Updated ID, Added ID)
    if ($data === "Added ID" || $data === "Updated ID") {
        // Redirect to added/updated platform
        if ($platform === "1") {
            header("Location: ../games/steam-info.php");
        } elseif ($platform === "2") {
            header("Location: ../music/lastfm-info.php");
        } else {
            header("Location: ../movies/tmdb-info.php");
        }
        exit;
    } else {
        echo 'ERROR: Account NOT linked.';
        exit;
    }
}

?>
