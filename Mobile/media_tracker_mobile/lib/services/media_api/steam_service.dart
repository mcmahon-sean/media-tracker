import 'dart:convert';
import 'package:http/http.dart' as http;
import '/config/api_connections.dart';
import '../../models/steam/steam_model.dart';

Future<List<SteamGame>> fetchSteamGames() async {
  final response = await http.get(Uri.parse(ApiServices.steamOwnedGamesUrl));

  if (response.statusCode == 200) {
    final data = json.decode(response.body);
    final gameList = data['response']['games'];

    if (gameList == null) {
      print('No games returned - check Steam ID.');
      return [];
    }

    return (gameList as List<dynamic>)
        .map((game) => SteamGame.fromJson(game))
        .toList();
  } else {
    throw Exception('Failed to load Steam games');
  }
}
