<?php

// Start session
session_start();

// Required
require_once '../media/TMDB/get_rated_tv_shows.php';
require_once '../filter_functions.php';

$has_filter = isset($_GET["searchString"]) && $_GET["searchString"] != "";
// Grab the input string and selected category for searching from the post array
$filter_string = $_GET["searchString"] ?? "";

$filtUrl = $has_filter ? "searchString=$filter_string" : "";

// Get sort options from URL
if (isset($_GET["sort"])) {
    $split = explode('_', $_GET["sort"]);
    $sort_field = $split[0];
    $sort_dir = $split[1];
} else {
    $sort_field = "name";
    $sort_dir = "asc";
}

$rated_shows_filt = sortBy(filter($ratedTvShows, "name", $filter_string, $movie = true), $sort_field, $sort_dir);

?>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Rated Shows</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../../../styles.css">
</head>

<body class="bg-dark-primary">
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
                        role="button">
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
                            Rated Shows
                        </small>
                    </h2>
                </div>

                <!-- Search Bar -->
                <div class="card bg-dark text-light mb-4">
                    <div class="card-body <?php echo ($has_filter) ? 'pb-2' : '' ?>">
                        <form method="get" class="row pe-3 ps-0 mb-0">
                            <?php if (isset($_GET["sort"])): ?>
                                <input type="hidden" name="sort" value="<?php echo $_GET["sort"] ?>" />
                            <?php endif; ?>
                            <div class="col-9 col-md-10">
                                <div class="input-group">
                                    <input name="searchString" type="text" class="form-control" placeholder="Search..."
                                        <?php echo ($filter_string != "") ? 'value="' . $filter_string . '"' : "" ?> />
                                    <span class="input-group-text search-select pe-3">Titles</span>
                                </div>
                            </div>
                            <button type="submit" class="btn bg-dark-secondary text-white col-3 col-md-2">Search</button>
                        </form>
                        <?php if ($has_filter): ?>
                            <form method="get" class="row mt-2 ms-1 filter-label">
                                <?php if (isset($_GET["sort"])): ?>
                                    <input type="hidden" name="sort" value="<?php echo $_GET["sort"] ?>" />
                                <?php endif; ?>
                                <div>
                                    <p>Searching for "<?php echo $filter_string ?>"</p>
                                </div>
                                <button type="submit" class="btn bg-dark-secondary text-white">Clear Filter</button>
                            </form>
                        <?php endif; ?>
                    </div>
                </div>

                <?php if (isset($_SESSION['user_platform_ids']['tmdb'])): ?>
                    <h3>Rated TV Shows</h3>
                    <div class="table-responsive">
                        <?php if (isset($error)): ?>
                            <p><?php echo $error; ?></p>
                        <?php else: ?>
                            <table class="table table-dark table-hover">
                                <thead>
                                    <tr>
                                        <th>Favorite</th>
                                        <th><a class="sort-link" <?php echo sortLink($sort_field, $sort_dir, "id", $filtUrl) ?>>
                                                ID
                                            </a></th>
                                        <th><a class="sort-link" <?php echo sortLink($sort_field, $sort_dir, "name", $filtUrl) ?>>
                                                Name
                                            </a></th>
                                        <th><a class="sort-link" <?php echo sortLink($sort_field, $sort_dir, "userRating", $filtUrl) ?>>
                                                Your Rating
                                            </a></th>
                                        <th><a class="sort-link" <?php echo sortLink($sort_field, $sort_dir, "avgRating", $filtUrl) ?>>
                                                Average Rating
                                            </a></th>
                                        <th><a class="sort-link" <?php echo sortLink($sort_field, $sort_dir, "votes", $filtUrl) ?>>
                                                Vote Count
                                            </a></th>
                                        <th>Overview</th>

                                    </tr>
                                </thead>
                                <tbody>
                                    <?php if (count($rated_shows_filt) > 0): ?>
                                        <?php foreach ($rated_shows_filt as $show): ?>
                                            <tr>
                                                <td>
                                                    <span
                                                        class="favorite-icon ms-3"
                                                        role="button"
                                                        data-platform_id="3"
                                                        data-media_type_id="2"
                                                        data-media_plat_id="<?php echo $show->id; ?>"
                                                        data-title="<?php echo htmlspecialchars($show->name); ?>"
                                                        data-album=""
                                                        data-artist=""
                                                        data-username="<?php echo $_SESSION['username']; ?>">
                                                        ☆
                                                    </span>
                                                </td>
                                                <td><?php echo htmlspecialchars($show->id); ?></td>
                                                <td><?php echo htmlspecialchars($show->name); ?></td>
                                                <td><?php echo htmlspecialchars($show->user_rating); ?></td>
                                                <td><?php echo htmlspecialchars($show->average_rating); ?></td>
                                                <td><?php echo htmlspecialchars($show->vote_count); ?></td>
                                                <td><?php echo htmlspecialchars($show->overview); ?></td>
                                            </tr>
                                        <?php endforeach; ?>
                                    <?php else: ?>
                                        <tr>
                                            <td colspan="7" class="lead text-center">No items match the filter</td>
                                        </tr>
                                    <?php endif; ?>
                                </tbody>
                            </table>
                        <?php endif; ?>
                    </div>
                <?php else: ?>
                    <h1>Please add your TMDB account</h1>
                <?php endif; ?>
            </main>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script src="../../../supabaseClient.js" type="module"></script>
    <script src="../../../script.js" type="module"></script>

</body>

</html>