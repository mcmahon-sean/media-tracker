<?php

// Start $_SESSION
session_start();

// Require config file
require_once('../configs/connections-config.php');

$username = $_SESSION['user_platform_id'];
$apiKey = LASTFMAPIKEY;

// Last.fm API URLs
$lovedTracksUrl = "https://ws.audioscrobbler.com/2.0/?method=user.getlovedtracks&user=$username&api_key=$apiKey&format=json&limit=5";
$recentTracksUrl = "https://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user=$username&api_key=$apiKey&format=json&limit=5";
$topAlbumsUrl = "https://ws.audioscrobbler.com/2.0/?method=user.gettopalbums&user=$username&api_key=$apiKey&format=json&limit=5";
$topArtistsUrl = "https://ws.audioscrobbler.com/2.0/?method=user.gettopartists&user=$username&api_key=$apiKey&format=json&limit=5";
$topTracksUrl = "https://ws.audioscrobbler.com/2.0/?method=user.gettoptracks&user=$username&api_key=$apiKey&format=json&limit=5";

// Initialize variables
$recentTracks = [];
$lovedTracks = [];
$topAlbums = [];
$topArtists = [];
$topTracks = [];
$error = '';

// Fetch data function
function fetchData($url, &$data, &$error, $dataName) {
    $json = @file_get_contents($url);
    if ($json === FALSE) {
        $error = "Failed to fetch $dataName. Please check your username or API key.";
    } else {
        $data = json_decode($json, true);
    }
}

// Fetch all data using fetchData()
fetchData($recentTracksUrl, $recentTracks, $error, 'recent tracks');
fetchData($lovedTracksUrl, $lovedTracks, $error, 'loved tracks');
fetchData($topAlbumsUrl, $topAlbums, $error, 'top albums');
fetchData($topArtistsUrl, $topArtists, $error, 'top artists');
fetchData($topTracksUrl, $topTracks, $error, 'top tracks');

?>

