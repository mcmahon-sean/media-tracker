class LastFmUser {
  final String name;
  final int age;
  final bool isSubscriber;
  final String realName;
  final int playCount;
  final int artistCount;
  final int albumCount;
  final int trackCount;
  final String country;
  final String gender;
  final String profileUrl;
  final DateTime registeredAt;
  final String? imageUrl; // From the largest available image

  LastFmUser({
    required this.name,
    required this.age,
    required this.isSubscriber,
    required this.realName,
    required this.playCount,
    required this.artistCount,
    required this.albumCount,
    required this.trackCount,
    required this.country,
    required this.gender,
    required this.profileUrl,
    required this.registeredAt,
    this.imageUrl,
  });

  factory LastFmUser.fromJson(Map<String, dynamic> json) {
    final user = json['user'];
    final imageList = user['image'] as List<dynamic>;
    final largestImage = imageList.lastWhere(
      (img) => img['#text'] != null && img['#text'].isNotEmpty,
      orElse: () => {'#text': null},
    );

    return LastFmUser(
      name: user['name'],
      age: int.tryParse(user['age'] ?? '') ?? 0,
      isSubscriber: user['subscriber'] == '1',
      realName: user['realname'] ?? '',
      playCount: int.tryParse(user['playcount'] ?? '') ?? 0,
      artistCount: int.tryParse(user['artist_count'] ?? '') ?? 0,
      albumCount: int.tryParse(user['album_count'] ?? '') ?? 0,
      trackCount: int.tryParse(user['track_count'] ?? '') ?? 0,
      country: user['country'] ?? '',
      gender: user['gender'] ?? '',
      profileUrl: user['url'] ?? '',
      registeredAt: DateTime.fromMillisecondsSinceEpoch(
        int.parse(user['registered']['unixtime']) * 1000,
      ),
      imageUrl: largestImage['#text'],
    );
  }
}
