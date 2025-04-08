import 'dart:convert';
import 'package:http/http.dart' as http;
import '../../models/tmdb/tmdb_account.dart';
import '../../models/tmdb/tmdb_movie.dart';
import '../../models/tmdb/tmdb_tv_show.dart';
import '/config/api_connections.dart';

class TMDBService {
  static Map<String, String> _headers(String authToken) => {
    'Authorization': 'Bearer $authToken',
    'accept': 'application/json',
  };

  static Future<TMDBAccount> fetchAccountDetails() async {
    final url = Uri.parse(
      'https://api.themoviedb.org/3/account/${ApiServices.tmdbUser}',
    );

    final response = await http.get(
      url,
      headers: _headers(ApiServices.tmdbAuthToken),
    );
    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);
      return TMDBAccount.fromJson(data);
    } else {
      throw Exception('Failed to fetch TMDB account details');
    }
  }

  static Future<List<TMDBMovie>> fetchRatedMovies() async {
    final url = Uri.parse(
      'https://api.themoviedb.org/3/account/${ApiServices.tmdbUser}/rated/movies?language=en-US&page=1&sort_by=created_at.desc',
    );

    final response = await http.get(
      url,
      headers: _headers(ApiServices.tmdbAuthToken),
    );
    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);
      final results = data['results'] as List<dynamic>;
      return results.map((json) => TMDBMovie.fromJson(json)).toList();
    } else {
      throw Exception('Failed to fetch rated movies');
    }
  }

  static Future<List<TMDBTvShow>> fetchFavoriteTvShows() async {
    final url = Uri.parse(
      'https://api.themoviedb.org/3/account/${ApiServices.tmdbUser}/favorite/tv?language=en-US&page=1&sort_by=created_at.asc',
    );

    final response = await http.get(
      url,
      headers: _headers(ApiServices.tmdbAuthToken),
    );
    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);
      final results = data['results'] as List<dynamic>;
      return results.map((json) => TMDBTvShow.fromJson(json)).toList();
    } else {
      throw Exception('Failed to fetch favorite TV shows');
    }
  }
}
