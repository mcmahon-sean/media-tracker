<?php

session_start();

// TEMP CODE START
require_once 'src/php/config.php';
//$_SESSION['session_id'] = TMDB_SESSION_ID;
// TEMP CODE END

?>

<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Media Tracker</title>
    <link
      href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"
      rel="stylesheet"
    />
    <script
      src="https://kit.fontawesome.com/a076d05399.js"
      crossorigin="anonymous"
    ></script>
    <link rel="stylesheet" href="styles.css" />
  </head>
  <body class="bg-dark-primary">
    <div class="container-fluid">
      <div class="row">
        <nav class="col-md-2 d-none d-md-block sidebar bg-dark-secondary">
          <div>
            <a
              class="btn btn-dark w-100"
              id="btn-home"
              href="index.php"
              role="button"
            >
              Home
            </a>
          </div>
          <hr />
          <div class="dropdown">
            <a
              class="btn btn-dark dropdown-toggle w-100 media-tab"
              href="#"
              role="button"
              data-bs-toggle="dropdown"
            >
              <img
                src="src/assets/images/icons/icon_music.svg"
                class="tab-icon me-2"
              />
              Last.fm
            </a>
            <ul class="dropdown-menu">
              <li>
                <a class="dropdown-item" href="src/php/views/lastfm_all.php"
                  >All Music</a
                >
              </li>
              <li>
                <a class="dropdown-item" href="src/php/views/lastfm_loved_tracks.php"
                  >Loved Tracks</a
                >
              </li>
              <li>
                <a class="dropdown-item" href="src/php/views/lastfm_recent_tracks.php"
                  >Recent Tracks</a
                >
              </li>
              <li>
                <a class="dropdown-item" href="src/php/views/lastfm_top_albums.php"
                  >Top Albums</a
                >
              </li>
              <li>
                <a class="dropdown-item" href="src/php/views/lastfm_top_artists.php"
                  >Top Artists</a
                >
              </li>
              <li>
                <a class="dropdown-item" href="src/php/views/lastfm_top_tracks.php"
                  >Top Tracks</a
                >
              </li>
            </ul>
          </div>

          <div class="dropdown mt-2">
            <a
              class="btn btn-dark dropdown-toggle w-100 media-tab"
              href="#"
              role="button"
              data-bs-toggle="dropdown"
            >
              <img
                src="src/assets/images/icons/icon_movies.svg"
                class="tab-icon me-2"
              />
              TMDB
            </a>
            <ul class="dropdown-menu">
              <li><a class="dropdown-item" href="src/php/views/tmdb_favorite_movies.php">Favorite Movies</a></li>
              <li><a class="dropdown-item" href="src/php/views/tmdb_rated_movies.php">Rated Movies</a></li>
              <li><a class="dropdown-item" href="src/php/views/tmdb_favorite_tv_shows.php">Favorite TV Shows</a></li>
              <li><a class="dropdown-item" href="src/php/views/tmdb_rated_tv_shows.php">Rated TV Shows</a></li>
            </ul>
          </div>

          <div class="dropdown mt-2">
            <a
              class="btn btn-dark dropdown-toggle w-100 media-tab"
              href="#"
              role="button"
              data-bs-toggle="dropdown"
            >
              <img
                src="src/assets/images/icons/icon_games.svg"
                class="tab-icon me-2"
              />
              Steam
            </a>
            <ul class="dropdown-menu">
              <li><a class="dropdown-item" href="src/php/views/steam_owned_games.php">Owned Games</a></li>
            </ul>
          </div>
        </nav>

        <main class="col-md-10 ms-sm-auto px-4">
          <div
            class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom"
          >
            <h2>Media Tracker</h2>
          </div>

          <div class="card bg-dark text-light mb-4">
            <div class="card-body">
              <input
                type="text"
                class="form-control"
                placeholder="Search Titles..."
              />
            </div>
          </div>
          <?php if (isset($_SESSION['session_id']) && !empty($_SESSION['session_id'])): ?>
              <p>SESSION ID: <strong><?= htmlspecialchars($_SESSION['session_id']) ?></strong></p>
          <?php endif; ?>

          <?php if (isset($_SESSION['signed_in'])): ?>
            <p>Hello, <strong><?= htmlspecialchars($_SESSION['username']) ?></strong>!</p>

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

            <button onclick="window.location.href='src/php/views/add_edit_3rd_party.php'">Add/Edit Platforms</button>
            <button onclick="window.location.href='src/php/authentication/logout.php'">Logout</button>
          <?php else: ?>
            <button onclick="window.location.href='src/php/views/user_login.php'">Login</button>
            <button onclick="window.location.href='src/php/views/user_signup.php'">Sign Up</button>
          <?php endif; ?>
        </main>
      </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script src="supabaseClient.js" type="module"></script>
    <script src="script.js" type="module"></script>

  </body>
</html>
