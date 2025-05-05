class LastFmAlbum {
  final String name;
  final String artist;
  final String? imageUrl;
  final int? playCount;

  LastFmAlbum({
    required this.name,
    required this.artist,
    this.imageUrl,
    this.playCount,
  });

  factory LastFmAlbum.fromJson(Map<String, dynamic> json) {
    final imageList = json['image'] as List<dynamic>? ?? [];
    final image = imageList.lastWhere(
      (img) => img['#text'] != null && (img['#text'] as String).isNotEmpty,
      orElse: () => {'#text': null},
    );

    return LastFmAlbum(
      name: json['name'] ?? '',
      artist: json['artist']?['name'] ?? '',
      imageUrl: image['#text'],
      playCount: int.tryParse('${json['playcount'] ?? ''}'),
    );
  }
}
