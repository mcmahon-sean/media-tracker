<?php
    require '../db.php';
    require_once '../tools.php';
    session_start();

    try{
        if (isset($_POST['username'])) {
            
            $username = sanitizeString($_POST['username']);
            $firstName = isset($_POST['first_name']) ? sanitizeString($_POST['first_name']) : null;
            $lastName = isset($_POST['last_name']) ? sanitizeString($_POST['last_name']) : null;
            $email = isset($_POST['email']) ? sanitizeString($_POST['email']) : null;
            $password = isset($_POST['password']) ? sanitizeString($_POST['password']) : null;

                $stmt = $pdo->prepare("SELECT public.update_user(?, ?, ?, ?, ?)");
                $stmt->execute([
                    $username,
                    $firstName,
                    $lastName,
                    $email,
                    $password
                ]);
                $result = $stmt->fetchColumn();
                if($result){
                    $userStmt = $pdo->prepare("SELECT first_name, last_name, email FROM users WHERE username = ?");
                    $userStmt->execute([$username]);
                    $user = $userStmt->fetch(PDO::FETCH_ASSOC);

                    if($user){
                        $_SESSION['firstName'] = $user['first_name'];
                        $_SESSION['lastName'] = $user['last_name'];
                        $_SESSION['email'] = $user['email'];
                    }
                    header("Location: ../views/manage_user.php");
                }else{
                    echo "No result returned.";
                }
        } else {
            echo 'Error: All fields are required.';
        }
    }catch (PDOException $e) {
        echo 'Database error: ' . $e->getMessage();
    }
    $pdo = null;
?>