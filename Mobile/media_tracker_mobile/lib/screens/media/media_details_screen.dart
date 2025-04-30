import 'package:flutter/material.dart';
import 'package:media_tracker_test/services/media_api/lastfm_service.dart';
import '../../models/tmdb/genre_helper.dart';
import '../../services/media_api/steam_service.dart';
import 'package:intl/intl.dart';

enum MediaType { steam, tmdb, lastfm, lastfmAlbum }

class MediaDetailsScreen extends StatefulWidget {
  final String appId; // Required for Steam, optional for TMDB
  final String title; // Title of the media (game name, movie title, artist name, etc.)
  final String subtitle; // Subtitle text (overview, artist info, etc.)
  final MediaType mediaType; // Determines what kind of data to show
  final List<int>? genreIds; // For TMDB movies/shows
  final String? releaseDate; // Optional release date (for TMDB)
  final String? posterPath; // Optional poster or album cover path
  final String? artistName; // Used for Last.fm album drilldown

  const MediaDetailsScreen({
    super.key,
    required this.appId,
    required this.title,
    required this.subtitle,
    required this.mediaType,
    this.genreIds,
    this.releaseDate,
    this.posterPath,
    this.artistName,
  });

  @override
  State<MediaDetailsScreen> createState() => _MediaDetailsScreenState();
}

class _MediaDetailsScreenState extends State<MediaDetailsScreen> {
  String? _imageUrl; // URL for media image to display
  List<String> _extraDetails = []; // List of extra string details

  // For Last.fm artist detail
  List<Map<String, dynamic>> _topTracks = [];
  List<Map<String, dynamic>> _topAlbums = [];
  List<Map<String, dynamic>> _similarArtists = [];

  // Expansion booleans for artist detail
  bool _showAllTracks = false;
  bool _showAllAlbums = false;
  bool _showAllSimilarArtists = false;

  @override
  void initState() {
    super.initState();

    // Determine what to load based on media type
    if (widget.mediaType == MediaType.steam) {
      _loadSteamGameDetails();
    } else if (widget.mediaType == MediaType.tmdb) {
      _loadTmdbDetails();
    } else if (widget.mediaType == MediaType.lastfm) {
      _loadLastFmDetails();
    } else if (widget.mediaType == MediaType.lastfmAlbum) {
      _loadLastFmAlbumDetails();
    }
  }

  // Load Steam-specific app details
  Future<void> _loadSteamGameDetails() async {
    final details = await fetchSteamAppDetails(widget.appId);
    if (details != null) {
      setState(() {
        _extraDetails = [
          'Genres: ${_formatGenres(details['genres'])}',
          'Developer: ${details['developers']?.join(', ')}',
          'Release Date: ${details['release_date']['date']}',
          'Description: ${details['short_description']}',
        ];
        _imageUrl = details['header_image'];
      });
    }
  }

  // Load TMDB movie/TV show details
  void _loadTmdbDetails() {
    final genres =
        widget.genreIds
            ?.map((id) => tmdbGenreMap[id] ?? 'Unknown')
            .join(', ') ??
        'Unknown';

    setState(() {
      _extraDetails = [
        'Genres: $genres',
        'Release Date: ${widget.releaseDate ?? 'Unknown'}',
        'Description: ${widget.subtitle}',
      ];
      _imageUrl = null;  // TMDB uses posterPath instead
    });
  }

  // Load Last.fm artist information (top tracks, albums, similar artists)
  Future<void> _loadLastFmDetails() async {
    try {
      final topTracks = await fetchArtistTopTracks(widget.title);
      final topAlbums = await fetchArtistTopAlbums(widget.title);
      final similarArtists = await fetchSimilarArtists(widget.title);

      setState(() {
        _topTracks = topTracks;
        _topAlbums = topAlbums;
        _similarArtists = similarArtists;
      });
    } catch (e) {
      print('Failed to load extra last.fm details: $e');
    }
  }

  // Load Last.fm album details (tracklist, album image, durations)
  Future<void> _loadLastFmAlbumDetails() async {
    try {
      final albumDetails = await fetchFullAlbumDetails(
        widget.artistName ?? '',
        widget.title,
      );

      // Load tracks, calculate total album duration
      final tracks = albumDetails['tracks'] as List<Map<String, dynamic>>;
      final totalSeconds = tracks.fold<int>(
        0,
        (sum, track) => sum + ((track['duration'] ?? 0) as int),
      );
      final totalMinutes = (totalSeconds / 60).round();

      setState(() {
        _extraDetails = [
          'Total Album Length: $totalMinutes minutes',
          'Tracks: ${tracks.length}',
        ];
        _topTracks = tracks;
        _imageUrl = albumDetails['albumImageUrl'];
      });
    } catch (e) {
      print('Failed to load Last.fm album details: $e');
    }
  }

  // Resolve the correct image URL based on media type
  String _resolveImageUrl() {
    if (widget.mediaType == MediaType.lastfm) {
      return ''; // No image for Last.fm artists
    }

    if (_imageUrl != null) return _imageUrl!;

    if (widget.mediaType == MediaType.tmdb && widget.posterPath != null) {
      return 'https://image.tmdb.org/t/p/w500${widget.posterPath}';
    }

    return 'https://via.placeholder.com/300x400?text=No+Image';
  }

