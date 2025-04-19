import 'package:flutter/material.dart';
import '../../models/tmdb/tmdb_account.dart';
import '../../models/tmdb/tmdb_movie.dart';
import '../../models/tmdb/tmdb_tv_show.dart';

Widget buildTmdbSection(
  TMDBAccount? account,
  List<TMDBMovie> ratedMovies,
  List<TMDBTvShow> favoriteTvShows,
) {
  return ListView(
    padding: const EdgeInsets.all(12),
    children: [
      if (account != null) ...[
        Text(
          "User: ${account.username}",
          style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
        ),
        const SizedBox(height: 12),
      ],
      const Text(
        "Rated Movies",
        style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
      ),
      ...ratedMovies.map(
        (movie) =>
            ListTile(title: Text(movie.title), subtitle: Text(movie.overview)),
      ),
      const SizedBox(height: 12),
      const Text(
        "Favorite TV Shows",
        style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
      ),
      ...favoriteTvShows.map(
        (show) =>
            ListTile(title: Text(show.name), subtitle: Text(show.overview)),
      ),
    ],
  );
}
