<?php

// Required files
require_once '../config.php';
require_once '../models/Last.FM/LastFmArtist.php';

// Username and API from config file
$apiKey = LASTFM_API_KEY;
$username = LASTFM_USERNAME;

// URL for the API request
$url = "https://ws.audioscrobbler.com/2.0/?method=user.gettopartists&user=$username&api_key=$apiKey&format=json";

// Fetch the data from the API
$response = @file_get_contents($url);

$artists = [];
$error = null;

if ($response === false) {
    $error = "Failed to retrieve data from Last.fm.";
} else {
    $data = json_decode($response, true);
    $artistList = $data['topartists']['artist'] ?? [];

    $topArtists = array_map(fn($item) => new LastFmArtist($item), $artistList);
}

?>