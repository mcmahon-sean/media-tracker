import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:media_tracker_test/providers/favorites_provider.dart';
import 'package:media_tracker_test/services/auth_service.dart';
import 'package:media_tracker_test/services/media_api/tmdb_service.dart';
import 'package:media_tracker_test/services/user_account_services.dart';
import '../providers/auth_provider.dart';
import 'widgets/link_account_card.dart';

class AccountLinkingScreen extends ConsumerWidget {
  const AccountLinkingScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final auth = ref.watch(authProvider);
    final notifier = ref.read(authProvider.notifier);

    return Scaffold(
      appBar: AppBar(title: const Text('Link Media Accounts')),
      body: SingleChildScrollView(
        padding: const EdgeInsets.symmetric(vertical: 16),
        child: Column(
          children: [
            LinkAccountCard(
              platformName: 'Steam',
              inputLabel: 'Enter Vanity URL or Steam ID',
              linkedValue: auth.steamId,
              onLink: (input) async {
                // Try to resolve Vanity URL to Steam ID
                final steamId = await UserAccountServices()
                    .fetchSteamIDFromVanity(input);

                // Save the resolved Steam ID
                final success = await UserAccountServices()
                    .savePlatformCredentials(
                      username: auth.username!,
                      platformId: 1,
                      userPlatformId: steamId,
                    );

                if (success) {
                  // Re-fetch the saved value from the DB
                  final authService = ref.read(authServiceProvider);
                  final updatedSteamId = await authService.getPlatformID(
                    auth.username!,
                    1,
                  );

                  // Update state only after confirming the value from DB
                  if (updatedSteamId != null) {
                    notifier.updateSteamId(updatedSteamId);
                    Navigator.pop(context, true);
                  }
                } else {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(
                      content: Text('Failed to link Steam account.'),
                    ),
                  );
                }
              },
              onUnlink: () async {
                final success = await UserAccountServices()
                    .removePlatformCredentials(
                      username: auth.username!,
                      platformId: 1,
                      userPlatformId: auth.steamId!,
                    );

                if (success) {
                  notifier.updateSteamId(null);
                  ref.read(favoritesProvider.notifier).state =
                      ref
                          .read(favoritesProvider)
                          .where((fav) => fav['media']['platform_id'] != 1)
                          .toList();
                } else {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(
                      content: Text('Failed to unlink Steam account.'),
                    ),
                  );
                }
              },
              onEdit: () {},
            ),
            LinkAccountCard(
              platformName: 'Last.fm',
              linkedValue: auth.lastFmUsername,
              onLink: (username) async {
                final success = await UserAccountServices()
                    .savePlatformCredentials(
                      username: auth.username!,
                      platformId: 2,
                      userPlatformId: username,
                    );

                if (success) {
                  // Re-fetch the saved value from the DB
                  final authService = ref.read(authServiceProvider);
                  final updatedLastfmUsername = await authService.getPlatformID(
                    auth.username!,
                    2,
                  );

                  // Update state only after confirming the value from DB
                  if (updatedLastfmUsername != null) {
                    notifier.updateLastFmUsername(updatedLastfmUsername);
                    Navigator.pop(context, true);
                  }
                } else {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(
                      content: Text('Failed to link Last.fm account'),
                    ),
                  );
                }
              },
              onUnlink: () async {
                final success = await UserAccountServices()
                    .removePlatformCredentials(
                      username: auth.username!,
                      platformId: 2,
                      userPlatformId: auth.lastFmUsername!,
                    );

                if (success) {
                  notifier.updateLastFmUsername(null);
                  ref.read(favoritesProvider.notifier).state =
                      ref
                          .read(favoritesProvider)
                          .where((fav) => fav['media']['platform_id'] != 2)
                          .toList();
                } else {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(
                      content: Text('Failed to unlink Last.FM account.'),
                    ),
                  );
                }
              },
              onEdit: () {},
            ),
            LinkAccountCard(
              platformName: 'TMDB',
              inputLabel: 'Enter TMDB Username',
              linkedValue: auth.tmdbSessionId,
              onLink: (_) async {
                final requestToken = await TMDBService.getRequestToken();
                if (requestToken == null) {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(
                      content: Text('Failed to generate TMDB request token.'),
                    ),
                  );
                  return;
                }

                // Launch TMDB authorization in browser
                await TMDBService.launchAuthUrl(requestToken);

                // Wait for user to approve and come back
                final approved = await showDialog(
                  context: context,
                  builder:
                      (context) => AlertDialog(
                        title: const Text('Continue?'),
                        content: const Text(
                          'Once you approve access on TMDB, press continue to finish linking.',
                        ),
                        actions: [
                          TextButton(
                            onPressed: () => Navigator.of(context).pop(true),
                            child: const Text('Continue'),
                          ),
                          TextButton(
                            onPressed: () => Navigator.of(context).pop(false),
                            child: const Text('Cancel'),
                          ),
                        ],
                      ),
                );

                if (approved != true) return;

                // Create session after user authorizes
                final sessionId = await TMDBService.createSession(requestToken);
                if (sessionId == null) {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(
                      content: Text('Failed to create TMDB session'),
                    ),
                  );
                  return;
                }
                // Save sessionId to DB via your service
                final success = await UserAccountServices()
                    .savePlatformCredentials(
                      username: auth.username!,
                      platformId: 3,
                      userPlatformId: sessionId,
                    );

                if (success) {
                  final authService = ref.read(authServiceProvider);
                  final updatedTmdbSessionId = await authService.getPlatformID(
                    auth.username!,
                    3,
                  );

                  if (updatedTmdbSessionId != null) {
                    notifier.updateTmdbSessionId(updatedTmdbSessionId);
                    Navigator.pop(context, true);
                  }
                } else {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(
                      content: Text('Failed to link TMDB account'),
                    ),
                  );
                }
              },
              onUnlink: () async {
                final success = await UserAccountServices()
                    .removePlatformCredentials(
                      username: auth.username!,
                      platformId: 3,
                      userPlatformId: auth.tmdbSessionId!,
                    );

                if (success) {
                  notifier.updateTmdbSessionId(null);
                  ref.read(favoritesProvider.notifier).state =
                      ref
                          .read(favoritesProvider)
                          .where((fav) => fav['media']['platform_id'] != 3)
                          .toList();
                } else {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(
                      content: Text('Failed to unlink TMDB account.'),
                    ),
                  );
                }
              },
              onEdit: () {},
            ),
          ],
        ),
      ),
    );
  }
}
