<?php
session_start();
require_once '../config.php';
require_once '../db.php';

// Initial call to TMDB
if (!isset($_GET['callback'])) {
    // Grab and store user-provided info from the GET query
    $user_plat_id = $_GET['user_plat_id'] ?? null;
    $platform_id  = isset($_GET['platform_id']) ? (int)$_GET['platform_id'] : null;

    if (!$user_plat_id || !$platform_id) {
        die('Missing platform_id, or user_plat_id.');
    }

    // Set SESSION
    $_SESSION['user_plat_id'] = $user_plat_id;
    $_SESSION['platform_id'] = $platform_id;

    try {
        // Request token from TMDB
        $url = TMDB_API_URL . '/authentication/token/new?api_key=' . TMDB_API_KEY;
        $response = file_get_contents($url);
        $data = json_decode($response, true);

        if ($data && $data['success']) {
            $request_token = $data['request_token'];
            $_SESSION['request_token'] = $request_token;

            // Build TMDB redirect URL
            $redirect_url = urlencode("http://localhost/media-tracker/Web/Media_Tracker_Web/src/php/authentication/auth_tmdb.php?callback=true");
            $auth_url = "https://www.themoviedb.org/authenticate/{$request_token}?redirect_to={$redirect_url}";

            header("Location: $auth_url");
            exit;
        } else {
            throw new Exception("Failed to retrieve request token.");
        }

    } catch (Exception $e) {
        error_log("Error: " . $e->getMessage());
        echo "TMDB authentication failed.";
        exit;
    }
}

// Callback from TMDB
if ($_GET['callback'] === 'true') {

    if (!isset($_SESSION['request_token'])) {
        die('TMDB request token missing from session.');
    }

    $request_token = $_SESSION['request_token'];

    // Exchange token for a session ID
    $session_url = TMDB_API_URL . '/authentication/session/new?api_key=' . TMDB_API_KEY;
    $options = [
        'http' => [
            'method'  => 'POST',
            'header'  => "Content-Type: application/json\r\n",
            'content' => json_encode(['request_token' => $request_token]),
        ]
    ];
    $context = stream_context_create($options);
    $response = file_get_contents($session_url, false, $context);
    $data = json_decode($response, true);

    if (!$data || !$data['success']) {
        die('Failed to create TMDB session.');
    }

    $session_id = $data['session_id'];
    $_SESSION['session_id'] = $session_id;

    // Get TMDB account details
    $account_url = TMDB_API_URL . "/account?api_key=" . TMDB_API_KEY . "&session_id=" . $session_id;
    $account_response = file_get_contents($account_url);
    $account_data = json_decode($account_response, true);

    if ($account_data && isset($account_data['id'])) {
        $user_tmdb_id = $account_data['id'];

        // Replace the original user_plat_id with TMDB ID
        $_SESSION['user_plat_id'] = $user_tmdb_id;

        // Redirect to your DB insert/update logic
        header("Location: ../database/add_3rd_party_id.php?auth_tmdb=true");
        exit;
    } else {
        echo "Failed to retrieve user info from TMDB.";
    }
}
