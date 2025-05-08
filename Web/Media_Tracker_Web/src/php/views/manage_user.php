<?php
    
    require_once "../db.php";
    
    session_start();

    // Check if the session username is set
    if (!isset($_SESSION['username'])) {
        $error['error_message'] = 'You are not logged in.';
    } else {
        // Retrieve username from session
        $username = $_SESSION['username'];

        // Fetch user details from db
        $stmt = $pdo->prepare("SELECT * FROM users WHERE username = ?");
        $stmt->execute([$username]);
        $result = $stmt->fetch(PDO::FETCH_ASSOC);

        if ($result) {
            $_SESSION['first_name'] = $result["first_name"];
            $_SESSION['last_name'] = $result["last_name"];
            $_SESSION['email'] = $result["email"];

            $firstName = $_SESSION["first_name"] ?? "N/A";
            $lastName = $_SESSION["last_name"] ?? "N/A";
            $email = $_SESSION["email"] ?? "N/A";
        }

    }
    $editMode = isset($_GET['edit']); // Check if we are in edit mode
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Manage Users/3rd Party Accounts</title>
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
                    <a class="btn btn-dark w-100 mt-2" id="btn-home" href="./manage_user.php" role="button">
                        Manage user
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
                            Manage Users and Platforms
                        </small>
                    </h2>
                </div>

                <div class="table-responsive">
                    <div class="form-container">
                        <?php if (empty($error)) { ?>
                            <!-- Display logged-in user information -->
                            <?php if ($editMode): ?>
                                <!-- Update Form -->
                                <form action="../database/update_user.php" method="POST">
                                    <input type="hidden" name="username" value="<?php echo htmlspecialchars($username); ?>">

                                    <div class="mb-3">
                                        <label for="firstName" class="form-label">First Name</label>
                                        <input type="text" class="form-control" id="firstName" name="first_name" value="<?php echo htmlspecialchars($firstName); ?>">
                                    </div>

                                    <div class="mb-3">
                                        <label for="lastName" class="form-label">Last Name</label>
                                        <input type="text" class="form-control" id="lastName" name="last_name" value="<?php echo htmlspecialchars($lastName); ?>">
                                    </div>

                                    <div class="mb-3">
                                        <label for="email" class="form-label">Email</label>
                                        <input type="email" class="form-control" id="email" name="email" value="<?php echo htmlspecialchars($email); ?>">
                                    </div>

                                    <button type="submit" class="btn btn-success">Save Changes</button>
                                    <a href="manage_user.php" class="btn btn-secondary">Cancel</a>
                                </form>
                            <?php else: ?>
                                <p><strong>Username:</strong> <?php echo htmlspecialchars($username); ?></p>
                                <p><strong>First Name:</strong> <?php echo htmlspecialchars($firstName); ?></p>
                                <p><strong>Last Name:</strong> <?php echo htmlspecialchars($lastName); ?></p>
                                <p><strong>Email:</strong> <?php echo htmlspecialchars($email); ?></p>

                                <!-- Edit Button -->
                                <form method="GET" action="">
                                    <button type="submit" name="edit" value="1" class="btn btn-primary">Edit</button>
                                </form>
                            <?php endif; ?>

                            <hr>
                            <!--Check to see if user has added a Steam account. If not, display message telling user to add an account-->
                            <?php if (isset($_SESSION['user_platform_ids']['steam'])): ?>
                            <p>Steam ID: <strong><?= htmlspecialchars($_SESSION['user_platform_ids']['steam']) ?></strong></p>
                            <?php else: ?>
                            <p>Please add your Steam account</p>
                            <?php endif; ?>

                            <!--Check to see if user has added a Last.fm account. If not, display message telling user to add an account-->
                            <?php if (isset($_SESSION['user_platform_ids']['lastfm'])): ?>
                            <p>Last.fm ID: <strong><?= htmlspecialchars($_SESSION['user_platform_ids']['lastfm']) ?></strong></p>
                            <?php else: ?>
                            <p>Last.fm ID: Please add your Last.fm account</p>
                            <?php endif; ?>

                            <!--Check to see if user has added a TMDB account. If not, display message telling user to add an account-->
                            <?php if (isset($_SESSION['user_platform_ids']['tmdb'])): ?>
                            <p>TMDB ID: <strong><?= htmlspecialchars($_SESSION['user_platform_ids']['tmdb']) ?></strong></p>
                            <?php else: ?>
                            <p>TMDB ID: Please add your TMDB account</p>
                            <?php endif; ?>

                            <!-- Button to delete the user -->
                            <h4>Delete User</h4>
                            <form action="../authentication/delete_user.php" method="POST">
                                <input type="hidden" name="username" value="<?php echo htmlspecialchars($username); ?>">
                                <button type="submit" class="btn btn-danger">Delete My Account</button>
                            </form>

                            <hr>

                            <!-- Form to delete a 3rd party account -->
                            <h4>Delete 3rd Party Account</h4>
                            <form action="../database/delete_3rd_party_ids.php" method="POST">
                                <div class="row mb-3 mx-1">
                                    <label class="form-label col-xxl-2 col-lg-3 col-md-5 lead" for="platform_id">
                                        Choose Platform:
                                    </label>
                                    <div class="col-xl-2 col-lg-3 col-md-4">
                                        <select class="form-select" name="platform_id" id="platform_id">
                                            <option value="1" <?php echo isset($_SESSION['user_platform_ids']['steam']) ? 'selected' : ''; ?>>Steam</option>
                                            <option value="2" <?php echo isset($_SESSION['user_platform_ids']['lastfm']) ? 'selected' : ''; ?>>Last.fm</option>
                                            <option value="3" <?php echo isset($_SESSION['user_platform_ids']['tmdb']) ? 'selected' : ''; ?>>TMDB</option>
                                        </select>
                                    </div>
                                </div>
                                <button type="submit" class="btn btn-danger">Delete 3rd Party Account</button>
                            </form>
                        <?php } ?>
                    </div>

                    <!-- Display Errors Below -->
                    <div id="errorMessages" class="mt-3">
                        <?php
                            if (!empty($error)) {
                                echo '<div class="alert alert-danger">' . htmlspecialchars($error['error_message']) . '</div>';
                            }
                        ?>
                    </div>
                </div>
            </main>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
