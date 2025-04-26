import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:media_tracker_test/providers/auth_provider.dart';
import 'package:media_tracker_test/services/user_account_services.dart';
import '../../models/tmdb/tmdb_account.dart';
import '../../models/tmdb/tmdb_movie.dart';
import '../../models/tmdb/tmdb_tv_show.dart';
import 'package:media_tracker_test/screens/media/media_details_screen.dart';

class TmdbSection extends ConsumerStatefulWidget {
  final TMDBAccount? account;
  final List<TMDBMovie> ratedMovies;
  final List<TMDBTvShow> favoriteTvShows;

  const TmdbSection({
    super.key,
    required this.account,
    required this.ratedMovies,
    required this.favoriteTvShows,
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
            tabs: [Tab(text: 'Rated Movies'), Tab(text: 'Favorite TV Shows')],
            indicatorColor: Colors.grey,
            labelColor: Colors.white,
            unselectedLabelColor: Colors.grey,
          ),
          Expanded(
            child: TabBarView(
              children: [
                _buildMediaList(context, widget.ratedMovies, isMovie: true),
                _buildMediaList(context, widget.favoriteTvShows, isMovie: false),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildMediaList(
    BuildContext context,
    List items, {
    required bool isMovie,
  }) {
    final auth = ref.watch(authProvider);
    return ListView.builder(
      itemCount: items.length,
      itemBuilder: (context, index) {
        final item = items[index];
        final title = item.title;

        return ListTile(
          title: Text(title),
          trailing: IconButton(
            icon: Icon(
              item.isFavorite ? Icons.star : Icons.star_border,
              color: item.isFavorite ? Colors.yellow : Colors.grey,
            ),
            onPressed: () async {
              final success = await UserAccountServices().toggleFavoriteMedia(
                platformId: 3, // Steam, TMDB. last.fm
                mediaTypeId: isMovie ? 3 : 2, // Media type name ("Game", "TV Show", "Film", "Song", "Album", or "Artist")
                mediaPlatId: item.id.toString(),
                title: title,
                username: auth.username!,
              );

              if (success) {
                setState(() {
                  if (isMovie || !isMovie) {
                    item.isFavorite = !item.isFavorite;
                  }
                });
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
}
