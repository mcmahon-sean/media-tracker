class SteamGame {
  final int appId;
  final String name;
  final int playtimeForever;
  final String imgIconUrl;
  final bool hasCommunityStats;
  final int playtimeWindows;
  final int playtimeMac;
  final int playtimeLinux;
  final int playtimeDeck;
  final int rtimeLastPlayed;
  final int playtimeDisconnected;
  bool isFavorite;

  SteamGame({
    required this.appId,
    required this.name,
    required this.playtimeForever,
    required this.imgIconUrl,
    required this.hasCommunityStats,
    required this.playtimeWindows,
    required this.playtimeMac,
    required this.playtimeLinux,
    required this.playtimeDeck,
    required this.rtimeLastPlayed,
    required this.playtimeDisconnected,
    this.isFavorite = false,
  });

  factory SteamGame.fromJson(Map<String, dynamic> json) {
    return SteamGame(
      appId: json['appid'],
      name: json['name'],
      playtimeForever: json['playtime_forever'],
      imgIconUrl: json['img_icon_url'],
      hasCommunityStats: json['has_community_visible_stats'] ?? false,
      playtimeWindows: json['playtime_windows_forever'] ?? 0,
      playtimeMac: json['playtime_mac_forever'] ?? 0,
      playtimeLinux: json['playtime_linux_forever'] ?? 0,
      playtimeDeck: json['playtime_deck_forever'] ?? 0,
      rtimeLastPlayed: json['rtime_last_played'] ?? 0,
      playtimeDisconnected: json['playtime_disconnected'] ?? 0,
      isFavorite: false,
    );
  }

  SteamGame copyWith({
    int? appId,
    String? name,
    int? playtimeForever,
    String? imgIconUrl,
    bool? hasCommunityStats,
    int? playtimeWindows,
    int? playtimeMac,
    int? playtimeLinux,
    int? playtimeDeck,
    int? rtimeLastPlayed,
    int? playtimeDisconnected,
    bool? isFavorite,
  }) {
    return SteamGame(
      appId: appId ?? this.appId,
      name: name ?? this.name,
      playtimeForever: playtimeForever ?? this.playtimeForever,
      imgIconUrl: imgIconUrl ?? this.imgIconUrl,
      hasCommunityStats: hasCommunityStats ?? this.hasCommunityStats,
      playtimeWindows: playtimeWindows ?? this.playtimeWindows,
      playtimeMac: playtimeMac ?? this.playtimeMac,
      playtimeLinux: playtimeLinux ?? this.playtimeLinux,
      playtimeDeck: playtimeDeck ?? this.playtimeDeck,
      rtimeLastPlayed: rtimeLastPlayed ?? this.rtimeLastPlayed,
      playtimeDisconnected: playtimeDisconnected ?? this.playtimeDisconnected,
      isFavorite: isFavorite ?? this.isFavorite,
    );
  }
}
