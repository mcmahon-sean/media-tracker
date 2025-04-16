<?php

// Start $_SESSION
session_start();

?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Media Tracker</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://kit.fontawesome.com/a076d05399.js" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="styles.css"> 
</head>
<body>

    <div class="container-fluid">
        <div class="row">
          
            <nav class="col-md-2 d-none d-md-block sidebar">

            <div class="mt-2">
                    <a class="btn btn-dark w-100 mt-2" href="index.php" role="button">
                        Home
                    </a>
                </div>

                <div class="mt-2">
                    <a class="btn btn-dark w-100 mt-2" href="php/games/steam-info.php" role="button">
                        Steam
                    </a>
                </div>

                <div class="mt-2">
                    <a class="btn btn-dark w-100 mt-2" href="php/music/lastfm-info.php" role="button">
                        Last.fm
                    </a>
                </div>

                <div class="mt-2">
                    <a class="btn btn-dark w-100 mt-2" href="php/movies/tmdb-info.php" role="button">
                        TMDB
                    </a>
                </div>
                
            </nav>

            <main class="col-md-10 ms-sm-auto px-4">
                <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
                    <h2>Media Tracker</h2>
                </div>

                <div class="card bg-dark text-light mb-4">
                    <div class="card-body">
                        <input type="text" class="form-control" placeholder="Search Titles...">
                    </div>
                </div>
                <div>
                    <form id="sign-in-form" method="post" action="php/account/login.php">
                        <label for="username">Username:</label><br>
                        <input type="text" id="username" name="username"><br>
                        <label for="password">Password:</label><br>
                        <input type="password" id="password" name="password"><br>
                        <button type="submit">Sign In</button>
                    </form>
                </div>
            </main>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script src="supabaseClient.js" type="module"></script>
    <script src="script.js" type="module"></script>
</body>
</html>
