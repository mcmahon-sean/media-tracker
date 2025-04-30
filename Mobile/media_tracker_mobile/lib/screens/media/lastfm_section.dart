import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:media_tracker_test/models/sort_options.dart';
import 'package:media_tracker_test/providers/auth_provider.dart';
import 'package:media_tracker_test/providers/favorites_provider.dart';
import 'package:media_tracker_test/services/user_account_services.dart';
import '../../models/lastfm/lastfm_top_artist.dart';
import '../../models/lastfm/lastfm_track.dart';
import '../../models/lastfm/lastfm_user.dart';
import '../../services/media_api/lastfm_service.dart';
import '../../screens/media/media_details_screen.dart';

class LastFmSection extends ConsumerStatefulWidget {
  final LastFmUser? user;
  final List<TopArtist> topArtists;
  final List<LastFmTrack> recentTracks;
  final SortOption sortOption;
  final SortDirection sortDirection;

  const LastFmSection({
    super.key,
    required this.user,
    required this.topArtists,
    required this.recentTracks,
    required this.sortOption,
    required this.sortDirection,
  });

  @override
  ConsumerState<LastFmSection> createState() => _LastFmSectionState();
}

class _LastFmSectionState extends ConsumerState<LastFmSection> {
  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return DefaultTabController(
      length: 2,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const TabBar(
            tabs: [Tab(text: 'Top Artists'), Tab(text: 'Recent Tracks')],
            indicatorColor: Colors.grey,
            labelColor: Colors.white,
            unselectedLabelColor: Colors.grey,
          ),
          Expanded(
            child: TabBarView(
              children: [
                _buildTopArtistsList(context),
                _buildRecentTracksList(),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildTopArtistsList(BuildContext context) {
    final auth = ref.watch(authProvider);
    final favorites = ref.watch(favoritesProvider);

    final sortedArtists = applySorting(
      list: widget.topArtists,
      option: widget.sortOption,
      direction: widget.sortDirection,
      getField: (artist) {
        if (widget.sortOption == SortOption.name) {
          return artist.name.toLowerCase();
        } else if (widget.sortOption == SortOption.playtime) {
          return artist.playCount;
        } else {
          return artist.name.toLowerCase(); // fallback
        }
      },
      isFavorite: (artist) {
        final favorites = ref.read(favoritesProvider);
        return favorites.any(
          (fav) =>
              fav['media']['platform_id'] == 2 &&
              fav['media']['media_plat_id'].toString().toLowerCase().trim() ==
                  artist.name.toLowerCase().trim() &&
              fav['favorites'] == true,
        );
      },
    );

    return ListView.builder(
      itemCount: sortedArtists.length,
      itemBuilder: (context, index) {
        final artist = sortedArtists[index];

        final isFavorite = favorites.any(
          (fav) =>
              fav['media']['platform_id'] == 2 &&
              fav['media']['media_plat_id'].toString().toLowerCase().trim() ==
                  artist.name.toLowerCase().trim() &&
              fav['favorites'] == true,
        );

        return ListTile(
          title: Text(artist.name),
          subtitle: Text("Plays: ${artist.playCount}"),
          trailing: IconButton(
            icon: Icon(
              isFavorite ? Icons.star : Icons.star_border,
              color: isFavorite ? Colors.yellow : Colors.grey,
            ),
            onPressed: () async {
              //print('Favorite icon clicked for index $index: ${artist.name}'); // DEBUGGING
              final success = await UserAccountServices().toggleFavoriteMedia(
                platformId: 2, // Steam, TMDB. last.fm
                mediaTypeId:
                    6, // Media type name ("Game", "TV Show", "Film", "Song", "Album", or "Artist")
                mediaPlatId: artist.name.toLowerCase().trim(),
                title: artist.name,
                artist: artist.name,
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
          onTap: () async {
            try {
              final details = await fetchArtistDetail(artist.name);

              Navigator.push(
                context,
                MaterialPageRoute(
                  builder:
                      (_) => MediaDetailsScreen(
                        appId: '',
                        title: details.name,
                        subtitle:
                            'Plays: ${details.playCount} • Listeners: ${details.listenerCount}',
                        mediaType: MediaType.lastfm,
                        genreIds: null,
                        releaseDate: '',
                        posterPath: details.imageUrl,
                      ),
                ),
              );
            } catch (e) {
              ScaffoldMessenger.of(context).showSnackBar(
                const SnackBar(content: Text('Failed to load artist details')),
              );
            }
          },
        );
      },
    );
  }

  Widget _buildRecentTracksList() {
    return ListView.builder(
      itemCount: widget.recentTracks.length,
      itemBuilder: (context, index) {
        final track = widget.recentTracks[index];
        return ListTile(
          title: Text(track.name),
          subtitle: Text("By: ${track.artist}"),
          trailing: Text(track.formattedDate ?? "Now Playing"),
          onTap: () async {
            try {
              // Fetch album info for this track
              final albumBasic = await fetchAlbumDetail(
                track.artist,
                track.name,
              );

              if (albumBasic['albumName'] == null) {
                ScaffoldMessenger.of(context).showSnackBar(
                  const SnackBar(
                    content: Text('Album info not found for this track.'),
                  ),
                );
                return;
              }

              // Fetch full album info
              final albumDetails = await fetchFullAlbumDetails(
                track.artist,
                albumBasic['albumName'],
              );

              Navigator.push(
                context,
                MaterialPageRoute(
                  builder:
                      (_) => MediaDetailsScreen(
                        appId: '', // Not needed
                        title: albumDetails['albumName'],
                        subtitle: 'Album by ${track.artist}',
                        mediaType: MediaType.lastfmAlbum,
                        genreIds: null,
                        releaseDate: '',
                        posterPath: albumDetails['albumImageUrl'],
                        artistName: track.artist,
                      ),
                ),
              );
            } catch (e) {
              ScaffoldMessenger.of(context).showSnackBar(
                const SnackBar(content: Text('Failed to load album details')),
              );
              print('Failed to load album from track tap: $e');
            }
          },
        );
      },
    );
  }
}
