import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:media_tracker_test/screens/media_screen.dart';
import '../../services/auth_service.dart';

class RegisterScreen extends ConsumerStatefulWidget {
  const RegisterScreen({super.key});

  @override
  _RegisterScreen createState() => _RegisterScreen();
}

class _RegisterScreen extends ConsumerState<RegisterScreen> {
  final usernameController = TextEditingController();
  final passwordController = TextEditingController();
  final emailController = TextEditingController();
  final firstNameController = TextEditingController();
  final lastNameController = TextEditingController();

  void register() async {
    final authService = ref.read(authServiceProvider);
    // A map of the TextEditingControllers for the required fields and an associated error message
    Map<TextEditingController, String> fieldInputMessages = {
      usernameController: "Please enter a username.",
      passwordController: "Please enter a password.",
      emailController: "Please enter a valid email address.",
      firstNameController: "Please enter your first name."
    };
    
    RegExp emailPattern = RegExp(r'.+@.+'); // Simple Regex pattern for validating email address

    String validationMessage = "";

    // Iterates through all controllers in the fieldInputMessages map and adds their error message if empty
    fieldInputMessages.forEach((k, v) => validationMessage += k.text.trim().isEmpty ? "$v\n" : "" );

    // Adds a validation message if emailController isn't empty and doesn't match the Regex pattern.
    if (emailController.text.trim().isNotEmpty && !emailPattern.hasMatch(emailController.text.trim())) {
      validationMessage += "Please enter a valid email address.";
    }

    if (validationMessage != "") {
      showDialog(
        context: context,
        builder:
            (_) => AlertDialog(
              title: Text('Invalid Input'),
              content: Text(validationMessage, style: TextStyle(color: Colors.black)),
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
      // Attempt to register a new user using the AuthService's register method
      // This sends all required user data to Supabase, calling a stored procedure on the backend
      final success = await authService.register(
        username: usernameController.text.trim(),
        password: passwordController.text,
        firstName: firstNameController.text.trim(),
        lastName: lastNameController.text.trim(),
        email: emailController.text.trim(),
      );

      if (success) {
        usernameController.clear();
        passwordController.clear();
        emailController.clear();
        firstNameController.clear();
        lastNameController.clear();

        Navigator.push(
          context,
          MaterialPageRoute(builder: (_) => MediaScreen()),
        );
      } else {
        showDialog(
          context: context,
          builder:
              (_) => AlertDialog(
                title: Text('Username Already Exists'),
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
      appBar: AppBar(title: Text('Sign Up')),
      body: SingleChildScrollView(
        // Padding(
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
                    controller: firstNameController,
                    maxLength: 50,
                    decoration: InputDecoration(label: Text('First Name')),
                  ),
                ),
              ],
            ),
            Row(
              children: [
                SizedBox(
                  width: 300,
                  child: TextField(
                    controller: lastNameController,
                    maxLength: 50,
                    decoration: InputDecoration(label: Text('Last Name')),
                  ),
                ),
              ],
            ),
            Row(
              children: [
                SizedBox(
                  width: 300,
                  child: TextField(
                    controller: emailController,
                    maxLength: 50,
                    decoration: InputDecoration(label: Text('Email')),
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
                ElevatedButton(onPressed: register, child: Text('Sign Up')),
              ],
            ),
          ],
          // ),
        ),
      ),
    ));
  }
}
