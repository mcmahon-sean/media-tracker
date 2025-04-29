import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:media_tracker_test/providers/auth_provider.dart';
import 'package:media_tracker_test/screens/auth/account_settings.dart';
import 'package:media_tracker_test/screens/home_screen.dart';

class DrawerMenu extends ConsumerWidget {
  final String firstName;
  final Function(int) onSectionSelected;

  const DrawerMenu({
    super.key,
    required this.firstName,
    required this.onSectionSelected,
  });

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final auth = ref.watch(authProvider);
    if (auth.username == null) {
      return const Drawer(child: Center(child: CircularProgressIndicator()));
    }
    return Drawer(
      backgroundColor: const Color.fromARGB(255, 72, 72, 72),
      child: Column(
        children: [
          DrawerHeader(
            decoration: const BoxDecoration(color: Colors.grey),
            margin: EdgeInsets.zero,
            padding: EdgeInsets.zero,
            child: Container(
              width: double.infinity,
              alignment: Alignment.center,
              child: Text(
                'Hello, $firstName',
                style: const TextStyle(color: Colors.white, fontSize: 24),
              ),
            ),
          ),
          ListTile(
            title: const Text('Link Media Services'),
            onTap: () {
              Navigator.pop(context);
              Navigator.pushNamed(context, '/linkAccounts');
            },
          ),
          ListTile(
            title: const Text('Favorite Media'),
            onTap: () {
              onSectionSelected(0);
              Navigator.pop(context);
            },
          ),
          ListTile(
            title: const Text('Account Settings'),
            onTap: () {
              Navigator.pop(context);
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder:
                      (context) => AccountSettings(
                        initialUsername: auth.username ?? 'Guest',
                      ),
                ),
              );
            },
          ),
          const Spacer(),
          const Divider(color: Colors.white54),
          ListTile(
            leading: const Icon(Icons.logout, color: Colors.white),
            title: const Text('Logout', style: TextStyle(color: Colors.white)),
            onTap: () {
              ref.read(authProvider.notifier).logout();
              Navigator.of(context).pushReplacement(
                MaterialPageRoute(builder: (_) => const HomeScreen()),
              );
            },
          ),
        ],
      ),
    );
  }
}
