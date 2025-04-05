class LastFmArtist {
  final String name;
  final int playCount;
  final String url;
  final String? mbid;
  final int rank;
  final String? imageUrl;

  LastFmArtist({
    required this.name,
    required this.playCount,
    required this.url,
    required this.rank,
    this.mbid,
    this.imageUrl,
  });

  factory LastFmArtist.fromJson(Map<String, dynamic> json) {
    final imageList = json['image'] as List<dynamic>;
    final image = imageList.lastWhere(
      (img) => img['#text'] != null && img['#text'].toString().isNotEmpty,
      orElse: () => {'#text': null},
    );

    return LastFmArtist(
      name: json['name'],
      playCount: int.tryParse(json['playcount'] ?? '') ?? 0,
      url: json['url'],
      mbid: json['mbid']?.toString(),
      rank: int.tryParse(json['@attr']?['rank'] ?? '') ?? 0,
      imageUrl: image['#text'],
    );
  }
}
