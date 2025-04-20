<?php
    require '../db.php';

    try{
        if (isset($_POST['username']) &&
        isset($_POST['media_id'])) {

            $username = $_POST['username'];
            $mediaID = $_POST['media_id'];
    
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