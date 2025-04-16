<?php

// Start $_SESSION
session_start();

// Require config file
require_once('../configs/connections-config.php');

$username = $_SESSION['user_platform_id'];
$authToken = TMDBAPIKEY;

// TMDB API URLs
$base = "https://api.themoviedb.org/3/movie";
$popularUrl = "$base/popular?language=en-US&page=1";
$topRatedUrl = "$base/top_rated?language=en-US&page=1";
$upcomingUrl = "$base/upcoming?language=en-US&page=1";

// Initialize variables
$popular = [];
$topRated = [];
$upcoming = [];
$error = '';

// Fetch data
function fetchData($url, &$data, &$error, $name, $authToken) {
    $options = [
        "http" => [
            "header" => "Authorization: Bearer $authToken\r\nAccept: application/json\r\n",
            "method" => "GET"
        ]
    ];
    $context = stream_context_create($options);
    $json = @file_get_contents($url, false, $context);

    if ($json === FALSE) {
        $lastError = error_get_last();
        $error .= "Failed to fetch $name. Error: " . $lastError['message'] . "<br>";
    } else {
        $data = json_decode($json, true);
    }
}

// Fetch all movie categories
fetchData($popularUrl, $popular, $error, 'popular', $authToken);
fetchData($topRatedUrl, $topRated, $error, 'top rated', $authToken);
fetchData($upcomingUrl, $upcoming, $error, 'upcoming', $authToken);

?>

<!-- HTML Page Layout -->
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>All Movies</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../../styles.css">
</head>
<body class="bg-dark text-light">

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
                <a class="btn btn-dark w-100 mt-2" href="../music/lastfm-info.php" role="button">
                    Last.fm
                </a>
            </div>

            <div class="mt-2">
                <a class="btn btn-dark w-100 mt-2" href="tmdb-info.php" role="button">
                    TMDB
                </a>
            </div>

        </nav>

        <main class="col-md-10 ms-sm-auto px-4">
            <div class="d-flex justify-content-between align-items-center pt-3 pb-2 mb-3 border-bottom">
                <h2>TMDB</h2>
            </div>

            <div class="card bg-dark text-light mb-4">
                <div class="card-body">
                    <input type="text" class="form-control" placeholder="Search Titles...">
                </div>
            </div>

            <!-- Popular Movies -->
            <h3>Popular Movies</h3>
            <?php if (!empty($popular['results'])): ?>
                <div class="table-responsive">
                    <table class="table table-dark table-hover">
                        <thead>
                            <tr>
                                <th>Title</th>
                                <th>Rating</th>
                                <th>Overview</th>
                            </tr>
                        </thead>
                        <tbody>
                            <?php foreach ($popular['results'] as $movie): ?>
                                <tr>
                                    <td><?php echo htmlspecialchars($movie['title']); ?></td>
                                    <td><?php echo $movie['vote_average']; ?></td>
                                    <td><?php echo htmlspecialchars($movie['overview']); ?></td>
                                </tr>
                            <?php endforeach; ?>
                        </tbody>
                    </table>
                </div>
            <?php else: ?>
                <p>No popular movies found.</p>
            <?php endif; ?>

            <!-- Top Rated Movies -->
            <h3 class="mt-5">Top Rated Movies</h3>
            <?php if (!empty($topRated['results'])): ?>
                <div class="table-responsive">
                    <table class="table table-dark table-hover">
                        <thead>
                            <tr>
                                <th>Title</th>
                                <th>Rating</th>
                                <th>Overview</th>
                            </tr>
                        </thead>
                        <tbody>
                            <?php foreach ($topRated['results'] as $movie): ?>
                                <tr>
                                    <td><?php echo htmlspecialchars($movie['title']); ?></td>
                                    <td><?php echo $movie['vote_average']; ?></td>
                                    <td><?php echo htmlspecialchars($movie['overview']); ?></td>
                                </tr>
                            <?php endforeach; ?>
                        </tbody>
                    </table>
                </div>
            <?php else: ?>
                <p>No top rated movies found.</p>
            <?php endif; ?>

            <!-- Upcoming Movies -->
            <h3 class="mt-5">Upcoming Movies</h3>
            <?php if (!empty($upcoming['results'])): ?>
                <div class="table-responsive">
                    <table class="table table-dark table-hover">
                        <thead>
                            <tr>
                                <th>Title</th>
                                <th>Release Date</th>
                                <th>Overview</th>
                            </tr>
                        </thead>
                        <tbody>
                            <?php foreach ($upcoming['results'] as $movie): ?>
                                <tr>
                                    <td><?php echo htmlspecialchars($movie['title']); ?></td>
                                    <td><?php echo $movie['release_date']; ?></td>
                                    <td><?php echo htmlspecialchars($movie['overview']); ?></td>
                                </tr>
                            <?php endforeach; ?>
                        </tbody>
                    </table>
                </div>
            <?php else: ?>
                <p>No upcoming movies found.</p>
            <?php endif; ?>

            <?php if ($error): ?>
                <div class="alert alert-danger mt-4"><?php echo htmlspecialchars($error); ?></div>
            <?php endif; ?>
        </main>
    </div>
</div>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

</body>
</html>
