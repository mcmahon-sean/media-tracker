<?php
    require '../db.php';
    require_once '../tools.php';

    session_start(); // Ensure session is started

    try {
        // Check if session variables are set for username and platform
        if (isset($_SESSION['username']) && isset($_SESSION['user_platform_ids'])) {
            $username = $_SESSION['username']; // Get the username from the session

            // Check if platform_id and user_plat_id are available in the session
            $platformId = null;
            $userPlatId = null;

            // Check if the session has the user platform IDs set for various platforms
            if (isset($_SESSION['user_platform_ids']['steam'])) {
                $platformId = 1; // Steam
                $userPlatId = $_SESSION['user_platform_ids']['steam'];
            } elseif (isset($_SESSION['user_platform_ids']['lastfm'])) {
                $platformId = 2; // Last.fm
                $userPlatId = $_SESSION['user_platform_ids']['lastfm'];
            } elseif (isset($_SESSION['user_platform_ids']['tmdb'])) {
                $platformId = 3; // TMDb
                $userPlatId = $_SESSION['user_platform_ids']['tmdb'];
            }

            if ($platformId && $userPlatId) {
                // Prepare and execute the delete statement using values from the session
                $stmt = $pdo->prepare("SELECT public.delete_3rd_party(?, ?, ?)");
                $stmt->execute([
                    $username,
                    $platformId,
                    $userPlatId
                ]);

                $result = $stmt->fetchColumn();
                if ($result) {
                    // Remove the corresponding platform from the session after successful deletion
                    switch ($platformId) {
                        case 1: // Steam
                            unset($_SESSION['user_platform_ids']['steam']);
                            break;
                        case 2: // Last.fm
                            unset($_SESSION['user_platform_ids']['lastfm']);
                            break;
                        case 3: // TMDb
                            unset($_SESSION['user_platform_ids']['tmdb']);
                            break;
                    }
                
                    // Optional: Clean up the entire array if it's empty now
                    if (empty($_SESSION['user_platform_ids'])) {
                        unset($_SESSION['user_platform_ids']);
                    }
                
                    echo $result;
                    header("Location: ../views/manage_user.php");
                } else {
                    echo "No result returned.";
                }
                
            } else {
                echo 'Error: Platform ID or User Platform ID is missing in the session.';
            }
        } else {
            echo 'Error: User session is not set properly.';
        }
    } catch (PDOException $e) {
        echo 'Database error: ' . $e->getMessage();
    }

    // Clean up and close the database connection
    $pdo = null;
?>
