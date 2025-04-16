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
                    <h2>Link 3rd Party Platform</h2>
                </div>
                <div>
                    <form id="link-platform" method="post" action="php/account/link.php">
                        <br>
                        <div class="form-group col-3">
                            <label for="platform">Select Platform:</label>
                            <select class="form-control" id="platform" name="platform">
                                <option value="1">Steam</option>
                                <option value="2">Last.fm</option>
                                <option value="3">TMDB</option>
                            </select>
                        </div>
                        <br>
                        <div class="form-group  col-3">
                            <label for="platformid">Platform ID:</label>
                            <input type="text" class="form-control" id="platformid" name="platformid" required>
                        </div>
                        <br>
                        <button type="submit">Link Platform</button>
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
