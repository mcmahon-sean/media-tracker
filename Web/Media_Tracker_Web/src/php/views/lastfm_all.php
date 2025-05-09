<?php

    // Start session
    session_start();

    // Required
    require_once '../media/LastFm/get_all.php';

    require_once '../filter_functions.php';

    $has_filter = isset($_POST["searchString"]) && $_POST["searchString"] != "";
    // Grab the input string and selected category for searching from the post array
    $filter_string = $_POST["searchString"] ?? "";
    $filter_category = $_POST["searchCategory"] ?? "";

    // Filter the various LastFm categories using filter()
    $loved_tracks_filt = filter($lovedTracks, $filter_category, $filter_string);
    $recent_tracks_filt = filter($recentTracks, $filter_category, $filter_string);
    $top_albums_filt = filter($topAlbums, $filter_category, $filter_string);
    $top_tracks_filt = filter($topTracks, $filter_category, $filter_string);
    /*  
     *  Top Artists doesn't have a title column, and its artist column uses the same $name property as the titles,
     *  so pass "name" as the category and return "hide" if the user wasn't filtering by artist
     */ 
    $top_artists_filt = ($filter_category == "name" && $filter_string != "") ? "hide" : filter($topArtists, "name", $filter_string);

?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>All Music</title>
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
                            All Music
                        </small>
                    </h2>
                </div>

                <!-- Search Bar -->
                <div class="card bg-dark text-light mb-4">
                    <div class="card-body <?php echo ($has_filter) ? 'pb-2' : '' ?>">
                        <form method="post" class="row pe-3 ps-0">
                            <div class="col-9 col-md-10">
                                <div class="input-group">
                                    <input name="searchString" type="text" class="form-control" placeholder="Search..." 
                                        <?php echo ($filter_string != "") ? 'value="'.$filter_string.'"' : "" ?>
                                    />
                                    <select name="searchCategory" class="form-select search-select">
                                        <option value="name" <?php if ($filter_category == "name") {echo "selected";} ?>>
                                            Titles
                                        </option>
                                        <option value="artist" <?php if ($filter_category == "artist") {echo "selected";} ?>>
                                            Artists
                                        </option>
                                    </select>
                                </div>
                            </div>
                            <button type="submit" class="btn bg-dark-secondary text-white col-3 col-md-2">Search</button>
                        </form>
                        <?php if($has_filter): ?>
                            <form method="post" class="row mt-2 ms-1 filter-label">
                                <input type="hidden" name="searchString" />
                                <input type="hidden" name="searchCategory" value="name" />
                                <div>
                                    <p>Searching for "<?php echo $filter_string ?>" in <?php echo ($filter_category == "artist") ? "Artists" : "Titles" ?></p>
                                </div>
                                <button type="submit" class="btn bg-dark-secondary text-white">Clear Filter</button>
                            </form>
                        <?php endif; ?>
                    </div>
                </div>

                <!-- Loved Tracks Section -->
                <div class="table-responsive">
                    <h3>Loved Tracks</h3>
                    <?php if (isset($error)): ?>
                        <p><?php echo $error; ?></p>
                    <?php else: ?>
                        <table class="table table-dark table-hover" id="loved-tracks">
                            <thead>
                                <tr>
                                    <th>Track Name</th>
                                    <th>Artist Name</th>
                                    <th>Last Played</th>
                                    <th>URL</th>
                                </tr>
                            </thead>
                            <tbody>
                                <?php if (count($loved_tracks_filt) > 0): ?>
                                    <?php foreach ($loved_tracks_filt as $track): ?>
                                        <tr>
                                            <td><?php echo htmlspecialchars($track->name); ?></td>
                                            <td><?php echo htmlspecialchars($track->artist); ?></td>
                                            <td><?php echo htmlspecialchars($track->getFormattedDate()); ?></td>
                                            <td><a href="<?php echo htmlspecialchars($track->url); ?>" target="_blank">View</a></td>
                                        </tr>
                                    <?php endforeach; ?>
                                <?php else: ?>
                                    <tr>
                                        <td colspan="4" class="lead text-center">No items match the filter</td>
                                    </tr>
                                <?php endif; ?>
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
                        <table class="table table-dark table-hover" id="recent-tracks">
                            <thead>
                                <tr>
                                    <th>Track Name</th>
                                    <th>Artist Name</th>
                                    <th>Last Played</th>
                                    <th>URL</th>
                                </tr>
                            </thead>
                            <tbody>
                                <?php if (count($recent_tracks_filt) > 0): ?>
                                    <?php foreach ($recent_tracks_filt as $track): ?>
                                        <tr>
                                            <td><?php echo htmlspecialchars($track->name); ?></td>
                                            <td><?php echo htmlspecialchars($track->artist); ?></td>
                                            <td><?php echo htmlspecialchars($track->getFormattedDate()); ?></td>
                                            <td><a href="<?php echo htmlspecialchars($track->url); ?>" target="_blank">View</a></td>
                                        </tr>
                                    <?php endforeach; ?>
                                <?php else: ?>
                                    <tr>
                                        <td colspan="4" class="lead text-center">No items match the filter</td>
                                    </tr>
                                <?php endif; ?>
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
                        <table class="table table-dark table-hover" id="top-albums">
                            <thead>
                                <tr>
                                    <th>Playcount</th>
                                    <th>Album Name</th>
                                    <th>Artist Name</th>
                                    <th>URL</th>
                                </tr>
                            </thead>
                            <tbody>
                                <?php if (count($top_albums_filt) > 0): ?>
                                    <?php foreach ($top_albums_filt as $album): ?>
                                        <tr>
                                            <td><?php echo htmlspecialchars($album->playCount); ?></td>
                                            <td><?php echo htmlspecialchars($album->name); ?></td>
                                            <td><?php echo htmlspecialchars($album->artist); ?></td>
                                            <td><a href="<?php echo htmlspecialchars($album->url); ?>" target="_blank">View</a></td>
                                        </tr>
                                    <?php endforeach; ?>
                                <?php else: ?>
                                    <tr>
                                        <td colspan="4" class="lead text-center">No items match the filter</td>
                                    </tr>
                                <?php endif; ?>
                            </tbody>
                        </table>
                    <?php endif; ?>
                </div>

                <!-- Top Artists Section -->
                <?php if ($top_artists_filt != "hide"): ?>
                    <div class="table-responsive">
                        <h3>Top Artists</h3>
                        <?php if (isset($error)): ?>
                            <p><?php echo $error; ?></p>
                        <?php else: ?>
                            <table class="table table-dark table-hover" id="top-artists">
                                <thead>
                                    <tr>
                                        <th>Playcount</th>
                                        <th>Artist Name</th>
                                        <th>URL</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <?php if (count($top_artists_filt) > 0): ?>
                                        <?php foreach ($top_artists_filt as $artist): ?>
                                            <tr>
                                                <td><?php echo htmlspecialchars($artist->playCount); ?></td>
                                                <td><?php echo htmlspecialchars($artist->name); ?></td>
                                                <td>
                                                    <a href="<?php echo htmlspecialchars($artist->url); ?>" target="_blank">View</a>
                                                </td>
                                            </tr>
                                        <?php endforeach; ?>
                                    <?php else: ?>
                                        <tr>
                                            <td colspan="3" class="lead text-center">No items match the filter</td>
                                        </tr>
                                    <?php endif; ?>
                                </tbody>
                            </table>
                        <?php endif; ?>
                    </div>
                <?php endif; ?>

                <!-- Top Tracks Section -->
                <div class="table-responsive">
                    <h3>Top Tracks</h3>
                    <?php if (isset($error)): ?>
                        <p><?php echo $error; ?></p>
                    <?php else: ?>
                        <table class="table table-dark table-hover" id="top-tracks">
                            <thead>
                                <tr>
                                    <th>Playcount</th>
                                    <th>Track Name</th>
                                    <th>Artist Name</th>
                                    <th>URL</th>
                                </tr>
                            </thead>
                            <tbody>
                                <?php if (count($top_tracks_filt) > 0): ?>
                                    <?php foreach ($top_tracks_filt as $track): ?>
                                        <tr>
                                            <td><?php echo htmlspecialchars($track->playCount); ?></td>
                                            <td><?php echo htmlspecialchars($track->name); ?></td>
                                            <td><?php echo htmlspecialchars($track->artist); ?></td>
                                            <td><a href="<?php echo htmlspecialchars($track->url); ?>" target="_blank">View</a></td>
                                        </tr>
                                    <?php endforeach; ?>
                                <?php else: ?>
                                    <tr>
                                        <td colspan="4" class="lead text-center">No items match the filter</td>
                                    </tr>
                                <?php endif; ?>
                            </tbody>
                        </table>
                    <?php endif; ?>
                </div>

                <!-- Commented out for consistency -->
               <!-- <?php if ($error): ?>
                    <div class="alert alert-danger">
                        <?php echo htmlspecialchars($error); ?>
                    </div>
                <?php endif; ?> -->
               

            </main>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script src="../../../supabaseClient.js" type="module"></script>
    <script src="../../../script.js" type="module"></script>

</body>
</html>