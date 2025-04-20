<?php

// Required
require_once "../config.php";

if (isset($_POST['username']) && 
    isset($_POST['password'])) {

    // Initialize variables
    $username = $_POST['username'];
    $password = $_POST['password'];

    $authUrl = SUPABASE_URL . "/rest/v1/rpc/auth_user";
    $userUrl = SUPABASE_URL . "/rest/v1/useraccounts";
    $platformUrl = SUPABASE_URL . "/rest/v1/platforms";
    $apiKey = SUPABASE_API_KEY;

    $params = [
        'username_input' => $username,
        'password_input' => $password
    ];

    $ch = curl_init($authUrl);
    curl_setopt($ch, CURLOPT_HTTPHEADER, [
        'apikey: ' . $apiKey,
        'Authorization: Bearer ' . $apiKey,
        'Content-Type: application/json',
        'Accept: application/json'
    ]);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_POST, true);
    curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($params));
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);

    $response = curl_exec($ch);

    if ($response === false) {
        echo 'cURL error: ' . curl_error($ch);
        curl_close($ch);
        exit;
    }

    //$http_status = curl_getinfo($ch, CURLINFO_HTTP_CODE);
    curl_close($ch);

    $data = json_decode($response, true);

    // Check for returned token
    if (!empty($data)) {
        session_start();
        $_SESSION['username'] = $username;
        $_SESSION['signed-in'] = true;
        $_SESSION['user_platform_ids'] = [];

        // Function to GET from Supabase
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

        // Loop through platform IDs 1, 2, 3
        for ($i = 1; $i <= 3; $i++) {
            $userQuery = http_build_query([
                'username' => 'eq.' . $username,
                'platform_id' => 'eq.' . $i,
                'select' => '*'
            ]);

            $userData = supabaseGet("$userUrl?$userQuery", $apiKey);
            $user = $userData[0] ?? null;

            if ($user) {
                switch ($i) {
                    case 1:
                        $_SESSION['user_platform_ids']['steam'] = $user['user_platform_id'];
                        break;
                    case 2:
                        $_SESSION['user_platform_ids']['lastfm'] = $user['user_platform_id'];
                        break;
                    case 3:
                        $_SESSION['user_platform_ids']['tmdb'] = $user['user_platform_id'];
                        break;
                }
            }
        }

        // Redirect to home page
        header("Location: ../../../index.php");
        exit;
        
    } else {

        // Redirect to home page
        header("Location: ../../../index.php");
        exit;

    }
}

?>
