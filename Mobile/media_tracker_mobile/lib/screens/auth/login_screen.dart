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
   final _formKey = GlobalKey<FormState>();
    // for teh form valididation
  final TextEditingController usernameController = TextEditingController();
  final TextEditingController passwordController = TextEditingController();

 

  void login() async {
   //  Validate al the fields first
    if (_formKey.currentState!.validate()) {
      try {
        // RPC to 'login' stored procedure with username and password
        final result = await Supabase.instance.client.rpc(
          'login',
          params: {
            'usernamevar': usernameController.text,
            'passwordvar': passwordController.text,
          },
        );

         if (result == true) {
          // Retrieve platform IDs for the logged-in user
          final steamID = await Supabase.instance.client
              .from('useraccounts')
              .select('user_platform_id')
              .eq('username', usernameController.text)
              .eq('platform_id', 1);
          final lastfmID = await Supabase.instance.client
              .from('useraccounts')
              .select('user_platform_id')
              .eq('username', usernameController.text)
              .eq('platform_id', 2);
          final imdbID = await Supabase.instance.client
              .from('useraccounts')
              .select('user_platform_id')
              .eq('username', usernameController.text)
              .eq('platform_id', 3);

          //clears the controllers
          usernameController.clear();
          passwordController.clear();

          //sets the ids in ApiServices
          ApiServices.steamUserId = steamID[0]['user_platform_id'].toString();
          ApiServices.lastFmUser = lastfmID[0]['user_platform_id'].toString();
          ApiServices.tmdbUser = imdbID[0]['user_platform_id'].toString();

          //sends to media screen
          Navigator.push(
            context,
            MaterialPageRoute(builder: (context) => MediaScreen()),
          );
        } else {
         showDialog(
            context: context,
            builder: (context) => AlertDialog(
              title: const Text('Wrong Username/Password'),
              content: const Text('Wrong password or account doesn\'t exist.'),
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
        // If there’s an error (e.g., procedure not found, bad params),
        // catch it and show an error SnackBar instead
        ScaffoldMessenger.of(
          context,
        ).showSnackBar(SnackBar(content: Text('Error: $e')),

        );
      
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Login'),
        centerTitle: true, 
   
      ),
      
      body: Padding(
        padding: const EdgeInsets.all(24.0),
        child: Form(
          key: _formKey,
          child: ListView(
            children: [
              TextFormField(
                controller: usernameController,
                maxLength: 50,
                decoration: const InputDecoration(
                  labelText: 'Username',
                  labelStyle: TextStyle(color: Color.fromARGB(179, 0, 0, 0)),
                ),
                style: const TextStyle(color: Color.fromARGB(255, 0, 0, 0)),
                validator: (value) =>
                    value == null || value.isEmpty ? 'Username is required' : null,
              ),
              TextFormField(
                controller: passwordController,
                maxLength: 50,
                obscureText: true,
                decoration: const InputDecoration(
                  labelText: 'Password',
                  labelStyle: TextStyle(color: Color.fromARGB(179, 0, 0, 0)),
                ),
                style: const TextStyle(color: Color.fromARGB(255, 0, 0, 0)),
                validator: (value) =>
                    value == null || value.isEmpty ? 'Password is required' : null,
              ),
              const SizedBox(height: 20),
              ElevatedButton(
                onPressed: login,
                child: const Text('Login'),
              ),
            ],
          ),
        ),
      ),
    );
  }
}