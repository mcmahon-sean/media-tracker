<?php

    // Start session
    session_start();

    // Required
    require_once '../media/LastFm/get_loved_tracks.php';
    require_once '../media/LastFm/get_recent_tracks.php';
    require_once '../media/LastFm/get_top_albums.php';
    require_once '../media/LastFm/get_top_artists.php';
    require_once '../media/LastFm/get_top_tracks.php';

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
                    <h2>Media Tracker</h2>
                </div>

                <div class="card bg-dark text-light mb-4">
                    <div class="card-body">
                        <input type="text" class="form-control" placeholder="Search Titles...">
                    </div>
                </div>

                <!-- Loved Tracks Section -->
                <div class="table-responsive">
                    <h3>Loved Tracks</h3>
                    <?php if (isset($error)): ?>
                        <p><?php echo $error; ?></p>
                    <?php else: ?>
                        <table class="table table-dark table-hover">
                            <thead>
                                <tr>
                                    <th>Track Name</th>
                                    <th>Artist Name</th>
                                    <th>Last Played</th>
                                    <th>URL</th>
                                </tr>
                            </thead>
                            <tbody>
                                <?php foreach ($lovedTracks as $track): ?>
                                    <tr>
                                        <td><?php echo htmlspecialchars($track->name); ?></td>
                                        <td><?php echo htmlspecialchars($track->artist); ?></td>
                                        <td><?php echo htmlspecialchars($track->getFormattedDate()); ?></td>
                                        <td><a href="<?php echo htmlspecialchars($track->url); ?>" target="_blank">View</a></td>
                                    </tr>
                                <?php endforeach; ?>
                            </tbody>
                        </table>
                    <?php endif; ?>
                </div>

                <!-- Recent Tracks Section -->
                <div class="table-responsive">
                    <h3>Recent Tracks</h3>
                    <?php if (isset($error)): ?>
                        <p><?php echo $error; ?></p>
                    <?php else: ?>
                        <table class="table table-dark table-hover">
                            <thead>
                                <tr>
                                    <th>Track Name</th>
                                    <th>Artist Name</th>
                                    <th>Last Played</th>
                                    <th>URL</th>
                                </tr>
                            </thead>
                            <tbody>
                                <?php foreach ($recentTracks as $track): ?>
                                    <tr>
                                        <td><?php echo htmlspecialchars($track->name); ?></td>
                                        <td><?php echo htmlspecialchars($track->artist); ?></td>
                                        <td><?php echo htmlspecialchars($track->getFormattedDate()); ?></td>
                                        <td><a href="<?php echo htmlspecialchars($track->url); ?>" target="_blank">View</a></td>
                                    </tr>
                                <?php endforeach; ?>
                            </tbody>
                        </table>
                    <?php endif; ?>
                </div>

                <!-- Top Albums Section -->
                <div class="table-responsive">
                    <h3>Top Albums</h3>
                    <?php if (isset($error)): ?>
                        <p><?php echo $error; ?></p>
                    <?php else: ?>
                        <table class="table table-dark table-hover">
                            <thead>
                                <tr>
                                    <th>Playcount</th>
                                    <th>Album Name</th>
                                    <th>Artist Name</th>
                                    <th>URL</th>
                                </tr>
                            </thead>
                            <tbody>
                                <?php foreach ($topAlbums as $album): ?>
                                    <tr>
                                        <td><?php echo htmlspecialchars($album->playCount); ?></td>
                                        <td><?php echo htmlspecialchars($album->name); ?></td>
                                        <td><?php echo htmlspecialchars($album->artist); ?></td>
                                        <td><a href="<?php echo htmlspecialchars($album->url); ?>" target="_blank">View</a></td>
                                    </tr>
                                <?php endforeach; ?>
                            </tbody>
                        </table>
                    <?php endif; ?>
                </div>

                <!-- Top Artists Section -->
                <div class="table-responsive">
                    <h3>Top Artists</h3>
                    <?php if (isset($error)): ?>
                        <p><?php echo $error; ?></p>
                    <?php else: ?>
                        <table class="table table-dark table-hover">
                            <thead>
                                <tr>
                                    <th>Playcount</th>
                                    <th>Artist Name</th>
                                    <th>URL</th>
                                </tr>
                            </thead>
                            <tbody>
                                <?php foreach ($topArtists as $artist): ?>
                                    <tr>
                                        <td><?php echo htmlspecialchars($artist->playCount); ?></td>
                                        <td><?php echo htmlspecialchars($artist->name); ?></td>
                                        <td>
                                            <a href="<?php echo htmlspecialchars($artist->url); ?>" target="_blank">View</a>
                                        </td>
                                    </tr>
                                <?php endforeach; ?>
                            </tbody>
                        </table>
                    <?php endif; ?>
                </div>

                <!-- Top Tracks Section -->
                <div class="table-responsive">
                    <h3>Top Tracks</h3>
                    <?php if (isset($error)): ?>
                        <p><?php echo $error; ?></p>
                    <?php else: ?>
                        <table class="table table-dark table-hover">
                            <thead>
                                <tr>
                                    <th>Playcount</th>
                                    <th>Track Name</th>
                                    <th>Artist Name</th>
                                    <th>URL</th>
                                </tr>
                            </thead>
                            <tbody>
                                <?php foreach ($topTracks as $track): ?>
                                    <tr>
                                        <td><?php echo htmlspecialchars($track->playCount); ?></td>
                                        <td><?php echo htmlspecialchars($track->name); ?></td>
                                        <td><?php echo htmlspecialchars($track->artist); ?></td>
                                        <td><a href="<?php echo htmlspecialchars($track->url); ?>" target="_blank">View</a></td>
                                    </tr>
                                <?php endforeach; ?>
                            </tbody>
                        </table>
                    <?php endif; ?>
                </div>

                <?php if ($error): ?>
                    <div class="alert alert-danger">
                        <?php echo htmlspecialchars($error); ?>
                    </div>
                <?php endif; ?>
            </main>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script src="../../../supabaseClient.js" type="module"></script>
    <script src="../../../script.js" type="module"></script>

</body>
</html>