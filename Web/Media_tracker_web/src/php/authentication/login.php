<?php
    require_once "../config.php";

    try{
        if(isset($_POST['username']) && isset($_POST['password'])){
            $username = $_POST['username'];
            $password = $_POST['password'];

            $stmt = $pdo->prepare("SELECT public.\"AuthenticateUser\"(?, ?)");
            $stmt->execute([$username, $password]);

             // Fetch the result which will be either true or false
            $result = $stmt->fetchColumn();

            if ($result) {
                session_start();
                $_SESSION['username'] = $username;

                // Fetch the user_platform_ids for all platforms
                $idStmt = $pdo->prepare("SELECT platform_id, user_platform_id FROM useraccounts WHERE username = ?");
                $idStmt->execute([$username]);
                $ids = $idStmt->fetchAll(PDO::FETCH_ASSOC);

                // Initialize platform ID array
                $_SESSION['user_platform_ids'] = [];

                // Store IDs by platform in session
                foreach ($ids as $row) {
                    switch ($row['platform_id']) {
                        case 1:
                            $_SESSION['user_platform_ids']['steam'] = $row['user_platform_id'];
                            break;
                        case 2:
                            $_SESSION['user_platform_ids']['lastfm'] = $row['user_platform_id'];
                            break;
                        case 3:
                            $_SESSION['user_platform_ids']['tmdb'] = $row['user_platform_id'];
                            break;
                    }
                }

                echo "Logged in successfully";
            } else {
                echo "Invalid username or password";
            }
        } else {
            echo "Username and password required";
        }
    } catch (PDOException $e) {
        // Handle errors by catching the exception
        echo "Error: " . $e->getMessage();
    }
?>