<!-- Page Layout -->
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>All Music</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../../styles.css">
</head>
<body>

    <div class="container-fluid">
        <div class="row">
            <nav class="col-md-2 d-none d-md-block sidebar">
               
                <div class="mt-2">
                    <a class="btn btn-dark w-100 mt-2" href="../../index.php" role="button">
                        Home
                    </a>
                </div>

                <div class="mt-2">
                    <a class="btn btn-dark w-100 mt-2" href="../games/steam-info.php" role="button">
                        Steam
                    </a>
                </div>

                <div class="mt-2">
                    <a class="btn btn-dark w-100 mt-2" href="lastfm-info.php" role="button">
                        Last.fm
                    </a>
                </div>

                <div class="mt-2">
                    <a class="btn btn-dark w-100 mt-2" href="../movies/tmdb-info.php" role="button">
                        TMDB
                    </a>
                </div>
               
            </nav>

            <main class="col-md-10 ms-sm-auto px-4">
                <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
                    <h2>Last.fm</h2>
                </div>

                <div class="card bg-dark text-light mb-4">
                    <div class="card-body">
                        <input type="text" class="form-control" placeholder="Search Titles...">
                    </div>
                </div>

                <!-- Loved Tracks Section -->
                <div class="table-responsive">
                    <h3>Loved Tracks</h3>
                    <?php if (isset($lovedTracks['lovedtracks']['track']) && count($lovedTracks['lovedtracks']['track']) > 0): ?>
                        <table class="table table-dark table-hover">
                            <thead>
                                <tr>
                                    <th>Track Name</th>
                                    <th>Artist Name</th>
                                    <th>URL</th>
                                </tr>
                            </thead>
                            <tbody>
                                <?php foreach ($lovedTracks['lovedtracks']['track'] as $track): ?>
                                    <tr>
                                        <td><?php echo htmlspecialchars($track['name']); ?></td>
                                        <td><?php echo htmlspecialchars($track['artist']['name']); ?></td>
                                        <td><a href="<?php echo htmlspecialchars($track['url']); ?>" target="_blank">View</a></td>
                                    </tr>
                                <?php endforeach; ?>
                            </tbody>
                        </table>
                    <?php else: ?>
                        <p>No loved tracks found.</p>
                    <?php endif; ?>
                </div>

                <!-- Recent Tracks Section -->
                <div class="table-responsive" id="recent_tracks_output">
                    <h3>Recent Tracks</h3>
                    <?php if (isset($recentTracks['recenttracks']['track']) && count($recentTracks['recenttracks']['track']) > 0): ?>
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
                                <?php foreach ($recentTracks['recenttracks']['track'] as $track): ?>
                                    <tr>
                                        <td><?php echo htmlspecialchars($track['name']); ?></td>
                                        <td><?php echo htmlspecialchars($track['artist']['#text']); ?></td>
                                        <td><?php echo htmlspecialchars($track['date']['#text']); ?></td>
                                        <td><a href="<?php echo htmlspecialchars($track['url']); ?>" target="_blank">View</a></td>
                                    </tr>
                                <?php endforeach; ?>
                            </tbody>
                        </table>
                    <?php else: ?>
                        <p>No recent tracks found.</p>
                    <?php endif; ?>
                </div>

                <!-- Top Albums Section -->
                <div class="table-responsive" id="recent_tracks_output">
                    <h3>Top Albums</h3>
                    <?php if (isset($topAlbums['topalbums']['album']) && count($topAlbums['topalbums']['album']) > 0): ?>
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
                                <?php foreach ($topAlbums['topalbums']['album'] as $album): ?>
                                    <tr>
                                        <td><?php echo htmlspecialchars($album['playcount']); ?></td>
                                        <td><?php echo htmlspecialchars($album['name']); ?></td>
                                        <td><?php echo htmlspecialchars($album['artist']['name']); ?></td>
                                        <td><a href="<?php echo htmlspecialchars($album['url']); ?>" target="_blank">View</a></td>
                                    </tr>
                                <?php endforeach; ?>
                            </tbody>
                        </table>
                    <?php else: ?>
                        <p>No top albums found.</p>
                    <?php endif; ?>
                </div>

                <!-- Top Artists Section -->
                <div class="table-responsive" id="recent_tracks_output">
                    <h3>Top Artists</h3>
                    <?php if (isset($topArtists['topartists']['artist']) && count($topArtists['topartists']['artist']) > 0): ?>
                        <table class="table table-dark table-hover">
                            <thead>
                                <tr>
                                    <th>Playcount</th>
                                    <th>Artist Name</th>
                                    <th>URL</th>
                                </tr>
                            </thead>
                            <tbody>
                                <?php foreach ($topArtists['topartists']['artist'] as $artist): ?>
                                    <tr>
                                        <td><?php echo htmlspecialchars($artist['playcount']); ?></td>
                                        <td><?php echo htmlspecialchars($artist['name']); ?></td>
                                        <td><a href="<?php echo htmlspecialchars($artist['url']); ?>" target="_blank">View</a></td>
                                    </tr>
                                <?php endforeach; ?>
                            </tbody>
                        </table>
                    <?php else: ?>
                        <p>No top artists found.</p>
                    <?php endif; ?>
                </div>

                <!-- Top Tracks Section -->
                <div class="table-responsive" id="recent_tracks_output">
                    <h3>Top Tracks</h3>
                    <?php if (isset($topTracks['toptracks']['track']) && count($topTracks['toptracks']['track']) > 0): ?>
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
                                <?php foreach ($topTracks['toptracks']['track'] as $track): ?>
                                    <tr>
                                        <td><?php echo htmlspecialchars($track['playcount']); ?></td>
                                        <td><?php echo htmlspecialchars($track['name']); ?></td>
                                        <td><?php echo htmlspecialchars($track['artist']['name']); ?></td>
                                        <td><a href="<?php echo htmlspecialchars($track['url']); ?>" target="_blank">View</a></td>
                                    </tr>
                                <?php endforeach; ?>
                            </tbody>
                        </table>
                    <?php else: ?>
                        <p>No top tracks found.</p>
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

</body>
</html>