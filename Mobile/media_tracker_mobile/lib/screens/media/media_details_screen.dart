import 'package:flutter/material.dart';
import '../../services/media_api/steam_service.dart';

class MediaDetailsScreen extends StatefulWidget {
  final String appId; // required to fetch from Steam API
  final String title;
  final String subtitle;

  const MediaDetailsScreen({
    super.key,
    required this.appId,
    required this.title,
    required this.subtitle,
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
    _loadSteamGameDetails();
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
            if (_imageUrl != null)
              Center(
                child: Image.network(
                  _imageUrl!,
                  fit: BoxFit.cover,
                  height: 200,
                ),
              ),
            const SizedBox(height: 16),
            Text(widget.subtitle, style: const TextStyle(fontSize: 16)),
            const SizedBox(height: 12),
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
