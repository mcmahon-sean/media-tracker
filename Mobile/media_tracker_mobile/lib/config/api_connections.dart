class ApiServices {
  // STEAM CREDENTIALS
  static const String steamApiKey = '5553B2F6E49998D47EB298C086A05084';
  static String steamUserId = '';

  // LAST.FM CREDENTIALS
  static String lastFmUser = '';
  static const String lastFmApiKey = '1456c592b2d19dabd13468f0eee62dc9';

  // TMDB CREDENTIALS
  static String tmdbUser = '';
  static const String tmdbAuthToken =
      'eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJhOWNhY2ExNDI2MzI3YmQ5ZmE2MTI2NTRiNTk5NTIwNCIsIm5iZiI6MTczNzk5ODQ1OS40MDIsInN1YiI6IjY3OTdjMDdiMDljMjUyZTNhYjIzZGY2YyIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.zmcMBdlO7wllH_BvB1mZaqvydJC98yN7WuqhQePF004';

  static String get steamOwnedGamesUrl =>
      'http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=$steamApiKey&steamid=$steamUserId&include_appinfo=1&format=json';

  static String get lastFmBaseUrl => 'https://ws.audioscrobbler.com/2.0/';
}
