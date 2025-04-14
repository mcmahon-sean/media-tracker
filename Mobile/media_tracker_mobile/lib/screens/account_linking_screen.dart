import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:media_tracker_test/services/auth_service.dart';
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
              linkedValue: auth.steamId,
              onLink: (id) async {
                final success = await UserAccountServices()
                    .savePlatformCredentials(
                      username: auth.username!,
                      platformId: 1,
                      userPlatformId: id,
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
                  }
                } else {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(
                      content: Text('Failed to link Steam account.'),
                    ),
                  );
                }
              },
              onUnlink: () => notifier.updateSteamId(null),
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
                  }
                } else {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(
                      content: Text('Failed to link Last.fm account'),
                    ),
                  );
                }
              },
              onUnlink: () => notifier.updateLastFmUsername(null),
              onEdit: () {},
            ),
            LinkAccountCard(
              platformName: 'TMDB',
              linkedValue: auth.tmdbSessionId,
              onLink: (sessionId) async {
                final success = await UserAccountServices()
                    .savePlatformCredentials(
                      username: auth.username!,
                      platformId: 3,
                      userPlatformId: sessionId,
                    );

                if (success) {
                  // Re-fetch the saved value from the DB
                  final authService = ref.read(authServiceProvider);
                  final updatedTmdbSessionId = await authService.getPlatformID(
                    auth.username!,
                    3,
                  );

                  // Update state only after confirming the value from DB
                  if (updatedTmdbSessionId != null) {
                    notifier.updateTmdbSessionId(updatedTmdbSessionId);
                  }
                } else {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(
                      content: Text('Failed to link TMDB account'),
                    ),
                  );
                }
              },
              onUnlink: () => notifier.updateTmdbSessionId(null),
              onEdit: () {},
            ),
          ],
        ),
      ),
    );
  }
}
