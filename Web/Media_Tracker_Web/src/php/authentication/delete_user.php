<?php
    require '../db.php';
    require_once '../tools.php';

    // Start the session to access session variables
    session_start();

    try {
        // Check if the session contains the logged-in username
        if (isset($_SESSION['username'])) {
            $username = $_SESSION['username'];

            // Sanitize the username for safety
            $username = sanitizeString($username);

            // Prepare and execute the delete user query
            $stmt = $pdo->prepare("SELECT public.delete_user(?)");
            $stmt->execute([$username]);

            // Fetch the result (if any) from the stored procedure
            $result = $stmt->fetchColumn();

            // If the deletion was successful, return a success message
            if ($result) {
                // Optionally, you could log the user out here and redirect them to a login page
                session_destroy(); // Uncomment to destroy the session
                echo "Your account has been deleted successfully.";
                header("Location: ../../../index.php");
                exit;
            } else {
                echo "There was an issue deleting your account. Please try again later.";
            }
        } else {
            echo "Error: You must be logged in to delete your account.";
        }
    } catch (PDOException $e) {
        echo "Database error: " . $e->getMessage();
    }

    // Close the PDO connection
    $pdo = null;
?>
