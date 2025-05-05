import 'dart:convert';
import 'package:http/http.dart' as http;
import '../../models/tmdb/tmdb_account.dart';
import '../../models/tmdb/tmdb_movie.dart';
import '../../models/tmdb/tmdb_tv_show.dart';
import '/config/api_connections.dart';
import 'package:url_launcher/url_launcher.dart';

class TMDBService {

  // Requests a temporary request token from TMDB, used for user authentication
  static Future<String?> getRequestToken() async {
    final url = Uri.parse(
      'https://api.themoviedb.org/3/authentication/token/new?api_key=${ApiServices.tmdbApiKey}',
    );
    final response = await http.get(url);

    // If successful, return the request token string
    if (response.statusCode == 200) {
      return jsonDecode(response.body)['request_token'];
    } else {
      return null; // Token request failed
    }
  }

  // Opens the TMDB authentication URL in the external browser to let user approve access
  static Future<void> launchAuthUrl(String requestToken) async {
    final url = Uri.parse(
      'https://www.themoviedb.org/authenticate/$requestToken',
    );

    // Launch the URL in external browser — fails if no browser is available
    if (!await launchUrl(url, mode: LaunchMode.externalApplication)) {
      throw 'Could not launch $url';
    }
  }

  // Exchanges the approved request token for a permanent session ID
  static Future<String?> createSession(String requestToken) async {
    final url = Uri.parse(
      'https://api.themoviedb.org/3/authentication/session/new?api_key=${ApiServices.tmdbApiKey}',
    );

    // Store and return session ID if successful
    final response = await http.post(
      url,
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode({'request_token': requestToken}),
    );

    if (response.statusCode == 200) {
      final sessionId = jsonDecode(response.body)['session_id'];
      ApiServices.tmdbSessionId = sessionId; // store it globally
      return sessionId;
    }
    return null; // Session creation failed
  }

  // Fetches the TMDB numeric account ID using the session ID
  static Future<int?> fetchAccountId(String sessionId) async {
    final url = Uri.parse(
      'https://api.themoviedb.org/3/account?api_key=${ApiServices.tmdbApiKey}&session_id=$sessionId',
    );

    final response = await http.get(url);

    // Extract and store the account ID
    if (response.statusCode == 200) {
      final id = jsonDecode(response.body)['id'];
      ApiServices.tmdbAccountId = id;
      return id;
    }
    return null; // Failed to fetch account ID
  }

  // Fetches full account details (e.g., username, name, ID) for the currently authenticated user
  static Future<TMDBAccount> fetchAccountDetails() async {
    final url = Uri.parse(
      'https://api.themoviedb.org/3/account?api_key=${ApiServices.tmdbApiKey}&session_id=${ApiServices.tmdbSessionId}',
    );

    final response = await http.get(url);

    // Parse and return as a TMDBAccount model
    if (response.statusCode == 200) {
      final data = jsonDecode(utf8.decode(response.bodyBytes));
      ApiServices.tmdbAccountId = data['id'].toString(); // Cache the ID
      return TMDBAccount.fromJson(data);
    } else {
      throw Exception('Failed to fetch TMDB account details');
    }
  }

  // Fetches a list of movies the user has rated, sorted by most recently rated
  static Future<List<TMDBMovie>> fetchRatedMovies() async {
    final url = Uri.parse(
      'https://api.themoviedb.org/3/account/${ApiServices.tmdbAccountId}/rated/movies'
      '?api_key=${ApiServices.tmdbApiKey}&session_id=${ApiServices.tmdbSessionId}'
      '&language=en-US&page=1&sort_by=created_at.desc',
    );

    final response = await http.get(url);

    // Parse and return as a list of TMDBMovie objects
    if (response.statusCode == 200) {
      final data = jsonDecode(utf8.decode(response.bodyBytes));
      final results = data['results'] as List<dynamic>;
      return results.map((json) => TMDBMovie.fromJson(json)).toList();
    } else {
      throw Exception('Failed to fetch rated movies');
    }
  }

  // Fetches the user's favorite TV shows, sorted by the order they were favorited
  static Future<List<TMDBTvShow>> fetchRatedTvShows() async {
    final url = Uri.parse(
      'https://api.themoviedb.org/3/account/${ApiServices.tmdbAccountId}/rated/tv'
      '?api_key=${ApiServices.tmdbApiKey}&session_id=${ApiServices.tmdbSessionId}'
      '&language=en-US&page=1&sort_by=created_at.asc',
    );

    final response = await http.get(url);

    // Parse and return as a list of TMDBTvShow objects
    if (response.statusCode == 200) {
      final data = jsonDecode(utf8.decode(response.bodyBytes));
      final results = data['results'] as List<dynamic>;
      return results.map((json) => TMDBTvShow.fromJson(json)).toList();
    } else {
      throw Exception('Failed to fetch favorite TV shows');
    }
  }
}
