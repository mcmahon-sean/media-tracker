import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:media_tracker_test/models/lastfm/lastfm_top_artist.dart';
import '../../models/lastfm/lastfm_user.dart';
import '../../models/lastfm/lastfm_artist.dart';
import '../../models/lastfm/lastfm_track.dart';
import '/config/api_connections.dart';

// User information
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
      '&limit=10'
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

Future<List<LastFmTrack>> fetchLastFmRecentTracks() async {
  final url =
      '${ApiServices.lastFmBaseUrl}?method=user.getrecenttracks'
      '&user=${ApiServices.lastFmUsername}'
      '&api_key=${ApiServices.lastFmApiKey}'
      '&limit=10'
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

// Artist information
Future<LastFmArtist> fetchArtistDetail(String artistName) async {
  final url =
      'http://ws.audioscrobbler.com/2.0/?method=artist.getinfo'
      '&artist=${Uri.encodeComponent(artistName)}'
      '&api_key=${ApiServices.lastFmApiKey}'
      '&format=json';

  final response = await http.get(Uri.parse(url));

  if (response.statusCode == 200) {
    final data = jsonDecode(response.body);
    print(jsonEncode(data));
    return LastFmArtist.fromJson(data);
  } else {
    throw Exception('Failed to fetch artist\'s detail.');
  }
}

// Artist top tracks
Future<List<Map<String, dynamic>>> fetchArtistTopTracks(
  String artistName,
) async {
  final url =
      'http://ws.audioscrobbler.com/2.0/?method=artist.gettoptracks'
      '&artist=${Uri.encodeComponent(artistName)}'
      '&api_key=${ApiServices.lastFmApiKey}'
      '&format=json';

  final response = await http.get(Uri.parse(url));

  if (response.statusCode == 200) {
    final data = jsonDecode(response.body);
    print(jsonEncode(data)); // DEBUGGING

    final tracks =
        (data['toptracks']['track'] as List)
            .map(
              (track) => {
                'name': track['name'],
                'playcount': track['playcount'],
                'url': track['url'],
              },
            )
            .toList();

    return tracks;
  } else {
    throw Exception('Failed to fetch artist\'s top tracks.');
  }
}

// Artist top albums
Future<List<Map<String, dynamic>>> fetchArtistTopAlbums(
  String artistName,
) async {
  final url =
      'http://ws.audioscrobbler.com/2.0/?method=artist.gettopalbums'
      '&artist=${Uri.encodeComponent(artistName)}'
      '&api_key=${ApiServices.lastFmApiKey}'
      '&format=json';

  final response = await http.get(Uri.parse(url));

  if (response.statusCode == 200) {
    final data = jsonDecode(response.body);
    print(jsonEncode(data)); // DEBUGGING

    final albums =
        (data['topalbums']['album'] as List)
            .map(
              (album) => {
                'name': album['name'],
                'playcount': album['playcount'],
                'imageUrl':
                    album['image'] != null && album['image'].length > 2
                        ? album['image'][2]['#text']
                        : null,
              },
            )
            .toList();

    return albums;
  } else {
    throw Exception('Failed to fetch artist\'s top albums.');
  }
}

// Similar artists
Future<List<Map<String, dynamic>>> fetchSimilarArtists(
  String artistName,
) async {
  final url =
      'http://ws.audioscrobbler.com/2.0/?method=artist.getsimilar'
      '&artist=${Uri.encodeComponent(artistName)}'
      '&api_key=${ApiServices.lastFmApiKey}'
      '&format=json';

  final response = await http.get(Uri.parse(url));

  if (response.statusCode == 200) {
    final data = jsonDecode(response.body);
    print(jsonEncode(data)); // DEBUGGING

    final artists =
        (data['similarartists']['artist'] as List)
            .map(
              (artist) => {
                'name': artist['name'],
                'match': artist['match'],
                'imageUrl':
                    artist['image'] != null && artist['image'].length > 2
                        ? artist['image'][2]['#text']
                        : null,
              },
            )
            .toList();

    return artists;
  } else {
    throw Exception('Failed to fetch similar artists.');
  }
}

// Fetch album details
Future<Map<String, dynamic>> fetchAlbumDetail(
  String artistName,
  String trackName,
) async {
  final url =
      'http://ws.audioscrobbler.com/2.0/?method=track.getInfo'
      '&artist=${Uri.encodeComponent(artistName)}'
      '&track=${Uri.encodeComponent(trackName)}'
      '&api_key=${ApiServices.lastFmApiKey}'
      '&format=json';

  final response = await http.get(Uri.parse(url));

  if (response.statusCode == 200) {
    final data = jsonDecode(response.body);
    print(jsonEncode(data)); // DEBUGGING

    final albumData = data['track']?['album'];
    if (albumData == null) throw Exception('Album info not found for track.');

    return {
      'albumName': albumData['title'],
      'albumImageUrl':
          albumData['image'] != null && albumData['image'].length > 2
              ? albumData['image'][2]['#text'] // medium size
              : null,
    };
  } else {
    throw Exception('Failed to fetch track/album info.');
  }
}

// Fetch full album details from the album name
Future<Map<String, dynamic>> fetchFullAlbumDetails(String artistName, String albumName) async {
  final url =
      'http://ws.audioscrobbler.com/2.0/?method=album.getinfo'
      '&artist=${Uri.encodeComponent(artistName)}'
      '&album=${Uri.encodeComponent(albumName)}'
      '&api_key=${ApiServices.lastFmApiKey}'
      '&format=json';

  final response = await http.get(Uri.parse(url));

  if (response.statusCode == 200) {
    final data = jsonDecode(response.body);
    final album = data['album'];

    final rawTracks = album['tracks']['track'];

    List<Map<String, dynamic>> tracks = [];

    if (rawTracks is List) {
      // Normal multiple tracks
      tracks = rawTracks.map<Map<String, dynamic>>((track) {
        return {
          'name': track['name'],
          'duration': int.tryParse('${track['duration'] ?? '0'}') ?? 0,
          'playcount': '${track['playcount'] ?? 'Unknown'}',
        };
      }).toList();
    } else if (rawTracks is Map) {
      // Only one track (special case)
      tracks = [
        {
          'name': rawTracks['name'],
          'duration': int.tryParse('${rawTracks['duration'] ?? '0'}') ?? 0,
          'playcount': '${rawTracks['playcount'] ?? 'Unknown'}',
        }
      ];
    }

    return {
      'albumName': album['name'],
      'albumImageUrl': album['image'] != null && album['image'].length > 2
          ? album['image'][2]['#text']
          : null,
      'tracks': tracks,
    };
  } else {
    throw Exception('Failed to fetch full album info.');
  }
}