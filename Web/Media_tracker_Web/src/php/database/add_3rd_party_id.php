<?php
session_start();
require_once '../db.php';
require_once '../tools.php';

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    // Regular form submission (Steam or Last.FM)
    if (isset($_POST['platform_id'], $_POST['user_plat_id'])) {
        $platform_id = sanitizeInt($_POST['platform_id']);
        $user_plat_id = sanitizeString($_POST['user_plat_id']);
    } else {
        die('Missing POST data.');
    }
} elseif (isset($_GET['auth_tmdb']) && $_GET['auth_tmdb'] === 'true') {
    // TMDB redirect
    if (isset($_SESSION['platform_id'], $_SESSION['user_plat_id'])) {
        $platform_id = sanitizeInt($_SESSION['platform_id']);
        $user_plat_id = sanitizeString($_SESSION['user_plat_id']);
    } else {
        die('Missing TMDB session data.');
    }
} else {
    die('Invalid request.');
}

// Update databse and session
try {
    $username = sanitizeString($_SESSION['username']);
    $stmt = $pdo->prepare("SELECT public.add_3rd_party_id(?, ?, ?)");
    $stmt->execute([$username, $platform_id, $user_plat_id]);

    // Update session variables
    switch ($platform_id) {
        case 1:
            $_SESSION['user_platform_ids']['steam'] = $user_plat_id;
            break;
        case 2:
            $_SESSION['user_platform_ids']['lastfm'] = $user_plat_id;
            break;
        case 3:
            $_SESSION['user_platform_ids']['tmdb'] = $user_plat_id;
            break;
    }

    // Redirect after success
    header("Location: ../../../index.php");
    exit;

} catch (PDOException $e) {
    echo "Error linking platform: " . $e->getMessage();
}

$pdo = null;
?>
