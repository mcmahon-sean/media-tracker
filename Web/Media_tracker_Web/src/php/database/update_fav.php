<?php
    require '../db.php';

    try{
        if (isset($_POST['username']) &&
        isset($_POST['media_id'])) {

            $username = sanitizeString($_POST['username']);
            $mediaID = sanitizeInt($_POST['media_id']);
    
            $stmt = $pdo->prepare("SELECT public.update_fav(?, ?)");
            $stmt->execute([$username, $mediaID]);
    
            // Get the returned message from the function
            $result = $stmt->fetchColumn();
    
            echo $result ?: "Favorite updated!";
    
        } else {
            echo "Required fields are missing.";
        }
    } catch (PDOException $e) {
        echo "Error: " . $e->getMessage();
    }
    
    $pdo = null;
?>