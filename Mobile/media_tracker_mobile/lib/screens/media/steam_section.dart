import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:media_tracker_test/providers/auth_provider.dart';
import 'package:media_tracker_test/providers/favorites_provider.dart';
import 'package:media_tracker_test/screens/media/media_details_screen.dart';
import 'package:media_tracker_test/services/user_account_services.dart';
import '../../models/steam/steam_model.dart';

class SteamSection extends ConsumerStatefulWidget {
  final List<SteamGame> steamGames;

  const SteamSection({super.key, required this.steamGames});

  @override
  ConsumerState<SteamSection> createState() => _SteamSectionState();
}

class _SteamSectionState extends ConsumerState<SteamSection> {
  //late List<SteamGame> _steamGames;

  @override
  void initState() {
    super.initState();
    //_steamGames = List.from(widget.steamGames); // Make mutable local copy
  }

  @override
  Widget build(BuildContext context) {
    final auth = ref.watch(authProvider);
    final favorites = ref.watch(favoritesProvider);
    //print('Favorites: $favorites'); // DEBUGGING
    
    final displayGames = widget.steamGames.map((game) {
    return game.copyWith(
      isFavorite: favorites.any(
        (fav) =>
            fav['media']['platform_id'] == 1 &&
            fav['media']['media_plat_id'] == game.name &&
            fav['favorites'] == true,
      ),
    );
  }).toList();

    return ListView.builder(
      itemCount: displayGames.length,
      itemBuilder: (context, index) {
        final game = displayGames[index];
        return ListTile(
          title: Text(game.name),
          subtitle: Text('Played: ${game.playtimeForever} mins'),
          trailing: IconButton(
            icon: Icon(
              game.isFavorite ? Icons.star : Icons.star_border,
              color: game.isFavorite ? Colors.yellow : Colors.grey,
            ),
            onPressed: () async {
              final success = await UserAccountServices().toggleFavoriteMedia(
                platformId: 1, // Steam
                mediaTypeId: 1, // Game
                mediaPlatId: game.name,
                title: game.name,
                username: auth.username!,
              );

              if (success) {
                // Re-fetch favorites and update the provider
                final updatedFavorites = await UserAccountServices()
                    .fetchUserFavorites(auth.username!);
                ref.read(favoritesProvider.notifier).state = updatedFavorites;
              } else {
                ScaffoldMessenger.of(context).showSnackBar(
                  const SnackBar(content: Text('Failed to favorite')),
                );
              }
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
                      mediaType: MediaType.steam,
                    ),
              ),
            );
          },
        );
      },
    );
  }
}
