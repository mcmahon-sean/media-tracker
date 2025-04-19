import 'package:flutter/material.dart';
import '../../models/steam/steam_model.dart';

Widget buildSteamSection(List<SteamGame> steamGames) {
  return ListView.builder(
    itemCount: steamGames.length,
    itemBuilder: (context, index) {
      final game = steamGames[index];
      return ListTile(
        title: Text(game.name),
        subtitle: Text('Played: ${game.playtimeForever} mins'),
      );
    },
  );
}