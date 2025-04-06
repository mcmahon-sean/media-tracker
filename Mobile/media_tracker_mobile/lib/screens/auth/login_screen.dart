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
    //we'll have to add further value testing to the controllers
    //making sure they're not null and whatnot
    //but we can add that next week.
    //we'll also have to add proper error messages, but for now
    //this works
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

      // If the call is successful, show a SnackBar with the result
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text(result.toString())));

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
        //we'll have to test to make sure they're not null
        //but we can fix that up next week
        ApiServices.steamUserId = steamID[0]['user_platform_id'].toString();
        ApiServices.lastFmUser = lastfmID[0]['user_platform_id'].toString();
        ApiServices.tmdbUser = imdbID[0]['user_platform_id'].toString();

        //sends to media screen
        Navigator.push(
          context,
          MaterialPageRoute(builder: (context) => MediaScreen()),
        );
      } else {
        ScaffoldMessenger.of(
          context,
        ).showSnackBar(SnackBar(content: Text(result.toString())));
      }
    } catch (e) {
      // If there’s an error (e.g., procedure not found, bad params),
      // catch it and show an error SnackBar instead
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
