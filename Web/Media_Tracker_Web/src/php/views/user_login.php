<?php

    // Start session
    session_start();

?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Login</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../../../styles.css">

    <?php if(isset($_GET['invalid']) && $_GET['invalid'] == 'true'): ?>
        <script>alert('Invalid username or password.');</script>
    <?php endif; ?>
</head>
<body  class="bg-dark-primary">
    <div class="container-fluid">
        <div class="row">
        <nav class="col-md-2 d-none d-md-block sidebar bg-dark-secondary">
                <div>
                    <a class="btn btn-dark w-100" id="btn-home" href="../../../index.php" role="button">
                        Home
                    </a>
                    <a
                        class="btn btn-dark w-100 mt-2"
                        id="btn-home"
                        href="./manage_user.php"
                        role="button"
                    >
                        Manager user
                    </a>
                </div>
                <hr>
                <div class="dropdown">
                    <a class="btn btn-dark dropdown-toggle w-100 media-tab" href="#" role="button" data-bs-toggle="dropdown">
                        <img src="../../assets/images/icons/icon_music.svg" class="tab-icon me-2">
                        Last.fm
                    </a>
                    <ul class="dropdown-menu">
                        <li><a class="dropdown-item" href="lastfm_all.php">All Music</a></li>
                        <li><a class="dropdown-item" href="lastfm_loved_tracks.php">Loved Tracks</a></li>
                        <li><a class="dropdown-item" href="lastfm_recent_tracks.php">Recent Tracks</a></li>
                        <li><a class="dropdown-item" href="lastfm_top_albums.php">Top Albums</a></li>
                        <li><a class="dropdown-item" href="lastfm_top_artists.php">Top Artists</a></li>
                        <li><a class="dropdown-item" href="lastfm_top_tracks.php">Top Tracks</a></li>
                    </ul>
                </div>

                
                <div class="dropdown mt-2">
                    <a class="btn btn-dark dropdown-toggle w-100 media-tab" href="#" role="button" data-bs-toggle="dropdown">
                        <img src="../../assets/images/icons/icon_movies.svg" class="tab-icon me-2">
                        TMDB
                    </a>
                    <ul class="dropdown-menu">
                        <li><a class="dropdown-item" href="tmdb_favorite_movies.php">Favorite Movies</a></li>
                        <li><a class="dropdown-item" href="tmdb_rated_movies.php">Rated Movies</a></li>
                        <li><a class="dropdown-item" href="tmdb_favorite_tv_shows.php">Favorite TV Shows</a></li>
                        <li><a class="dropdown-item" href="tmdb_rated_tv_shows.php">Rated TV Shows</a></li>
                    </ul>
                </div>

                
                <div class="dropdown mt-2">
                    <a class="btn btn-dark dropdown-toggle w-100 media-tab" href="#" role="button" data-bs-toggle="dropdown">
                        <img src="../../assets/images/icons/icon_games.svg" class="tab-icon me-2">
                        Steam
                    </a>
                    <ul class="dropdown-menu">
                        <li><a class="dropdown-item" href="steam_owned_games.php">Owned Games</a></li>
                    </ul>
                </div>
                
            </nav>

            <main class="col-md-10 ms-sm-auto px-4">
                <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
                    <h2 class="display-6">
                        Media Tracker
                        <small class="text-title-secondary">
                            Login
                        </small>
                    </h2>
                </div>

                <div class="table-responsive">
                    <?php if (isset($error)): ?>
                        <p><?php echo $error; ?></p>
                    <?php else: ?>
                        <div class="form-container">
                            <form id="loginForm" action="../authentication/login.php" method="post">
                            <h3 class="mb-4">Login</h3>
                                <div class="row mb-3 mx-1">
                                    <label class="form-label col-xl-1 col-lg-2 col-md-3 lead" for="username">
                                        Username:
                                    </label>
                                    <div class="col-xl-5 col-lg-8 col-md">
                                        <input class="form-control" type="text" name="username" required>
                                    </div>
                                </div>
                                <div class="row mb-3 mx-1">
                                    <label class="form-label col-xl-1 col-lg-2 col-md-3 lead" for="password">
                                        Password:
                                    </label>
                                    <div class="col-xl-5 col-lg-8 col-md">
                                        <input class="form-control" type="password" name="password" required>
                                    </div>
                                </div>
                                <input class="btn bg-dark-secondary text-white" type="submit" value="Login">
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