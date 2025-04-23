<?php

// Required files
require_once '../config.php';
require_once '../models/Last.FM/LastFmTrack.php';

// Username and API from config file
$apiKey = LASTFM_API_KEY;

if(isset($_SESSION['signed_in']) && $_SESSION['signed_in'] === true) {
    $username = $_SESSION['user_platform_ids']['lastfm'];

    // URL for the API request
    $url = "https://ws.audioscrobbler.com/2.0/?method=user.getlovedtracks&user=$username&api_key=$apiKey&format=json&limit=5";

    // Fetch the data from the API
    $response = @file_get_contents($url);

    $tracks = [];
    $error = null;

    if ($response === false) {
        $error = "Failed to retrieve data from Last.fm.";
    } else {
        $data = json_decode($response, true);
        $trackList = $data['lovedtracks']['track'] ?? [];

        $lovedTracks = array_map(fn($item) => new LastFmTrack($item), $trackList);
    }
} else {
    $error = "Please login to view.";
}

?>