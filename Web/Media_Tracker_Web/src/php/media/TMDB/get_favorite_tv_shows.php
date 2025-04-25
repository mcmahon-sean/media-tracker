<?php

require_once '../config.php';
require_once '../models/TMDB/TMDBTvShow.php';

// Check if user is signed in
if (isset($_SESSION['signed_in']) && $_SESSION['signed_in'] === true) {

    $tvShows = [];
    $error = null;

    // Check if session_id and user_plat_id are set
    if (!isset($_SESSION['session_id']) || !isset($_SESSION['user_plat_id'])) {
        $error = "TMDB session or user ID is missing.";
    } else {
        $session_id = $_SESSION['session_id'];
        $account_id = $_SESSION['user_plat_id']; 

        $url = TMDB_API_URL . "/account/{$account_id}/favorite/tv?api_key=" . TMDB_API_KEY . "&session_id=" . $session_id;

        $ch = curl_init($url);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
        curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false); // Remove before deployment

        $response = curl_exec($ch);
        $httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);

        if (curl_errno($ch)) {
            $error = "cURL error (" . curl_errno($ch) . "): " . curl_error($ch);
        } elseif ($httpCode !== 200) {
            $error = "TMDB returned HTTP code $httpCode.<br>Response: $response";
        } else {
            $data = json_decode($response, true);
            if (isset($data['results'])) {
                foreach ($data['results'] as $tvJson) {
                    $favoriteTvShows[] = new TMDBTvShow($tvJson);
                }
            } else {
                $error = "No favorite TV shows found.";
            }
        }

        curl_close($ch);
    }
} else {
    $error = "Please login to view this content.";
}
?>
