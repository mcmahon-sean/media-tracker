import 'dart:convert';
import 'package:http/http.dart' as http;
import '/config/api_connections.dart';
import '../../models/steam/steam_model.dart';

Future<List<SteamGame>> fetchSteamGames() async {
  final response = await http.get(Uri.parse(ApiServices.steamOwnedGamesUrl));

  if (response.statusCode == 200) {
    final data = json.decode(response.body);
    final games = data['response']['games'] as List<dynamic>;
    return games.map((game) => SteamGame.fromJson(game)).toList();
  } else {
    throw Exception('Failed to load Steam games');
  }
}