<?php

    // Start session
    session_start();

?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Top Artists</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../../../styles.css"> 
</head>
<body  class="bg-dark-primary">
    <div class="container-fluid">
        <div class="row">
        <nav class="col-md-2 d-none d-md-block sidebar bg-dark-secondary">
                <div>
                    <a class="btn btn-dark w-100" id="btn-home" href="../../../index.php" role="button">
                        Home
                    </a>
                </div>
                <hr>
                <div class="dropdown">
                    <a class="btn btn-dark dropdown-toggle w-100 media-tab" href="#" role="button" data-bs-toggle="dropdown">
                        <img src="../../assets/images/icons/icon_music.svg" class="tab-icon me-2">
                        Last.fm
                    </a>
                    <ul class="dropdown-menu">
                        <li><a class="dropdown-item" href="lastfm-all.php">All Music</a></li>
                        <li><a class="dropdown-item" href="lastfm-loved-tracks.php">Loved Tracks</a></li>
                        <li><a class="dropdown-item" href="lastfm-recent-tracks.php">Recent Tracks</a></li>
                        <li><a class="dropdown-item" href="lastfm-top-albums.php">Top Albums</a></li>
                        <li><a class="dropdown-item" href="lastfm-top-artists.php">Top Artists</a></li>
                        <li><a class="dropdown-item" href="lastfm-top-tracks.php">Top Tracks</a></li>
                    </ul>
                </div>

                
                <div class="dropdown mt-2">
                    <a class="btn btn-dark dropdown-toggle w-100 media-tab" href="#" role="button" data-bs-toggle="dropdown">
                        <img src="../../assets/images/icons/icon_movies.svg" class="tab-icon me-2">
                        TMDB
                    </a>
                    <ul class="dropdown-menu">
                        <li><a class="dropdown-item" href="#">All Movies</a></li>
                        <li><a class="dropdown-item" href="#">Last Played</a></li>
                    </ul>
                </div>

                
                <div class="dropdown mt-2">
                    <a class="btn btn-dark dropdown-toggle w-100 media-tab" href="#" role="button" data-bs-toggle="dropdown">
                        <img src="../../assets/images/icons/icon_games.svg" class="tab-icon me-2">
                        Steam
                    </a>
                    <ul class="dropdown-menu">
                        <li><a class="dropdown-item" href="#">All Games</a></li>
                        <li><a class="dropdown-item" href="#">Last Played</a></li>
                    </ul>
                </div>
                
            </nav>

            <main class="col-md-10 ms-sm-auto px-4">
                <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
                    <h2>Welcome Back</h2>
                </div>

                <div class="card bg-dark text-light mb-4">
                    <div class="card-body">
                        <input type="text" class="form-control" placeholder="Search Titles...">
                    </div>
                </div>

                <div class="table-responsive">
                    <?php if (isset($error)): ?>
                        <p><?php echo $error; ?></p>
                    <?php else: ?>
                        <div class="form-container">
                            <form id="loginForm" action="../authentication/create_user.php" method="post">
                                <h3>Login</h3>
                                <label>Username:</label><br>
                                <input type="text" name="username" required><br>
                                <label>First Name:</label><br>
                                <input type="text" name="first_name" required><br>
                                <label>Last Name:</label><br>
                                <input type="text" name="last_name" required><br>
                                <label>Email:</label><br>
                                <input type="email" name="email" required><br>
                                <label>Password:</label><br>
                                <input type="password" name="password" required><br><br>
                                <input type="submit" value="Login">
                            </form>
                        </div>
                    <?php endif; ?>
                </div>
            </main>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script src="../../../supabaseClient.js" type="module"></script>
    <script src="../../../script.js" type="module"></script>

</body>
</html>