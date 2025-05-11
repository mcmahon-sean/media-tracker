import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:media_tracker_test/models/steam/steam_model.dart';
import 'package:media_tracker_test/models/tmdb/tmdb_movie.dart';
import 'package:media_tracker_test/models/tmdb/tmdb_tv_show.dart';
import 'package:media_tracker_test/models/lastfm/lastfm_top_artist.dart';
import 'package:media_tracker_test/providers/favorites_provider.dart';
import 'package:media_tracker_test/providers/auth_provider.dart';

class FavoritesScreen extends ConsumerWidget {
  final List<SteamGame> steamGames;
  final List<TMDBMovie> tmdbMovies;
  final List<TMDBTvShow> tmdbTvShows;
  final List<TopArtist> topArtists;

  const FavoritesScreen({
    super.key,
    required this.steamGames,
    required this.tmdbMovies,
    required this.tmdbTvShows,
    required this.topArtists,
  });

  String safeImageUrl(String? url) {
    if (url == null || !url.startsWith('http')) {
      return 'https://via.placeholder.com/300x400?text=No+Image';
    }
    return url;
  }

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final auth = ref.watch(authProvider);
    final favorites = ref.watch(favoritesProvider);
    final firstName = auth.firstName ?? 'User';

    final displaySteam =
        steamGames
            .where(
              (game) => favorites.any(
                (fav) =>
                    fav['media']['platform_id'] == 1 &&
                    fav['media']['media_plat_id'] == game.appId.toString() &&
                    fav['favorites'] == true,
              ),
            )
            .toList();

    final displayMovies =
        tmdbMovies
            .where(
              (movie) => favorites.any(
                (fav) =>
                    fav['media']['platform_id'] == 3 &&
                    fav['media']['media_type_id'] == 3 &&
                    fav['media']['media_plat_id'] == movie.id.toString() &&
                    fav['favorites'] == true,
              ),
            )
            .toList();

    final displayShows =
        tmdbTvShows
            .where(
              (show) => favorites.any(
                (fav) =>
                    fav['media']['platform_id'] == 3 &&
                    fav['media']['media_type_id'] == 2 &&
                    fav['media']['media_plat_id'] == show.id.toString() &&
                    fav['favorites'] == true,
              ),
            )
            .toList();

    final favoriteArtists =
        topArtists
            .where(
              (artist) => favorites.any(
                (fav) =>
                    fav['media']['platform_id'] == 2 &&
                    fav['media']['media_type_id'] == 6 &&
                    fav['media']['media_plat_id']
                            .toString()
                            .toLowerCase()
                            .trim() ==
                        artist.name.toLowerCase().trim() &&
                    fav['favorites'] == true,
              ),
            )
            .toList();

