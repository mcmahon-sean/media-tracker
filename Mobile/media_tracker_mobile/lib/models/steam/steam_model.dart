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
    );
  }

  bool get isFavorite => false;
}
