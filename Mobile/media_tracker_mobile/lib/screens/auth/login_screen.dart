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

  void login() async {
    if (usernameController.text.replaceAll(' ', '') != '' &&
        passwordController.text.replaceAll(' ', '') != '') {
      try {
        // Makes an RPC (remote procedure call) to the stored procedure 'login'
        // It passes the username and password
        final result = await Supabase.instance.client.rpc(
          'login',
          params: {
            'usernamevar': usernameController.text,
            'passwordvar': passwordController.text,
          },
        );

        if (result == true) {
          //gets the related ID's
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
            builder: (context) {
              return AlertDialog(
                title: Text('Wrong Username/Password'),
                content: Text('Wrong password or account doesn\'t exsist.'),
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
            content: Text('Please enter a valid username and password.'),
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
