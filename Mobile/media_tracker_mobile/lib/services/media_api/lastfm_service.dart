import 'dart:convert';
import 'package:http/http.dart' as http;
import '../../models/lastfm/lastfm_user.dart';
import '../../models/lastfm/lastfm_artist.dart';
import '../../models/lastfm/lastfm_track.dart';
import '/config/api_connections.dart';

Future<LastFmUser> fetchLastFmUser() async {
  final url =
      '${ApiServices.lastFmBaseUrl}?method=user.getinfo'
      '&user=${ApiServices.lastFmUser}'
      '&api_key=${ApiServices.lastFmApiKey}'
      '&format=json';

  final response = await http.get(Uri.parse(url));
  if (response.statusCode == 200) {
    final jsonData = jsonDecode(response.body);
    return LastFmUser.fromJson(jsonData);
  } else {
    throw Exception('Failed to fetch Last.fm user info');
  }
}

Future<List<LastFmArtist>> fetchLastFmTopArtists() async {
  final url =
      '${ApiServices.lastFmBaseUrl}?method=user.gettopartists'
      '&user=${ApiServices.lastFmUser}'
      '&api_key=${ApiServices.lastFmApiKey}'
      '&limit=5'
      '&format=json';

  final response = await http.get(Uri.parse(url));
  if (response.statusCode == 200) {
    final data = jsonDecode(response.body);
    final artists = data['topartists']['artist'] as List<dynamic>;
    return artists.map((json) => LastFmArtist.fromJson(json)).toList();
  } else {
    throw Exception('Failed to fetch top artists');
  }
}

Future<List<LastFmTrack>> fetchLastFmRecentTracks() async {
  final url =
      '${ApiServices.lastFmBaseUrl}?method=user.getrecenttracks'
      '&user=${ApiServices.lastFmUser}'
      '&api_key=${ApiServices.lastFmApiKey}'
      '&limit=5'
      '&format=json';

  final response = await http.get(Uri.parse(url));
  if (response.statusCode == 200) {
    final data = jsonDecode(response.body);
    final tracks = data['recenttracks']['track'] as List<dynamic>;
    return tracks.map((json) => LastFmTrack.fromJson(json)).toList();
  } else {
    throw Exception('Failed to fetch recent tracks');
  }
}
