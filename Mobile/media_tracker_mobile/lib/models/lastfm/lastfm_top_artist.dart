class TopArtist {
  final String name;
  final int playCount;
  final String url;
  final int rank;
  final String? imageUrl;
  final String? topAlbumImageUrl;
  bool isFavorite;

  TopArtist({
    required this.name,
    required this.playCount,
    required this.url,
    required this.rank,
    this.imageUrl,
    this.topAlbumImageUrl,
    this.isFavorite = false,
  });

  TopArtist copyWith({String? topAlbumImageUrl, bool? isFavorite}) {
    return TopArtist(
      name: name,
      playCount: playCount,
      url: url,
      rank: rank,
      imageUrl: imageUrl,
      topAlbumImageUrl: topAlbumImageUrl ?? this.topAlbumImageUrl,
      isFavorite: isFavorite ?? this.isFavorite,
    );
  }

  factory TopArtist.fromJson(Map<String, dynamic> json) {
    final imageList = json['image'] as List<dynamic>? ?? [];
    final image = imageList.lastWhere(
      (img) => img['#text'] != null && img['#text'].toString().isNotEmpty,
      orElse: () => {'#text': null},
    );

    return TopArtist(
      name: json['name'],
      playCount: int.tryParse(json['playcount'] ?? '') ?? 0,
      url: json['url'],
      rank: int.tryParse(json['@attr']?['rank'] ?? '') ?? 0,
      imageUrl: image['#text'],
      isFavorite: false,
    );
  }
}
