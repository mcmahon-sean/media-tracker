<?php
    require '../db.php';
    try{
        if (isset($_POST['username']) &&
        isset($_POST['platform_id']) &&
        isset($_POST['user_plat_id'])) {

            $username = $_POST['username'];
            $platformID = $_POST['platform_id'];
            $userplatID = $_POST['user_plat_id'];

            $stmt = $pdo->prepare("SELECT public.add_3rd_party_id(?, ?, ?)");
            $stmt->execute([$username, $platformID, $userplatID]);

            session_start();         
            $_SESSION['user_platform_ids']['steam'] = $userplatID;
            
            header("Location: ../../../index.php");
            exit;
        } else {
            echo "Required fields are missing.";
        }
    }catch (PDOException $e) {
        // Handle errors by catching the exception
        echo "Error: " . $e->getMessage();
    }

    $pdo = null;
?>
