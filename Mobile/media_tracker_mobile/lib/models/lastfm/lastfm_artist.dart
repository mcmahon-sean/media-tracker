class LastFmArtist {
  final String name;
  final String bio;
  final String imageUrl;
  final int playCount;
  final int listenerCount;
  final List<String> tags;
  final String url;

  LastFmArtist({
    required this.name,
    required this.bio,
    required this.imageUrl,
    required this.playCount,
    required this.listenerCount,
    required this.tags,
    required this.url,
  });

  factory LastFmArtist.fromJson(Map<String, dynamic> json) {
    final artist = json['artist'];

    return LastFmArtist(
      name: artist['name'],
      bio: artist['bio']['summary'],
      imageUrl: artist['image'].last['#text'],
      playCount: int.tryParse(artist['stats']['playcount']) ?? 0,
      listenerCount: int.tryParse(artist['stats']['listeners']) ?? 0,
      tags: (artist['tags']['tag'] as List)
          .map((tag) => tag['name'].toString())
          .toList(),
      url: artist['url'],
    );
  }
}
