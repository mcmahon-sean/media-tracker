import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:media_tracker_test/models/lastfm/lastfm_top_artist.dart';
import '../../models/lastfm/lastfm_user.dart';
import '../../models/lastfm/lastfm_artist.dart';
import '../../models/lastfm/lastfm_track.dart';
import '/config/api_connections.dart';

Future<LastFmUser> fetchLastFmUser() async {
  final url =
      '${ApiServices.lastFmBaseUrl}?method=user.getinfo'
      '&user=${ApiServices.lastFmUsername}'
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

Future<List<TopArtist>> fetchLastFmTopArtists() async {
  final url =
      '${ApiServices.lastFmBaseUrl}?method=user.gettopartists'
      '&user=${ApiServices.lastFmUsername}'
      '&api_key=${ApiServices.lastFmApiKey}'
      '&limit=5'
      '&format=json';

  final response = await http.get(Uri.parse(url));
  if (response.statusCode == 200) {
    final data = jsonDecode(response.body);
    final artists = data['topartists']['artist'] as List<dynamic>;
    return artists.map((json) => TopArtist.fromJson(json)).toList();
  } else {
    throw Exception('Failed to fetch top artists');
  }
}

Future<LastFmArtist> fetchArtistDetail(String artistName) async {
  final url =
      'http://ws.audioscrobbler.com/2.0/?method=artist.getinfo'
      '&artist=${Uri.encodeComponent(artistName)}'
      '&api_key=${ApiServices.lastFmApiKey}'
      '&format=json';

  final response = await http.get(Uri.parse(url));

  if (response.statusCode == 200) {
    final data = jsonDecode(response.body);
    return LastFmArtist.fromJson(data);
  } else {
    throw Exception('Failed to fetch artist\'s detail.');
  }
}

Future<List<LastFmTrack>> fetchLastFmRecentTracks() async {
  final url =
      '${ApiServices.lastFmBaseUrl}?method=user.getrecenttracks'
      '&user=${ApiServices.lastFmUsername}'
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
