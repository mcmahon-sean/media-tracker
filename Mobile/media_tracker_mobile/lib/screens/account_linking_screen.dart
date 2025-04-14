import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import '../providers/auth_provider.dart';
import 'widgets/link_account_card.dart';

class AccountLinkingScreen extends ConsumerWidget {
  const AccountLinkingScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final auth = ref.watch(authProvider);
    final notifier = ref.read(authProvider.notifier);

    return Scaffold(
      appBar: AppBar(
        title: const Text('Link Media Accounts'),
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.symmetric(vertical: 16),
        child: Column(
          children: [
            LinkAccountCard(
              platformName: 'Steam',
              linkedValue: auth.steamId,
              onLink: (id) => notifier.updateSteamId(id),
              onUnlink: () => notifier.updateSteamId(null),
              onEdit: () {},
            ),
            LinkAccountCard(
              platformName: 'Last.fm',
              linkedValue: auth.lastFmUsername,
              onLink: (username) => notifier.updateLastFmUsername(username),
              onUnlink: () => notifier.updateLastFmUsername(null),
              onEdit: () {},
            ),
            LinkAccountCard(
              platformName: 'TMDB',
              linkedValue: auth.tmdbSessionId,
              onLink: (sessionId) => notifier.updateTmdbSessionId(sessionId),
              onUnlink: () => notifier.updateTmdbSessionId(null),
              onEdit: () {},
            ),
          ],
        ),
      ),
    );
  }
}
