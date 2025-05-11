import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:media_tracker_test/models/sort_options.dart';
import 'package:media_tracker_test/providers/auth_provider.dart';
import 'package:media_tracker_test/providers/favorites_provider.dart';
import 'package:media_tracker_test/screens/media/media_details_screen.dart';
import 'package:media_tracker_test/services/user_account_services.dart';
import '../../models/steam/steam_model.dart';

class SteamSection extends ConsumerStatefulWidget {
  final List<SteamGame> steamGames;
  final SortOption sortOption;
  final SortDirection sortDirection;

  const SteamSection({
    super.key,
    required this.steamGames,
    required this.sortOption,
    required this.sortDirection,
  });

  @override
  ConsumerState<SteamSection> createState() => _SteamSectionState();
}

class _SteamSectionState extends ConsumerState<SteamSection> {
  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    final auth = ref.watch(authProvider);
    final favorites = ref.watch(favoritesProvider);
    //print('Favorites: $favorites'); // DEBUGGING

    final displayGames =
        widget.steamGames.map((game) {
          return game.copyWith(
            isFavorite: favorites.any(
              (fav) =>
                  fav['media']['platform_id'] == 1 &&
                  fav['media']['media_plat_id'] == game.appId.toString() &&
                  fav['media']['title'] == game.name &&
                  fav['favorites'] == true,
            ),
          );
        }).toList();

    final sortedGames = applySorting<SteamGame>(
      list: displayGames,
      option: widget.sortOption,
      direction: widget.sortDirection,
      getField: (game) {
        if (widget.sortOption == SortOption.name) {
          return game.name.toLowerCase();
        } else if (widget.sortOption == SortOption.playtime) {
          return game.playtimeForever;
        }
        return game.name.toLowerCase(); // fallback
      },
      isFavorite: (game) => game.isFavorite,
    );

    return ListView.builder(
      itemCount: sortedGames.length,
      itemBuilder: (context, index) {
        final game = sortedGames[index];
        return ListTile(
          title: Text(game.name),
          subtitle: Text('Played: ${formatPlaytime(game.playtimeForever)}'),
          trailing: IconButton(
            icon: Icon(
              game.isFavorite ? Icons.star : Icons.star_border,
              color: game.isFavorite ? Colors.yellow : Colors.grey,
            ),
            onPressed: () async {
              final success = await UserAccountServices().toggleFavoriteMedia(
                platformId: 1, // Steam
                mediaTypeId: 1, // Game
                mediaPlatId: game.appId.toString(),
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

  // Helper method to convert playtime to something more readable
  String formatPlaytime(int minutes) {
    final hours = minutes ~/ 60;
    final remainingMinutes = minutes % 60;

    if (hours > 0 && remainingMinutes > 0) {
      return '$hours hrs $remainingMinutes min';
    } else if (hours > 0) {
      return '$hours hrs';
    } else {
      return '$remainingMinutes min';
    }
  }
}