    return Scaffold(
      appBar: AppBar(title: Text("$firstName's Favorites")),
      body: SingleChildScrollView(
        padding: const EdgeInsets.symmetric(vertical: 16, horizontal: 12),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            _buildMediaSection("Favorite Games", displaySteam, "Steam"),
            _buildMediaSection("Favorite Movies", displayMovies, "TMDB"),
            _buildMediaSection("Favorite TV Shows", displayShows, "TMDB"),
            _buildMediaSection("Favorite Artists", favoriteArtists, "Last.fm"),
          ],
        ),
      ),
    );
  }

  Widget _buildMediaSection<T>(String title, List<T> items, String platform) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          title,
          style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
        ),
        const SizedBox(height: 8),
        if (items.isEmpty)
          Container(
            height: 160,
            alignment: Alignment.center,
            padding: const EdgeInsets.symmetric(horizontal: 16),
            decoration: BoxDecoration(
              color: Colors.grey[850],
              borderRadius: BorderRadius.circular(12),
            ),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: const [
                Icon(Icons.star_border, color: Colors.grey),
                SizedBox(width: 8),
                Text('No favorites yet', style: TextStyle(color: Colors.grey)),
              ],
            ),
          )
        else
          SizedBox(
            height: 240,
            child: ListView.separated(
              scrollDirection: Axis.horizontal,
              itemCount: items.length,
              separatorBuilder: (_, __) => const SizedBox(width: 12),
              itemBuilder: (context, index) {
                final item = items[index];
                String title = '';
                String? subtitle;
                String? imageUrl;

                if (item is SteamGame) {
                  title = item.name;
                  subtitle = _formatPlaytime(item.playtimeForever);
                  imageUrl = safeImageUrl(item.headerImage);
                } else if (item is TMDBMovie) {
                  title = item.title;
                  subtitle = _formatDate(item.releaseDate);
                  imageUrl =
                      'https://image.tmdb.org/t/p/w500${item.posterPath}';
                } else if (item is TMDBTvShow) {
                  title = item.title;
                  subtitle = _formatDate(item.releaseDate);
                  imageUrl =
                      'https://image.tmdb.org/t/p/w500${item.posterPath}';
                } else if (item is TopArtist) {
                  title = item.name;
                  subtitle = 'Play Count: ${item.playCount}';
                  imageUrl = safeImageUrl(item.topAlbumImageUrl);
                }

                return SizedBox(
                  width: 150,
                  child: _FavoriteMediaCard(
                    title: title,
                    subtitle: subtitle,
                    imageUrl: imageUrl,
                    platform: platform,
                    onTap: () {},
                  ),
                );
              },
            ),
          ),
        const SizedBox(height: 24),
      ],
    );
  }

  // Helper function to format the release date
  String _formatDate(String dateStr) {
    try {
      final date = DateTime.parse(dateStr);
      return '${_monthName(date.month)} ${date.day}, ${date.year}';
    } catch (e) {
      return dateStr; // fallback if parsing fails
    }
  }

  String _monthName(int month) {
    const months = [
      'Jan',
      'Feb',
      'Mar',
      'Apr',
      'May',
      'Jun',
      'Jul',
      'Aug',
      'Sep',
      'Oct',
      'Nov',
      'Dec',
    ];
    return months[month - 1];
  }

  // Helper method to convert playtime to something more readable
  String _formatPlaytime(int minutes) {
    final hours = minutes ~/ 60;
    final remainingMinutes = minutes % 60;

    if (hours > 0 && remainingMinutes > 0) {
      return '$hours hrs $remainingMinutes min';
    } else if (hours > 0) {
      return '$hours hrs';
    } else {
      return '$remainingMinutes min';
    }
  }
}

class _FavoriteMediaCard extends StatelessWidget {
  final String title;
  final String? subtitle;
  final String? imageUrl;
  final String platform;
  final VoidCallback onTap;

  const _FavoriteMediaCard({
    required this.title,
    this.subtitle,
    this.imageUrl,
    required this.platform,
    required this.onTap,
  });

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: onTap,
      child: Card(
        elevation: 4,
        color: Colors.grey[850],
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
        clipBehavior: Clip.antiAlias,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            imageUrl != null
                ? Image.network(
                  imageUrl!,
                  width: 150,
                  height: 160,
                  fit: BoxFit.cover,
                  errorBuilder: (_, __, ___) => _fallbackImage(),
                )
                : _fallbackImage(),
            Padding(
              padding: const EdgeInsets.all(8.0),
              child: Text(
                title,
                style: const TextStyle(
                  fontWeight: FontWeight.bold,
                  overflow: TextOverflow.ellipsis,
                ),
              ),
            ),
            if (subtitle != null)
              Padding(
                padding: const EdgeInsets.symmetric(horizontal: 8.0),
                child: Text(
                  subtitle!,
                  style: const TextStyle(fontSize: 12, color: Colors.grey),
                  overflow: TextOverflow.ellipsis,
                ),
              ),
          ],
        ),
      ),
    );
  }

  Widget _fallbackImage() {
    return Image.asset(
      'assets/images/no_image.jpg',
      width: 150,
      height: 160,
      fit: BoxFit.cover,
    );
  }
}
