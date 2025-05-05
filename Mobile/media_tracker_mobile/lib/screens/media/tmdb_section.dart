import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:media_tracker_test/models/sort_options.dart';
import 'package:media_tracker_test/providers/auth_provider.dart';
import 'package:media_tracker_test/providers/favorites_provider.dart';
import 'package:media_tracker_test/services/user_account_services.dart';
import '../../models/tmdb/tmdb_account.dart';
import '../../models/tmdb/tmdb_movie.dart';
import '../../models/tmdb/tmdb_tv_show.dart';
import 'package:media_tracker_test/screens/media/media_details_screen.dart';

class TmdbSection extends ConsumerStatefulWidget {
  final TMDBAccount? account;
  final List<TMDBMovie> ratedMovies;
  final List<TMDBTvShow> favoriteTvShows;
  final SortOption sortOption;
  final SortDirection sortDirection;

  const TmdbSection({
    super.key,
    required this.account,
    required this.ratedMovies,
    required this.favoriteTvShows,
    required this.sortOption,
    required this.sortDirection,
  });

  @override
  ConsumerState<TmdbSection> createState() => _TmdbSectionState();
}

class _TmdbSectionState extends ConsumerState<TmdbSection> {
  @override
  Widget build(BuildContext context) {
    return DefaultTabController(
      length: 2,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const TabBar(
            tabs: [Tab(text: 'Rated Movies'), Tab(text: 'Rated TV Shows')],
            indicatorColor: Colors.grey,
            labelColor: Colors.white,
            unselectedLabelColor: Colors.grey,
          ),
          Expanded(
            child: TabBarView(
              children: [
                _buildMediaList(context, widget.ratedMovies, isMovie: true),
                _buildMediaList(
                  context,
                  widget.favoriteTvShows,
                  isMovie: false,
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildMediaList(
    BuildContext context,
    List<dynamic> items, {
    required bool isMovie,
  }) {
    final auth = ref.watch(authProvider);
    final favorites = ref.watch(favoritesProvider);

    final sortedItems = applySorting(
      list: items,
      option: widget.sortOption,
      direction: widget.sortDirection,
      getField: (item) {
        if (widget.sortOption == SortOption.name) {
          return item.title.toLowerCase();
        } else if (widget.sortOption == SortOption.playtime) {
          return item.releaseDate ?? '';
        } else {
          return item.title.toLowerCase(); // fallback
        }
      },
      isFavorite: (item) => item.isFavorite,
    );

    return ListView.builder(
      itemCount: sortedItems.length,
      itemBuilder: (context, index) {
        final item = sortedItems[index];
        final title = item.title;

        // Check if this item is favorited
        item.isFavorite = favorites.any(
          (fav) =>
              fav['media']['platform_id'] == 3 &&
              fav['media']['media_plat_id'] == item.id.toString() &&
              fav['favorites'] == true,
        );

        return ListTile(
          title: Text(title),
          subtitle: Text(
            item.releaseDate != null && item.releaseDate!.isNotEmpty
                ? 'Release Date: ${_formatDate(item.releaseDate!)}'
                : 'Release Date: Unknown',
          ),
          trailing: IconButton(
            icon: Icon(
              item.isFavorite ? Icons.star : Icons.star_border,
              color: item.isFavorite ? Colors.yellow : Colors.grey,
            ),
            onPressed: () async {
              final success = await UserAccountServices().toggleFavoriteMedia(
                platformId: 3, // Steam, TMDB. last.fm
                mediaTypeId:
                    isMovie
                        ? 3
                        : 2, // Media type name ("Game", "TV Show", "Film", "Song", "Album", or "Artist")
                mediaPlatId: item.id.toString(),
                title: title,
                username: auth.username!,
              );

              if (success) {
                final updatedFavorites = await UserAccountServices()
                    .fetchUserFavorites(auth.username!);
                ref.read(favoritesProvider.notifier).state = updatedFavorites;
              } else {
                ScaffoldMessenger.of(context).showSnackBar(
                  const SnackBar(content: Text('Failed to favorite')),
                );
              }
            },
          ),
          onTap: () {
            Navigator.push(
              context,
              MaterialPageRoute(
                builder:
                    (_) => MediaDetailsScreen(
                      appId: item.id.toString(),
                      title: item.title,
                      subtitle: item.overview,
                      genreIds: item.genreIds,
                      releaseDate: item.releaseDate,
                      posterPath: item.posterPath,
                      mediaType: MediaType.tmdb,
                    ),
              ),
            );
          },
        );
      },
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
}
