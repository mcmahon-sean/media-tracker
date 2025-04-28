import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:media_tracker_test/providers/auth_provider.dart';
import 'package:media_tracker_test/screens/auth/account_settings.dart';

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
    return Drawer(
      backgroundColor: Color.fromARGB(255, 72, 72, 72),
      child: ListView(
        padding: EdgeInsets.zero,
        children: [
          DrawerHeader(
            decoration: const BoxDecoration(color: Colors.grey),
            child: Text(
              'Hello, $firstName!',
              style: const TextStyle(color: Colors.white, fontSize: 24),
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
                      (context) =>
                          AccountSettings(initialUsername: auth.username!),
                ),
              );
            },
          ),
        ],
      ),
    );
  }
}
