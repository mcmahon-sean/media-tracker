import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:media_tracker_test/providers/auth_provider.dart';
import 'package:media_tracker_test/screens/media/media_screen.dart';
import '../../services/auth_service.dart';

class LoginScreen extends ConsumerStatefulWidget {
  const LoginScreen({super.key});

  @override
  _LoginScreenState createState() => _LoginScreenState();
}

class _LoginScreenState extends ConsumerState<LoginScreen> {
  final usernameController = TextEditingController();
  final passwordController = TextEditingController();

  @override
  void dispose() {
    usernameController.dispose();
    passwordController.dispose();
    super.dispose();
  }

  void login() async {
    final authService = ref.read(authServiceProvider);
    if (usernameController.text.trim().isEmpty ||
        passwordController.text.trim().isEmpty) {
      showDialog(
        context: context,
        builder:
            (_) => AlertDialog(
              title: Text('Invalid Input'),
              content: Text(
                'Please enter a valid username and password.',
                style: TextStyle(color: Colors.black),
              ),
              actions: [
                TextButton(
                  onPressed: () => Navigator.pop(context),
                  child: Text('Okay'),
                ),
              ],
            ),
      );
      return;
    }

    try {
      // Attempt to log in using the AuthService's login method
      // It sends the trimmed username and password to Supabase for authentication
      final success = await authService.login(
        usernameController.text.trim(),
        passwordController.text.trim(),
      );

      if (success) {
        final auth = ref.read(authProvider);

        if (auth.username != null) {
          usernameController.clear();
          passwordController.clear();
          Navigator.push(
            context,
            MaterialPageRoute(builder: (_) => MediaScreen()),
          );
        } else {
          print('Login succeeded but user data is missing.');
        }
      } else {
        showDialog(
          context: context,
          builder:
              (_) => AlertDialog(
                title: Text('Wrong Username/Password'),
                content: Text('Wrong password or account doesn\'t exist.'),
                actions: [
                  TextButton(
                    onPressed: () => Navigator.pop(context),
                    child: Text('Okay'),
                  ),
                ],
              ),
        );
      }
    } catch (e) {
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text('Error: $e')));
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Login')),
      body: SafeArea(
        child: Padding(
          padding: const EdgeInsets.all(24.0),
          child: Column(
            children: [
              Expanded(
                child: SingleChildScrollView(
                  padding: const EdgeInsets.only(bottom: 24),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      _buildField('Username', usernameController),
                      _buildField(
                        'Password',
                        passwordController,
                        obscure: true,
                      ),
                    ],
                  ),
                ),
              ),
              SizedBox(
                width: double.infinity,
                child: ElevatedButton(
                  onPressed: login,
                  child: const Text('Login'),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildField(
    String label,
    TextEditingController controller, {
    bool obscure = false,
  }) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 16),
      child: TextField(
        controller: controller,
        obscureText: obscure,
        maxLength: 50,
        decoration: InputDecoration(labelText: label),
      ),
    );
  }
}
