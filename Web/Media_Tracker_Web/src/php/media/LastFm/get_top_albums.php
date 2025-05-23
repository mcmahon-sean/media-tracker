<?php

// Required files
require_once '../config.php';
require_once '../models/Last.FM/LastFmAlbum.php';

$topAlbums = [];

// Username and API from config file
$apiKey = LASTFM_API_KEY;

if(isset($_SESSION['signed_in']) && $_SESSION['signed_in'] === true) {
    
    if(!isset($_SESSION['user_platform_ids']['lastfm'])) {
        $error = 'Last.fm Id is missing.';
    } else {
        $username = $_SESSION['user_platform_ids']['lastfm'];

        // URL for the API request
        $url = "https://ws.audioscrobbler.com/2.0/?method=user.gettopalbums&user=$username&api_key=$apiKey&format=json";

        // Fetch the data from the API
        $response = @file_get_contents($url);

        $albums = [];
        $error = null;

        if ($response === false) {
            $error = "Failed to retrieve data from Last.fm.";
        } else {
            $data = json_decode($response, true);
            $albumList = $data['topalbums']['album'] ?? [];

            $topAlbums = array_map(fn($item) => new LastFmAlbum($item), $albumList);
        }
    }

    
} else {
    $error = "Please login to view.";
}



?>

