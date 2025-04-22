import 'package:flutter/material.dart';
import '../../models/tmdb/genre_helper.dart';
import '../../services/media_api/steam_service.dart';

enum MediaType { steam, tmdb, lastfm }

class MediaDetailsScreen extends StatefulWidget {
  final String appId; // Required for Steam, optional for TMDB
  final String title;
  final String subtitle;
  final MediaType mediaType;
  final List<int>? genreIds;
  final String? releaseDate;
  final String? posterPath;

  const MediaDetailsScreen({
    super.key,
    required this.appId,
    required this.title,
    required this.subtitle,
    required this.mediaType,
    this.genreIds,
    this.releaseDate,
    this.posterPath,
  });

  @override
  State<MediaDetailsScreen> createState() => _MediaDetailsScreenState();
}

class _MediaDetailsScreenState extends State<MediaDetailsScreen> {
  String? _imageUrl;
  List<String> _extraDetails = [];

  @override
  void initState() {
    super.initState();
    if (widget.mediaType == MediaType.steam) {
      _loadSteamGameDetails();
    } else {
      _loadTmdbDetails();
    }
  }

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
      _imageUrl = null;
    });
  }

  String _resolveImageUrl() {
    if (_imageUrl != null) return _imageUrl!;

    if (widget.mediaType == MediaType.tmdb && widget.posterPath != null) {
      return 'https://image.tmdb.org/t/p/w500${widget.posterPath}';
    }

    if (widget.mediaType == MediaType.lastfm && widget.posterPath != null) {
      print(widget.posterPath!);
      return widget.posterPath!;
    }

    // Fallback image if none are provided
    return 'https://via.placeholder.com/300x400?text=No+Image';
  }

  String _formatGenres(List<dynamic>? genres) {
    return genres?.map((g) => g['description']).join(', ') ?? 'Unknown';
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text(widget.title)),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            if (_imageUrl != null || widget.posterPath != null)
              Center(
                child: Image.network(
                  _resolveImageUrl(),
                  fit: BoxFit.cover,
                  height:
                      widget.mediaType == MediaType.steam
                          ? 200
                          : widget.mediaType == MediaType.tmdb
                          ? 500
                          : 300, // last.fm
                ),
              ),

            const SizedBox(height: 16),
            ..._extraDetails.map(
              (detail) => Padding(
                padding: const EdgeInsets.only(bottom: 8.0),
                child: Text(
                  detail,
                  style: const TextStyle(fontSize: 14, color: Colors.grey),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
