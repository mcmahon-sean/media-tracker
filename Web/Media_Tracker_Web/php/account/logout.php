<?php

// Start session
session_start();

// Check if signed in
if ($_SESSION['signed-in'] === true) {
    // Clear all session variables
    $_SESSION = array();

    // Destroy current session
    session_destroy();

    // Return to home page
    header("Location: ../../index.php");

    // Exit php
    exit;
}

?>