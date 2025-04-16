<?php

// Start $_SESSION
session_start();

// Required
require_once "../configs/supabase-config.php";

// Check if username is set
if (!isset($_SESSION['username'])) {
    echo "You're not signed in.";
    exit;
}

$userUrl = SUPABASE_URL . "/rest/v1/useraccounts";
$platformUrl = SUPABASE_URL . "/rest/v1/platforms";
$apiKey = SUPABASE_API_KEY;

$userQuery = http_build_query([
    'username' => 'eq.' . $_SESSION['username'],
    'platform_id' => 'eq.1',
    'select' => '*'
]);

function supabaseGet($url, $apiKey) {
    $ch = curl_init($url);
    curl_setopt($ch, CURLOPT_HTTPHEADER, [
        "apikey: $apiKey",
        "Authorization: Bearer $apiKey",
        "Accept: application/json"
    ]);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
    $response = curl_exec($ch);
    curl_close($ch);
    return json_decode($response, true);
}

$userData = supabaseGet("$userUrl?$userQuery", $apiKey);
$user = $userData[0] ?? null;

if (!$user) {
    echo "No user account found for that platform.";
    exit;
}

$platformQuery = http_build_query([
    'platform_id' => 'eq.1',
    'select' => '*'
]);
$platformData = supabaseGet("$platformUrl?$platformQuery", $apiKey);
$platform = $platformData[0] ?? null;

if (!$platform) {
    echo "Platform not found.";
    exit;
}

$_SESSION['user_platform_id'] = $user['user_platform_id'];

header("Location: display-steam.php");
exit;

?>
