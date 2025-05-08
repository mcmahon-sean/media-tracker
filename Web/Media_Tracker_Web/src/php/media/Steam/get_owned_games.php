<?php

require_once '../config.php';
require_once '../models/Steam/SteamGame.php';

$steam_id = $_SESSION['user_platform_ids']['steam'];

if (!$steam_id) {
    die("Steam ID is missing.");
}

// Build the Steam API URL
$apiUrl = "https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/?" . http_build_query([
    'key' => STEAM_API_KEY,
    'steamid' => $steam_id,
    'include_appinfo' => true,
    'include_played_free_games' => true
]);



// Make the API call
$response = file_get_contents($apiUrl);
$data = json_decode($response, true);

// Array to hold SteamGame objects
$steamGames = [];

if (isset($data['response']['games'])) {
    foreach ($data['response']['games'] as $gameData) {
        $ownedGames[] = new SteamGame($gameData);
    }
} else {
    echo "No games found. This Steam profile might be private.";
}