import 'package:flutter/material.dart';
import 'package:media_tracker_test/screens/media_screen.dart';
import 'package:supabase_flutter/supabase_flutter.dart';

// This screen allows users to create a new account by submitting their info.


class RegisterScreen extends StatefulWidget {
  const RegisterScreen({super.key});

  @override
  _RegisterScreenState createState() => _RegisterScreenState();
}

class _RegisterScreenState extends State<RegisterScreen> {
  // Key used to validate the entire form
  final _formKey = GlobalKey<FormState>();

  // Controllers to capture user input
  final TextEditingController usernameController = TextEditingController();
  final TextEditingController passwordController = TextEditingController();
  final TextEditingController emailController = TextEditingController();
  final TextEditingController firstNameController = TextEditingController();
  final TextEditingController lastNameController = TextEditingController();

  // Function to handle registration logic
  void register() async {
    // Validate all form fields
    if (_formKey.currentState!.validate()) {
      try {
        // Makes an RPC (remote procedure call) to the stored procedure 'CreateUser'
        // Passes all required fields as parameters

        final result = await Supabase.instance.client.rpc(
          'CreateUser',
          params: {
            'usernamevar': usernameController.text,
            'firstnamevar': firstNameController.text,
            'lastnamevar': lastNameController.text,
            'emailvar': emailController.text,
            'passwordvar': passwordController.text,
          },
        );

        if (result == true) {
          // Clear all text fields on successful registration

          usernameController.clear();
          passwordController.clear();
          emailController.clear();
          firstNameController.clear();
          lastNameController.clear();

          // Navigate to media screen after successful registration
          Navigator.push(
            context,
            MaterialPageRoute(builder: (context) => const MediaScreen()),
          );
        } else {
          // Show alert if the username already exists
          showDialog(
            context: context,
            builder: (context) => AlertDialog(
              title: const Text('Username Already Exists'),
              actions: [
                TextButton(
                  onPressed: () => Navigator.pop(context),
                  child: const Text('Okay'),
                ),
              ],
            ),
          );
        }
      } catch (e) {
        // Show a snackbar with the error message if something goes wrong
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Error: $e')),
        );
      }
    } else {
      // Form input is invalid – show a warning dialog
      showDialog(
        context: context,
        builder: (context) => AlertDialog(
          title: const Text('Invalid Input'),
          content: const Text('Please enter valid information.'),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(context),
              child: const Text('Okay'),
            ),
          ],
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      // AppBar with centered title
      appBar: AppBar(
        title: const Text('Sign Up'),
        centerTitle: true,
      ),

      
      

      // Form content
      body: Padding(
        padding: const EdgeInsets.all(24.0),
        child: Form(
          key: _formKey,
          child: ListView(
            children: [
              // Username field with validation
              TextFormField(
                controller: usernameController,
                maxLength: 50,
                decoration: const InputDecoration(labelText: 'Username'),
                validator: (value) =>
                    value == null || value.isEmpty ? 'Username is required' : null,
              ),
              // First name field with validation
              TextFormField(
                controller: firstNameController,
                maxLength: 50,
                decoration: const InputDecoration(labelText: 'First Name'),
                validator: (value) =>
                    value == null || value.isEmpty ? 'First name is required' : null,
              ),
              // Last name field optional not requrieed
              TextFormField(
                controller: lastNameController,
                maxLength: 50,
                decoration: const InputDecoration(labelText: 'Last Name'),
              ),
              // Email field with the email format validation
              TextFormField(
                controller: emailController,
                maxLength: 50,
                decoration: const InputDecoration(labelText: 'Email'),
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Email is required';
                  } else if (!RegExp(r'^[\w\.-]+@[\w\.-]+\.\w{2,4}$').hasMatch(value)) {
                    return 'Enter a valid email address';
                  }
                  return null;
                },
              ),
              // Password field with length validation
              TextFormField(
                controller: passwordController,
                maxLength: 50,
                obscureText: true,
                decoration: const InputDecoration(labelText: 'Password'),
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Password is required';
                  } else if (value.length < 6) {
                    return 'Password must be at least 6 characters';
                  }
                  return null;
                },
              ),
              const SizedBox(height: 20),
              // Register button that submits the form
              ElevatedButton(
                onPressed: register,
                child: const Text('Sign Up'),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
