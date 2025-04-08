<?php
    require '../db.php';
    try{
        if (isset($_POST['username']) &&
        isset($_POST['platform_id']) &&
        isset($_POST['user_plat_id'])) {

            $username = $_POST['username'];
            $platformID = $_POST['platform_id'];
            $userplatID = $_POST['user_plat_id'];

            $stmt = $pdo->prepare("SELECT public.\"Add3rdPartyID\"(?, ?, ?)");
            $stmt->execute([$username, $platformID, $userplatID]);

            echo "3rd party ID saved!";
        } else {
            echo "Required fields are missing.";
        }
    }catch (PDOException $e) {
        // Handle errors by catching the exception
        echo "Error: " . $e->getMessage();
    }

    $pdo = null;
?>
