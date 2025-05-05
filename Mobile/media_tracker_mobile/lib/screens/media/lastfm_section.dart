import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:media_tracker_test/models/lastfm/lastfm_album.dart';
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
  List<LastFmTrack> _lovedTracks = [];
  List<LastFmTrack> _topTracks = [];
  List<LastFmAlbum> _topAlbums = [];
  bool _isLoadingLoved = true;
  bool _isLoadingTopTracks = true;
  bool _isLoadingTopAlbums = true;

  @override
  void initState() {
    super.initState();
    _loadLovedTracks();
    _loadTopTracks();
    _loadTopAlbums();
  }

  Future<void> _loadLovedTracks() async {
    try {
      final tracks = await fetchLastFmLovedTracks();
      setState(() {
        _lovedTracks = tracks;
        _isLoadingLoved = false;
      });
    } catch (e) {
      print('Error loading loved tracks: $e');
      setState(() => _isLoadingLoved = false);
    }
  }

  Future<void> _loadTopTracks() async {
    try {
      final tracks = await fetchLastFmTopTracks();
      setState(() {
        _topTracks = tracks;
        _isLoadingTopTracks = false;
      });
    } catch (e) {
      print('Error loading top tracks: $e');
      setState(() => _isLoadingTopTracks = false);
    }
  }

  Future<void> _loadTopAlbums() async {
    try {
      final albums = await fetchLastFmTopAlbums();
      setState(() {
        _topAlbums = albums;
        _isLoadingTopAlbums = false;
      });
    } catch (e) {
      print('Error loading top albums: $e');
      setState(() => _isLoadingTopAlbums = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return DefaultTabController(
      length: 5,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const TabBar(
            isScrollable: true,
            tabAlignment: TabAlignment.start,
            tabs: [
              Tab(text: 'Top Artists'),
              Tab(text: 'Recent Tracks'),
              Tab(text: 'Loved Tracks'),
              Tab(text: 'Top Songs'),
              Tab(text: 'Top Albums'),
            ],
            indicatorColor: Colors.grey,
            labelColor: Colors.white,
            unselectedLabelColor: Colors.grey,
          ),
          Expanded(
            child: TabBarView(
              children: [
                _buildTopArtistsList(context),
                _buildRecentTracksList(),
                _buildLovedTracksList(),
                _buildTopTracksList(),
                _buildTopAlbumsList(),
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
                albumBasic['albumArtist'] ?? track.artist,
                albumBasic['albumName'],
              );

              Navigator.push(
                context,
                MaterialPageRoute(
                  builder:
                      (_) => MediaDetailsScreen(
                        appId: '', 
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

  Widget _buildLovedTracksList() {
    if (_isLoadingLoved) {
      return const Center(child: CircularProgressIndicator());
    }
    if (_lovedTracks.isEmpty) {
      return const Center(child: Text("No loved tracks found."));
    }

    final sortedLoved = applySorting(
      list: _lovedTracks,
      option: widget.sortOption,
      direction: widget.sortDirection,
      getField:
          (track) =>
              widget.sortOption == SortOption.name
                  ? track.name.toLowerCase()
                  : track.playCount ?? 0,
    );

    return ListView.builder(
      itemCount: sortedLoved.length,
      itemBuilder: (context, index) {
        final track = sortedLoved[index];
        return ListTile(
          title: Text(track.name),
          subtitle: Text("By: ${track.artist}"),
          trailing: Text(track.formattedDate ?? ""),
          onTap: () async {
            try {
              final albumBasic = await fetchAlbumDetail(
                track.artist,
                track.name,
              );
              if (albumBasic['albumName'] == null) {
                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(
                    content: Text("No album information for '${track.name}'"),
                  ),
                );
                return;
              }
              final albumDetails = await fetchFullAlbumDetails(
                track.artist,
                albumBasic['albumName'],
              );

              Navigator.push(
                context,
                MaterialPageRoute(
                  builder:
                      (_) => MediaDetailsScreen(
                        appId: '',
                        title: albumDetails['albumName'],
                        subtitle: 'Album by ${track.artist}',
                        mediaType: MediaType.lastfmAlbum,
                        posterPath: albumDetails['albumImageUrl'],
                        artistName: track.artist,
                      ),
                ),
              );
            } catch (e) {
              ScaffoldMessenger.of(context).showSnackBar(
                const SnackBar(content: Text('Failed to load album details')),
              );
            }
          },
        );
      },
    );
  }

  Widget _buildTopTracksList() {
    if (_isLoadingTopTracks) {
      return const Center(child: CircularProgressIndicator());
    }

    if (_topTracks.isEmpty) {
      return const Center(child: Text("No top tracks found."));
    }

    final sortedTopTracks = applySorting(
      list: _topTracks,
      option: widget.sortOption,
      direction: widget.sortDirection,
      getField:
          (track) =>
              widget.sortOption == SortOption.name
                  ? track.name.toLowerCase()
                  : track.playCount ?? 0,
    );

    return ListView.builder(
      itemCount: sortedTopTracks.length,
      itemBuilder: (context, index) {
        final track = sortedTopTracks[index];
        return ListTile(
          title: Text(track.name),
          subtitle: Text('By: ${track.artist}'),
          trailing: Text(
            '${track.playCount} plays',
            style: const TextStyle(color: Colors.grey, fontSize: 12),
          ),
        );
      },
    );
  }

  Widget _buildTopAlbumsList() {
    if (_isLoadingTopAlbums) {
      return const Center(child: CircularProgressIndicator());
    }

    if (_topAlbums.isEmpty) {
      return const Center(child: Text("No top albums found."));
    }

    final sortedAlbums = applySorting(
      list: _topAlbums,
      option: widget.sortOption,
      direction: widget.sortDirection,
      getField:
          (album) =>
              widget.sortOption == SortOption.name
                  ? album.name.toLowerCase()
                  : album.playCount ?? 0,
    );

    return ListView.builder(
      itemCount: sortedAlbums.length,
      itemBuilder: (context, index) {
        final album = sortedAlbums[index];
        return ListTile(
          title: Text(album.name),
          subtitle: Text('By: ${album.artist}'),
          leading:
              album.imageUrl != null
                  ? Image.network(
                    album.imageUrl!,
                    width: 50,
                    height: 50,
                    fit: BoxFit.cover,
                  )
                  : const Icon(Icons.album),
          trailing: Text(
            '${album.playCount} plays',
            style: const TextStyle(color: Colors.grey, fontSize: 12),
          ),
        );
      },
    );
  }
}
