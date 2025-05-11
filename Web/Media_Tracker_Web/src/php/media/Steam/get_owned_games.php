<?php

require_once '../config.php';
require_once '../models/Steam/SteamGame.php';

$ownedGames = [];

$error = null;

if(isset($_SESSION['user_platform_ids']['steam'])) {
    
    $steam_id = $_SESSION['user_platform_ids']['steam'];

    // Build the Steam API URL
    $apiUrl = "https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/?" . http_build_query([
        'key' => STEAM_API_KEY,
        'steamid' => $steam_id,
        'include_appinfo' => true,
        'include_played_free_games' => true
    ]);

    // Make the API call
    $response = @file_get_contents($apiUrl);
    $data = json_decode($response, true);

    // Array to hold SteamGame objects
    $ownedGames = [];
  
    if (isset($data['response']['games'])) {
        foreach ($data['response']['games'] as $gameData) {
            $ownedGames[] = new SteamGame($gameData);
        }
    } else {
        $error = "No games found. This Steam profile might be private.";
    }
} else {
    $error = "Steam ID is missing.";
}