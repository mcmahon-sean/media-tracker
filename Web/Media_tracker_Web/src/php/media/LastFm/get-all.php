<?php

// Require config file
require_once('../../configs/lastfm-config.php');

// Username and API from config file
$username = LASTFM_USERNAME;
$apiKey = $_SESSION['user_platform_ids']['lastfm'];

// URLs for the API requests
$lovedTracksUrl = "https://ws.audioscrobbler.com/2.0/?method=user.getlovedtracks&user=$username&api_key=$apiKey&format=json&limit=5";
$recentTracksUrl = "https://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user=$username&api_key=$apiKey&format=json&limit=5";
$topAlbumsUrl = "https://ws.audioscrobbler.com/2.0/?method=user.gettopalbums&user=$username&api_key=$apiKey&format=json&limit=5";
$topArtistsUrl = "https://ws.audioscrobbler.com/2.0/?method=user.gettopartists&user=$username&api_key=$apiKey&format=json&limit=5";
$topTracksUrl = "https://ws.audioscrobbler.com/2.0/?method=user.gettoptracks&user=$username&api_key=$apiKey&format=json&limit=5";

// Initialize variables
$recentTracks = [];
$lovedTracks = [];
$topAlbums = [];
$topArtists = [];
$topTracks = [];
$error = '';

// Fetch data function
function fetchData($url, &$data, &$error, $dataName) {
    $json = @file_get_contents($url);
    if ($json === FALSE) {
        $error = "Failed to fetch $dataName. Please check your username or API key.";
    } else {
        $data = json_decode($json, true);
    }
}

// Fetch all data using fetchData()
fetchData($recentTracksUrl, $recentTracks, $error, 'recent tracks');
fetchData($lovedTracksUrl, $lovedTracks, $error, 'loved tracks');
fetchData($topAlbumsUrl, $topAlbums, $error, 'top albums');
fetchData($topArtistsUrl, $topArtists, $error, 'top artists');
fetchData($topTracksUrl, $topTracks, $error, 'top tracks');

?>
