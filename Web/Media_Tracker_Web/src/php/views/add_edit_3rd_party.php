<?php

    // Start session
    session_start();

?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Add/Edit 3rd Party Accounts</title>
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
                            Add/Edit Platforms
                        </small>
                    </h2>
                </div>

                <div class="table-responsive">
                <?php if (isset($error)): ?>
                    <p><?php echo $error; ?></p>
                <?php else: ?>
                    <div class="form-container">
                        <form id="addEditForm" action="../database/add_3rd_party_id.php" method="post">
                            <input type="hidden" name="username" value="<?php echo htmlspecialchars($_SESSION['username']); ?>">
                            <div class="row mb-3 mx-1">
                                <label class="form-label col-xxl-2 col-lg-3 col-md-5 lead" for="platform_id">
                                    Choose a platform:
                                </label>
                                <div class="col-xl-2 col-lg-3 col-md-4">
                                    <select class="form-select" name="platform_id" id="platform_id" onchange="updateFormAction()">
                                        <option value="1">Steam</option>
                                        <option value="2">Last.fm</option>
                                        <option value="3">TMDB</option>
                                    </select>
                                </div>
                            </div>
                            <div class="row mb-3 mx-1">
                                <label id="usrLabel" class="form-label col-xxl-2 col-lg-3 col-md-5 lead" for="user_plat_id">
                                    Enter Username:
                                </label>
                                <div class="col-xl-4 col-lg-6 col-md">
                                    <input class="form-control" type="text" name="user_plat_id" id="user_plat_id" required>
                                </div>
                            </div>
                            <input class="btn bg-dark-secondary text-white" type="submit" value="Add/Edit">
                        </form>
                    </div>

                    <script>
                        function updateFormAction() {
                            const form = document.getElementById("addEditForm");
                            const platform = document.getElementById("platform_id").value;
                            const user_plat_id = document.getElementById("user_plat_id").value;
                            const username = document.querySelector("input[name='username']").value;
                            const userLabel = document.getElementById("usrLabel");

                            if (platform == "1") {
                                userLabel.innerHTML = "Enter Steam ID:";
                            }
                            else {
                                userLabel.innerHTML = "Enter Username:";
                            }

                            switch (platform) {
                                case "3":
                                    form.action = `../authentication/auth_tmdb.php?username=${encodeURIComponent(username)}&user_plat_id=${encodeURIComponent(user_plat_id)}&platform_id=3`;
                                    form.method = "get";
                                    break;
                                default:
                                    form.action = "../database/add_3rd_party_id.php";
                                    break;
                            }
                        }
                        window.onload = updateFormAction;
                    </script>
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