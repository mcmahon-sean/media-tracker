<?php
session_start();
require_once '../config.php';

function getRequestToken() {
    $url = TMDB_API_URL . '/authentication/token/new?api_key=' . TMDB_API_KEY;

    $response = file_get_contents($url);
    $data = json_decode($response, true);

    if ($data && $data['success']) {
        return $data['request_token'];
    }

    return false;
}

$request_token = getRequestToken();

if ($request_token) {
    
    $_SESSION['request_token'] = $request_token;

    $auth_url = "https://www.themoviedb.org/authenticate/{$request_token}?redirect_to=http://localhost.com/http://localhost/Testing/Media_Tracker_Web/src/php/authentication/callback.php";
    header("Location: $auth_url");
    exit;
} else {
    echo "Failed to get request token from TMDB.";
}
