import 'package:flutter/material.dart';
import 'package:media_tracker_test/config/api_connections.dart';
import 'package:media_tracker_test/screens/media_screen.dart';
import 'package:supabase_flutter/supabase_flutter.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  _LoginScreenState createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  TextEditingController usernameController = TextEditingController();
  TextEditingController passwordController = TextEditingController();

  @override
  void initState() {
    super.initState();
  }

  @override
  void dispose() {
    usernameController.dispose();
    passwordController.dispose();
    super.dispose();
  }

  Future<String?> _getPlatformID(String username, int platformId) async {
    try {
      final response = await Supabase.instance.client
          .from('useraccounts')
          .select('user_platform_id')
          .eq('username', username)
          .eq('platform_id', platformId)
          .maybeSingle();

      return response?['user_platform_id']?.toString();
    } catch (e) {
      print('Error fetching platform ID for $platformId: $e');
      return null;
    }
  }

  void login() async {
    if (usernameController.text.trim().isNotEmpty &&
        passwordController.text.trim().isNotEmpty) {
      try {
        final result = await Supabase.instance.client.rpc(
          'AuthenticateUser',
          params: {
            'usernamevar': usernameController.text,
            'passwordvar': passwordController.text,
          },
        );

        if (result == true) {
          final steamID =
              await _getPlatformID(usernameController.text, 1);
          final lastfmID =
              await _getPlatformID(usernameController.text, 2);
          final imdbID =
              await _getPlatformID(usernameController.text, 3);

          usernameController.clear();
          passwordController.clear();

          ApiServices.steamUserId = steamID ?? "";
          ApiServices.lastFmUser = lastfmID ?? "";
          ApiServices.tmdbUser = imdbID ?? "";

          Navigator.push(
            context,
            MaterialPageRoute(builder: (context) => MediaScreen()),
          );
        } else {
          _showAlertDialog(
              'Wrong Username/Password', 'Incorrect login details.');
        }
      } catch (e) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Error: $e')),
        );
      }
    } else {
      _showAlertDialog(
          'Invalid Input', 'Please enter both username and password.');
    }
  }

  void _showAlertDialog(String title, String message) {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        title: Text(title),
        content: Text(message),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: const Text('Okay'),
          ),
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text('Login')),
      body: Padding(
        padding: const EdgeInsets.all(50),
        child: Column(
          children: [
            Row(
              children: [
                SizedBox(
                  width: 300,
                  child: TextField(
                    controller: usernameController,
                    maxLength: 50,
                    decoration: InputDecoration(label: Text('Username')),
                  ),
                ),
              ],
            ),
            Row(
              children: [
                SizedBox(
                  width: 300,
                  child: TextField(
                    controller: passwordController,
                    maxLength: 50,
                    obscureText: true,
                    decoration: InputDecoration(label: Text('Password')),
                  ),
                ),
              ],
            ),
            Row(
              children: [
                ElevatedButton(onPressed: login, child: Text('Login')),
              ],
            ),
          ],
        ),
      ),
    );
  }
}
