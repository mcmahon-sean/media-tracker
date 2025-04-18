<?php

// Required files
require_once '../config.php';
require_once '../models/Last.FM/LastFmTrack.php';

// Username and API from config file
$apiKey = LASTFM_API_KEY;
$username = LASTFM_USERNAME;

// URL for the API request
$url = "https://ws.audioscrobbler.com/2.0/?method=user.gettoptracks&user=$username&api_key=$apiKey&format=json&limit=5";

// Fetch the data from the API
$response = file_get_contents($url);

$tracks = [];
$error = null;

if ($response === false) {
    $error = "Failed to retrieve data from Last.fm.";
} else {
    $data = json_decode($response, true);
    $trackList = $data['toptracks']['track'] ?? [];

    $topTracks = array_map(fn($item) => new LastFmTrack($item), $trackList);
}
