import 'package:flutter/material.dart';
import '../../models/tmdb/tmdb_account.dart';
import '../../models/tmdb/tmdb_movie.dart';
import '../../models/tmdb/tmdb_tv_show.dart';
import 'package:media_tracker_test/screens/media/media_details_screen.dart';

class TmdbSection extends StatelessWidget {
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
                _buildMediaList(context, ratedMovies, isMovie: true),
                _buildMediaList(context, favoriteTvShows, isMovie: false),
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
            onPressed: () {
              // toggle favorite logic
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
