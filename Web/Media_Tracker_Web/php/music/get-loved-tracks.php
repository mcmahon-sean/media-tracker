<?php

// Require config file
require_once('../configs/lastfm-config.php');

// Username and API from config file
$username = LASTFM_USERNAME;
$apiKey = LASTFM_API_KEY;

// URL for the API request
$lovedTracksUrl = "https://ws.audioscrobbler.com/2.0/?method=user.getlovedtracks&user=$username&api_key=$apiKey&format=json&limit=5";

// Fetch the data from the API
$response = file_get_contents($lovedTracksUrl);

// Decode the JSON response
$lovedTracks = json_decode($response, true);

// Check if the response is valid
if ($lovedTracks && isset($lovedTracks['lovedtracks']['track'])) {
    $tracks = $lovedTracks['lovedtracks']['track'];
} else {
    $tracks = [];
    $error = "Error fetching data. Please check the username or API key.";
}
?>

<!-- Page Layout -->
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Loved Tracks</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../../styles.css"> 
</head>
<body>

    <div class="container-fluid">
        <div class="row">
            <nav class="col-md-2 d-none d-md-block sidebar">
                <div class="dropdown">
                    <a class="btn btn-dark dropdown-toggle w-100" href="#" role="button" data-bs-toggle="dropdown">
                        Music
                    </a>
                    <ul class="dropdown-menu">
                        <li><a class="dropdown-item" href="get-all.php">All Music</a></li>
                        <li><a class="dropdown-item" href="get-loved-tracks.php">Loved Tracks</a></li>
                        <li><a class="dropdown-item" href="get-recent-tracks.php">Recent Tracks</a></li>
                        <li><a class="dropdown-item" href="get-top-albums.php">Top Albums</a></li>
                        <li><a class="dropdown-item" href="get-top-artists.php">Top Artists</a></li>
                        <li><a class="dropdown-item" href="get-top-tracks.php">Top Tracks</a></li>
                    </ul>
                </div>
                
                <div class="dropdown mt-2">
                    <a class="btn btn-dark dropdown-toggle w-100" href="#" role="button" data-bs-toggle="dropdown">
                         Movies
                    </a>
                    <ul class="dropdown-menu">
                        <li><a class="dropdown-item" href="#">All Movies</a></li>
                        <li><a class="dropdown-item" href="#">Last Played</a></li>
                    </ul>
                </div>

                <div class="dropdown mt-2">
                    <a class="btn btn-dark dropdown-toggle w-100" href="#" role="button" data-bs-toggle="dropdown">
                         Games
                    </a>
                    <ul class="dropdown-menu">
                        <li><a class="dropdown-item" href="#">All Games</a></li>
                        <li><a class="dropdown-item" href="#">Last Played</a></li>
                    </ul>
                </div>

                <div class="mt-2">
                    <a class="btn btn-dark w-100" href="../../index.html" role="button">
                        Home
                    </a>
                </div>

            </nav>

            <main class="col-md-10 ms-sm-auto px-4">
                <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
                    <h2>Loved Tracks</h2>
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
                                <?php foreach ($tracks as $track): ?>
                                    <tr>
                                        <td><?php echo htmlspecialchars($track['name']); ?></td>
                                        <td><?php echo htmlspecialchars($track['artist']['name']); ?></td>
                                        <td><?php echo htmlspecialchars($track['date']['#text']); ?></td>
                                        <td><a href="<?php echo htmlspecialchars($track['url']); ?>" target="_blank">View</a></td>
                                    </tr>
                                <?php endforeach; ?>
                            </tbody>
                        </table>
                    <?php endif; ?>
                </div>
            </main>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    
</body>
</html>
