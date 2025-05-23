import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:media_tracker_test/models/lastfm/lastfm_top_artist.dart';
import 'package:media_tracker_test/models/steam/steam_model.dart';
import 'package:media_tracker_test/models/theme.dart';
import 'package:media_tracker_test/models/tmdb/tmdb_movie.dart';
import 'package:media_tracker_test/models/tmdb/tmdb_tv_show.dart';
import 'package:media_tracker_test/providers/auth_provider.dart';
import 'package:media_tracker_test/screens/auth/account_settings.dart';
import 'package:media_tracker_test/screens/favorites_screen.dart';
import 'package:media_tracker_test/screens/home_screen.dart';

class DrawerMenu extends ConsumerWidget {
  final String firstName;
  final Function(int) onSectionSelected;
  final List<SteamGame> steamGames;
  final List<TMDBMovie> tmdbMovies;
  final List<TMDBTvShow> tmdbTvShows;
  final List<TopArtist> topArtists;

  const DrawerMenu({
    super.key,
    required this.firstName,
    required this.onSectionSelected,
    required this.steamGames,
    required this.tmdbMovies,
    required this.tmdbTvShows,
    required this.topArtists,
  });

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final auth = ref.watch(authProvider);
    if (auth.username == null) {
      return const Drawer(child: Center(child: CircularProgressIndicator()));
    }
    return Drawer(
      backgroundColor: colorInput,
      child: LayoutBuilder(
        builder: (context, constraints) {
          return SingleChildScrollView(
            child: ConstrainedBox(
              constraints: BoxConstraints(minHeight: constraints.maxHeight),
              child: IntrinsicHeight(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    DrawerHeader(
                      decoration: BoxDecoration(color: colorPrimary),
                      margin: EdgeInsets.zero,
                      padding: EdgeInsets.zero,
                      child: SafeArea(
                        bottom: false,
                        child: Center(
                          child: Text(
                            'Hello, $firstName',
                            style: const TextStyle(
                              color: Colors.white,
                              fontSize: 24,
                              fontWeight: FontWeight.bold,
                            ),
                          ),
                        ),
                      ),
                    ),

                    _drawerItem('Link Media Services', () {
                      Navigator.pop(context);
                      Navigator.pushNamed(context, '/linkAccounts');
                    }),
                    _drawerItem('Favorite Media', () {
                      Navigator.pop(context); // Close the drawer first
                      Navigator.push(
                        context,
                        MaterialPageRoute(
                          builder:
                              (_) => FavoritesScreen(
                                steamGames: steamGames,
                                tmdbMovies: tmdbMovies,
                                tmdbTvShows: tmdbTvShows,
                                topArtists: topArtists,
                              ),
                        ),
                      );
                    }),
                    _drawerItem('Account Settings', () {
                      Navigator.pop(context);
                      Navigator.push(
                        context,
                        MaterialPageRoute(
                          builder:
                              (_) => AccountSettings(
                                initialUsername: auth.username ?? 'Guest',
                              ),
                        ),
                      );
                    }),

                    const Spacer(),

                    const Divider(color: Colors.white24),
                    ListTile(
                      title: const Text(
                        'Logout',
                        style: TextStyle(color: Colors.redAccent),
                      ),
                      onTap: () {
                        ref.read(authProvider.notifier).logout();
                        Navigator.of(context).pushReplacement(
                          MaterialPageRoute(builder: (_) => const HomeScreen()),
                        );
                      },
                    ),
                  ],
                ),
              ),
            ),
          );
        },
      ),
    );
  }

  Widget _drawerItem(String label, VoidCallback onTap) {
    return ListTile(
      title: Text(
        label,
        style: const TextStyle(color: Colors.white, fontSize: 16),
      ),
      onTap: onTap,
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
      hoverColor: Colors.white12,
      contentPadding: const EdgeInsets.symmetric(horizontal: 20),
    );
  }
}
