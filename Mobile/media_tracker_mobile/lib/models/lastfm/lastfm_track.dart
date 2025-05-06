class LastFmTrack {
  final String name;
  final String artist;
  final String album;
  final String url;
  final int? timestamp;
  final String? imageUrl;
  final int? playCount;

  LastFmTrack({
    required this.name,
    required this.artist,
    required this.album,
    required this.url,
    this.timestamp,
    this.imageUrl,
    this.playCount,
  });

  factory LastFmTrack.fromJson(Map<String, dynamic> json) {
    String artistName;
    if (json['artist'] is String) {
      artistName = json['artist'];
    } else if (json['artist'] is Map && json['artist']['name'] != null) {
      artistName = json['artist']['name'];
    } else if (json['artist'] is Map && json['artist']['#text'] != null) {
      artistName = json['artist']['#text'];
    } else {
      artistName = 'Unknown';
    }

    String albumName = '';
    if (json['album'] is Map) {
      albumName = json['album']['#text'] ?? json['album']['title'] ?? '';
    }

    String? imageUrl;
    if (json['image'] is List) {
      final imageList = json['image'] as List<dynamic>;
      final image = imageList.lastWhere(
        (img) => img['#text'] != null && img['#text'].toString().isNotEmpty,
        orElse: () => {'#text': null},
      );
      imageUrl = image['#text'];
    }

    int? timestamp;
    if (json['date'] != null && json['date']['uts'] != null) {
      timestamp = int.tryParse(json['date']['uts']);
    }

    return LastFmTrack(
      name: json['name'] ?? '',
      artist: artistName,
      album: albumName,
      url: json['url'] ?? '',
      timestamp: timestamp,
      imageUrl: imageUrl,
      playCount: int.tryParse('${json['playcount'] ?? '0'}'),
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
