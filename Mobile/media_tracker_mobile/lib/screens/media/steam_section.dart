import 'package:flutter/material.dart';
import 'package:media_tracker_test/screens/media/media_details_screen.dart';
import '../../models/steam/steam_model.dart';

Widget buildSteamSection(List<SteamGame> steamGames) {
  return ListView.builder(
    itemCount: steamGames.length,
    itemBuilder: (context, index) {
      final game = steamGames[index];
      return ListTile(
        title: Text(game.name),
        subtitle: Text('Played: ${game.playtimeForever} mins'),
        trailing: IconButton(
          icon: Icon(
            game.isFavorite ? Icons.star : Icons.star_border,
            color: game.isFavorite ? Colors.yellow : Colors.grey,
          ),
          onPressed: () {
            // toggle favorite logic
          },
        ),
        onTap: () {
          Navigator.push(
            context,
            MaterialPageRoute(
              builder:
                  (_) => MediaDetailsScreen(
                    appId: game.appId.toString(),
                    title: game.name,
                    subtitle:
                        'Total playtime: ${game.playtimeForever} minutes',
                  ),
            ),
          );
        },
      );
    },
  );
}
