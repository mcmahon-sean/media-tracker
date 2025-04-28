class ApiServices {
  // STEAM CREDENTIALS
  static const String steamApiKey = '5553B2F6E49998D47EB298C086A05084';
  static String steamId = '';
  static String get steamOwnedGamesUrl =>
      'https://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=$steamApiKey&steamid=$steamId&include_appinfo=1&format=json';

  // LAST.FM CREDENTIALS
  static String lastFmUsername = '';
  static const String lastFmApiKey = '1456c592b2d19dabd13468f0eee62dc9';
  static String get lastFmBaseUrl => 'https://ws.audioscrobbler.com/2.0/';

  // TMDB CREDENTIALS
  static const String tmdbApiKey = 'c42aedff935d3382bc95b0f7e8f5f7e3';
  static String tmdbSessionId = '';
  static String tmdbAccountId = '';
}
