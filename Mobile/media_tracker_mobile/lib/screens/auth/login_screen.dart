import 'package:flutter/material.dart';
import 'package:media_tracker_test/screens/media_screen.dart';
import '../../services/auth_service.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  _LoginScreenState createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final usernameController = TextEditingController();
  final passwordController = TextEditingController();
  final authService = AuthService();

  void login() async {
    if (usernameController.text.trim().isEmpty ||
        passwordController.text.trim().isEmpty) {
      showDialog(
        context: context,
        builder:
            (_) => AlertDialog(
              title: Text('Invalid Input'),
              content: Text('Please enter a valid username and password.'),
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
      final success = await authService.login(
        usernameController.text.trim(),
        passwordController.text.trim(),
      );

      if (success) {
        usernameController.clear();
        passwordController.clear();
        Navigator.push(
          context,
          MaterialPageRoute(builder: (_) => MediaScreen()),
        );
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
    return (Scaffold(
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
    ));
  }
}
