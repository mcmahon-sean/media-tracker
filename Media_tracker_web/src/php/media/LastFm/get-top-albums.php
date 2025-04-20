<?php

// Required files
require_once '../config.php';
require_once '../models/Last.FM/LastFmAlbum.php';

// Username and API from config file
$apiKey = LASTFM_API_KEY;
$username = LASTFM_USERNAME;

// URL for the API request
$url = "https://ws.audioscrobbler.com/2.0/?method=user.gettopalbums&user=$username&api_key=$apiKey&format=json&limit=5";

// Fetch the data from the API
$response = file_get_contents($url);

$albums = [];
$error = null;

if ($response === false) {
    $error = "Failed to retrieve data from Last.fm.";
} else {
    $data = json_decode($response, true);
    $albumList = $data['topalbums']['album'] ?? [];

    $topAlbums = array_map(fn($item) => new LastFmAlbum($item), $albumList);
}

?>

