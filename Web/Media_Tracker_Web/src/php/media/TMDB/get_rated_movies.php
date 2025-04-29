<?php

require_once '../config.php';
require_once '../models/TMDB/TMDBRatedMovie.php';

// Check if user is signed in
if (isset($_SESSION['signed_in']) && $_SESSION['signed_in'] === true) {

    $movies = [];
    $error = null;

    // Check if session_id and user_plat_id are set
    if (!isset($_SESSION['user_platform_ids']['tmdb'])) {
        $error = "TMDB session or user ID is missing.";
    } else {
        $session_id = $_SESSION['user_platform_ids']['tmdb'];

        $account_url = TMDB_API_URL . "/account?api_key=" . TMDB_API_KEY . "&session_id=" . $session_id;

        // Now fetch account_id first
        $account_response = file_get_contents($account_url);
        $account_data = json_decode($account_response, true);

        if (!$account_data || !isset($account_data['id'])) {
            $error = "Failed to retrieve TMDB account information.";
        } else {
            $account_id = $account_data['id'];

            // Now you can fetch rated movies
            $url = TMDB_API_URL . "/account/{$account_id}/rated/movies?api_key=" . TMDB_API_KEY . "&session_id=" . $session_id;
        }

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
                foreach ($data['results'] as $movieJson) {
                    $ratedMovies[] = new TMDBRatedMovie($movieJson);
                }
            } else {
                $error = "No rated movies found.";
            }
        }

        curl_close($ch);
    }
} else {
    $error = "Please login to view this content.";
}
