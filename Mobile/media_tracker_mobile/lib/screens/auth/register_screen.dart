import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:media_tracker_test/screens/media/media_screen.dart';
import '../../services/auth_service.dart';

class RegisterScreen extends ConsumerStatefulWidget {
  const RegisterScreen({super.key});

  @override
  _RegisterScreen createState() => _RegisterScreen();
}

class _RegisterScreen extends ConsumerState<RegisterScreen> {
  final usernameController = TextEditingController();
  final passwordController = TextEditingController();
  final confirmPasswordController = TextEditingController();
  final emailController = TextEditingController();
  final firstNameController = TextEditingController();
  final lastNameController = TextEditingController();

  @override
  void dispose() {
    usernameController.dispose();
    passwordController.dispose();
    emailController.dispose();
    firstNameController.dispose();
    lastNameController.dispose();
    confirmPasswordController.dispose();
    super.dispose();
  }

  void register() async {
    final authService = ref.read(authServiceProvider);
    // A map of the TextEditingControllers for the required fields and an associated error message
    Map<TextEditingController, String> fieldInputMessages = {
      usernameController: "Please enter a username.",
      passwordController: "Please enter a password.",
      emailController: "Please enter a valid email address.",
      firstNameController: "Please enter your first name.",
    };

    RegExp emailPattern = RegExp(
      r'.+@.+',
    ); // Simple Regex pattern for validating email address

    String validationMessage = "";

    // Iterates through all controllers in the fieldInputMessages map and adds their error message if empty
    fieldInputMessages.forEach(
      (k, v) => validationMessage += k.text.trim().isEmpty ? "$v\n" : "",
    );

    // Adds a validation message if emailController isn't empty and doesn't match the Regex pattern.
    if (emailController.text.trim().isNotEmpty &&
        !emailPattern.hasMatch(emailController.text.trim())) {
      validationMessage += "Please enter a valid email address.";
    }

    if (passwordController.text.trim() !=
        confirmPasswordController.text.trim()) {
      validationMessage += "Passwords do not match.\n";
    }

    if (validationMessage != "") {
      showDialog(
        context: context,
        builder:
            (_) => AlertDialog(
              title: Text('Invalid Input'),
              content: Text(
                validationMessage,
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
    return Scaffold(
      appBar: AppBar(title: const Text('Sign Up')),
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
                      _buildField('First Name', firstNameController),
                      _buildField('Last Name', lastNameController),
                      _buildField('Email', emailController),
                      _buildField(
                        'Password',
                        passwordController,
                        obscure: true,
                      ),
                      _buildField(
                        'Confirm Password',
                        confirmPasswordController,
                        obscure: true,
                      ),
                    ],
                  ),
                ),
              ),
              SizedBox(
                width: double.infinity,
                child: ElevatedButton(
                  onPressed: register,
                  child: const Text('Sign Up'),
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
        maxLength: 50,
        obscureText: obscure,
        decoration: InputDecoration(labelText: label),
      ),
    );
  }
}
