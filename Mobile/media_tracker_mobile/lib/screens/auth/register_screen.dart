import 'package:flutter/material.dart';
import 'package:media_tracker_test/screens/media_screen.dart';
import 'package:supabase_flutter/supabase_flutter.dart';

class RegisterScreen extends StatefulWidget {
  const RegisterScreen({super.key});

  @override
  _RegisterScreen createState() => _RegisterScreen();
}

class _RegisterScreen extends State<RegisterScreen> {
  TextEditingController usernameController = TextEditingController();
  TextEditingController passwordController = TextEditingController();
  TextEditingController emailController = TextEditingController();
  TextEditingController firstNameController = TextEditingController();
  TextEditingController lastNameController = TextEditingController();

  @override
  void initState() {
    super.initState();
  }

  void login() async {
    if (usernameController.text.replaceAll(' ', '') != '' &&
        passwordController.text.replaceAll(' ', '') != '' &&
        emailController.text.replaceAll(' ', '') != '' &&
        firstNameController.text.replaceAll(' ', '') != '') {
      try {
        // Makes an RPC (remote procedure call) to the stored procedure 'login'
        // It passes the username and password
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
          //clears controllers
          usernameController.clear();
          passwordController.clear();
          emailController.clear();
          firstNameController.clear();
          lastNameController.clear();

          //sends to media screen
          Navigator.push(
            context,
            MaterialPageRoute(builder: (context) => MediaScreen()),
          );
        } else {
          showDialog(
            context: context,
            builder: (context) {
              return AlertDialog(
                title: Text('Username Already Exists'),
                actions: [
                  TextButton(
                    onPressed: () {
                      Navigator.pop(context);
                    },
                    child: const Text('Okay'),
                  ),
                ],
              );
            },
          );
        }
      } catch (e) {
        // If there’s an error (e.g., procedure not found, bad params),
        // catch it and show an error SnackBar instead
        ScaffoldMessenger.of(
          context,
        ).showSnackBar(SnackBar(content: Text('Error: $e')));
      }
    } else {
      showDialog(
        context: context,
        builder: (context) {
          return AlertDialog(
            title: Text('Invalid Input'),
            content: Text('Please enter a valid information.'),
            actions: [
              TextButton(
                onPressed: () {
                  Navigator.pop(context);
                },
                child: const Text('Okay'),
              ),
            ],
          );
        },
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return (Scaffold(
      appBar: AppBar(title: Text('Sign Up')),
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
                ElevatedButton(onPressed: login, child: Text('Sign Up')),
              ],
            ),
          ],
        ),
      ),
    ));
  }
}
