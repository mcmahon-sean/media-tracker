<?php

// Require config file
require_once '../config.php';
require_once '../models/Last.FM/LastFmTrack.php';
require_once '../models/Last.FM/LastFmArtist.php';
require_once '../models/Last.FM/LastFmAlbum.php';

 // Initialize variables
    $recentTracks = [];
    $lovedTracks = [];
    $topAlbums = [];
    $topArtists = [];
    $topTracks = [];
    $error = null;

    // Username and API from config file
$apiKey = LASTFM_API_KEY;
if(isset($_SESSION['signed_in']) && $_SESSION['signed_in'] === true) {

    if(!isset($_SESSION['user_platform_ids']['lastfm'])) {
        $error = 'Last.fm Id is missing.';
    } else {
        
        $username = $_SESSION['user_platform_ids']['lastfm'];

        // URLs for the API requests
        $lovedTracksUrl = "https://ws.audioscrobbler.com/2.0/?method=user.getlovedtracks&user=$username&api_key=$apiKey&format=json&limit=10";
        $recentTracksUrl = "https://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user=$username&api_key=$apiKey&format=json&limit=10";
        $topAlbumsUrl = "https://ws.audioscrobbler.com/2.0/?method=user.gettopalbums&user=$username&api_key=$apiKey&format=json&limit=10";
        $topArtistsUrl = "https://ws.audioscrobbler.com/2.0/?method=user.gettopartists&user=$username&api_key=$apiKey&limit=10&format=json";
        $topTracksUrl = "https://ws.audioscrobbler.com/2.0/?method=user.gettoptracks&user=$username&api_key=$apiKey&format=json&limit=10";

        //loved tracks data
        $response = @file_get_contents($lovedTracksUrl);

        if ($response === false) {
            $error = "Failed to retrieve data from Last.fm.";
        } else {
            $data = json_decode($response, true);
            $trackList = $data['lovedtracks']['track'] ?? [];

            $lovedTracks = array_map(fn($item) => new LastFmTrack($item, 'lovedtracks'), $trackList);
        }

        //recent tracks data
        $response = @file_get_contents($recentTracksUrl);

        if ($response === false) {
            $error = "Failed to retrieve data from Last.fm.";
        } else {
            $data = json_decode($response, true);
            $trackList = $data['recenttracks']['track'] ?? [];

            $recentTracks = array_map(fn($item) => new LastFmTrack($item, 'recenttracks'), $trackList);
        }

        //top albums data
        $response = @file_get_contents($topAlbumsUrl);

        if ($response === false) {
            $error = "Failed to retrieve data from Last.fm.";
        } else {
            $data = json_decode($response, true);
            $albumList = $data['topalbums']['album'] ?? [];

            $topAlbums = array_map(fn($item) => new LastFmAlbum($item), $albumList);
        }

        //top artists data
        $response = @file_get_contents($topArtistsUrl);

        if ($response === false) {
            $error = "Failed to retrieve data from Last.fm.";
        } else {
            $data = json_decode($response, true);
            $artistList = $data['topartists']['artist'] ?? [];

            $topArtists = array_map(fn($item) => new LastFmArtist($item), $artistList);
        }

        //top tracks data
        $response = @file_get_contents($topTracksUrl);

        if ($response === false) {
            $error = "Failed to retrieve data from Last.fm.";
        } else {
            $data = json_decode($response, true);
            $trackList = $data['toptracks']['track'] ?? [];

            $topTracks = array_map(fn($item) => new LastFmTrack($item, 'toptracks'), $trackList);
        }
    }



} else{
    $error = "Please login to view.";
}


?>