  // Format genres for TMDB movies and tv shows
  String _formatGenres(List<dynamic>? genres) {
    return genres?.map((g) => g['description']).join(', ') ?? 'Unknown';
  }

  // Format number with thousands separator
  String formatNumber(int number) {
    final formatter = NumberFormat.decimalPattern();
    return formatter.format(number);
  }

  @override
  Widget build(BuildContext context) {
    final imageUrl = _resolveImageUrl();
    return Scaffold(
      appBar: AppBar(title: Text(widget.title)),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            if (imageUrl.isNotEmpty)
              Center(
                child: Image.network(
                  imageUrl,
                  fit: BoxFit.cover,
                  height:
                      widget.mediaType == MediaType.steam
                          ? 200
                          : widget.mediaType == MediaType.tmdb
                          ? 500
                          : widget.mediaType == MediaType.lastfmAlbum
                          ? 300 // Album Images
                          : 400, // Default fallback height
                ),
              ),

            const SizedBox(height: 16),

            // Show extra details
            ..._extraDetails.map(
              (detail) => Padding(
                padding: const EdgeInsets.only(bottom: 8.0),
                child: Text(
                  detail,
                  style: const TextStyle(fontSize: 14, color: Colors.grey),
                ),
              ),
            ),

            // Last.fm Artist Detail Screen
            if (widget.mediaType == MediaType.lastfm) ...[
              if (_topTracks.isNotEmpty) _buildSectionTitle('Top Tracks'),
              ...(_showAllTracks ? _topTracks.take(10) : _topTracks.take(5)).map(
                (track) => ListTile(
                  title: Text(track['name']),
                  subtitle: Text(
                    'Playcount: ${formatNumber(int.parse(track['playcount']))}',
                  ),
                ),
              ),
              if (_topTracks.length > 5)
                TextButton(
                  onPressed: () {
                    setState(() {
                      _showAllTracks = !_showAllTracks;
                    });
                  },
                  child: Text(_showAllTracks ? 'Show Less' : 'Show More'),
                ),

              const SizedBox(height: 24),
              if (_topAlbums.isNotEmpty) _buildSectionTitle('Top Albums'),
              ...(_showAllAlbums ? _topAlbums.take(10) : _topAlbums.take(5))
                  .map(
                    (album) => ListTile(
                      leading:
                          album['imageUrl'] != null
                              ? Image.network(
                                album['imageUrl'],
                                width: 50,
                                height: 50,
                                fit: BoxFit.cover,
                              )
                              : null,
                      title: Text(album['name']),
                      subtitle: Text(
                        'Playcount: ${formatNumber(album['playcount'] ?? '')}',
                        ),
                    ),
                  ),
              if (_topAlbums.length > 5)
                TextButton(
                  onPressed: () {
                    setState(() {
                      _showAllAlbums = !_showAllAlbums;
                    });
                  },
                  child: Text(_showAllAlbums ? 'Show Less' : 'Show More'),
                ),

              const SizedBox(height: 24),
              if (_similarArtists.isNotEmpty)
                _buildSectionTitle('Similar Artists'),
              ...(_showAllSimilarArtists
                      ? _similarArtists.take(10)
                      : _similarArtists.take(5))
                  .map((artist) => ListTile(title: Text(artist['name']))),
              if (_similarArtists.length > 5)
                TextButton(
                  onPressed: () {
                    setState(() {
                      _showAllSimilarArtists = !_showAllSimilarArtists;
                    });
                  },
                  child: Text(
                    _showAllSimilarArtists ? 'Show Less' : 'Show More',
                  ),
                ),
            ],
            
            // Last.fm Album Detail Screen
            if (widget.mediaType == MediaType.lastfmAlbum) ...[
                if (_topTracks.isNotEmpty) _buildSectionTitle('Tracklist'),
                ListView.separated(
                  shrinkWrap: true,
                  physics: const NeverScrollableScrollPhysics(),
                  itemCount: _topTracks.length,
                  separatorBuilder:
                      (context, index) => const Divider(height: 1),
                  itemBuilder: (context, index) {
                    final track = _topTracks[index];
                    final trackNumber = index + 1;
                    final trackName = track['name'] ?? 'Unknown';
                    final durationSeconds = track['duration'] ?? 0;

                    return ListTile(
                      leading: Text(
                        '$trackNumber.',
                        style: const TextStyle(fontWeight: FontWeight.bold),
                      ),
                      title: Text(trackName),
                      subtitle: Text(
                        'Duration: ${_formatDuration(durationSeconds)}',
                      ),
                    );
                  },
                ),
              ],
          ],
        ),
      ),
    );
  }

  // Helper to build section titles
  Widget _buildSectionTitle(String title) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8.0),
      child: Text(
        title,
        style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
      ),
    );
  }

  // Format seconds into mm:ss
  String _formatDuration(int seconds) {
    final minutes = seconds ~/ 60;
    final remainingSeconds = seconds % 60;
    final twoDigits = remainingSeconds.toString().padLeft(2, '0');
    return '$minutes:$twoDigits';
  }
}
