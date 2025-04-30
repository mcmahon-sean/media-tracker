class LastFmTrack {
  final String name;
  final String artist;
  final String album;
  final String url;
  final int? timestamp;
  final String? imageUrl;

  LastFmTrack({
    required this.name,
    required this.artist,
    required this.album,
    required this.url,
    this.timestamp,
    this.imageUrl,
  });

  factory LastFmTrack.fromJson(Map<String, dynamic> json) {
    // Safely get the largest non-empty image URL
    final imageList = json['image'] as List<dynamic>;
    final image = imageList.lastWhere(
      (img) => img['#text'] != null && img['#text'].toString().isNotEmpty,
      orElse: () => {'#text': null},
    );

    return LastFmTrack(
      name: json['name'] ?? '',
      artist: json['artist']['#text'] ?? '',
      album: json['album']['#text'] ?? '',
      url: json['url'] ?? '',
      timestamp:
          json['date'] != null
              ? int.tryParse(json['date']['uts'] ?? '')
              : null, // null if currently playing
      imageUrl: image['#text'],
    );
  }

  // formatted date string for display (MM/DD/YYYY)
  String? get formattedDate {
    if (timestamp == null) return null;
    final dt = DateTime.fromMillisecondsSinceEpoch(timestamp! * 1000);
    return '${dt.month.toString().padLeft(2, '0')}/'
        '${dt.day.toString().padLeft(2, '0')}/'
        '${dt.year}';
  }
}
