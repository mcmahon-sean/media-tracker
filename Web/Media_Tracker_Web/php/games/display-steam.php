<?php

// Start $_SESSION
session_start();

// Required
require_once('../configs/connections-config.php');

$username = $_SESSION['user_platform_id'];
$apiKey = STEAMAPIKEY;

// Steam API URLs
$userSummaryUrl = "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key=$apiKey&steamids=$username";
$recentGamesUrl = "https://api.steampowered.com/IPlayerService/GetRecentlyPlayedGames/v1/?key=$apiKey&steamid=$username&format=json";

// Initialize variables
$profile = [];
$recentGames = [];
$error = '';

// Fetch data
function fetchData($url, &$data, &$error, $dataName) {
    $json = @file_get_contents($url);
    if ($json === FALSE) {
        $error .= "Failed to fetch $dataName.<br>";
    } else {
        $data = json_decode($json, true);
    }
}

// Fetch profile and recent games
fetchData($userSummaryUrl, $profile, $error, 'profile');
fetchData($recentGamesUrl, $recentGames, $error, 'recent games');

// Extract profile info if available
$player = $profile['response']['players'][0] ?? null;
$games = $recentGames['response']['games'] ?? [];

?>

<!-- HTML Page Layout -->
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
                <a class="btn btn-dark w-100 mt-2" href="steam-info.php" role="button">
                    Steam
                </a>
            </div>

            <div class="mt-2">
                <a class="btn btn-dark w-100 mt-2" href="../music/lastfm-info.php" role="button">
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
                    <h2>All Games</h2>
                </div>

                <div class="card bg-dark text-light mb-4">
                    <div class="card-body">
                        <input type="text" class="form-control" placeholder="Search Titles...">
                    </div>
                </div>
                <!-- User Profile -->
                <?php if ($player): ?>
                    <div class="card bg-dark text-light mb-4">
                        <div class="card-body d-flex align-items-center">
                            <img src="<?php echo htmlspecialchars($player['avatarfull']); ?>" class="rounded me-3" alt="Avatar">
                            <div>
                                <h4 class="mb-0"><?php echo htmlspecialchars($player['personaname']); ?></h4>
                                <p class="mb-0"><?php echo htmlspecialchars($player['profileurl']); ?></p>
                            </div>
                        </div>
                    </div>
                <?php endif; ?>

                <!-- Recent Games -->
                <h3>Recently Played Games</h3>
                <?php if (!empty($games)): ?>
                    <div class="table-responsive">
                        <table class="table table-dark table-hover">
                            <thead>
                                <tr>
                                    <th>Game</th>
                                    <th>Playtime (Last 2 Weeks)</th>
                                    <th>Total Playtime</th>
                                </tr>
                            </thead>
                            <tbody>
                                <?php foreach ($games as $game): ?>
                                    <tr>
                                        <td><?php echo htmlspecialchars($game['name']); ?></td>
                                        <td><?php echo round($game['playtime_2weeks'] / 60, 1); ?> hrs</td>
                                        <td><?php echo round($game['playtime_forever'] / 60, 1); ?> hrs</td>
                                    </tr>
                                <?php endforeach; ?>
                            </tbody>
                        </table>
                    </div>
                <?php else: ?>
                    <p>No recent games found.</p>
                <?php endif; ?>

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