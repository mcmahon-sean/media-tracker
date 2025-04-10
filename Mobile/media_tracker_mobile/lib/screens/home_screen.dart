import 'package:flutter/material.dart';
import 'package:media_tracker_test/screens/auth/register_screen.dart';
import 'package:supabase_flutter/supabase_flutter.dart';
import 'auth/login_screen.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  @override
  void initState() {
    super.initState();
    // Ensures that the following callback (_showConnectionMessage)
    // runs after the first frame is rendered, so context is available
    // for showing a SnackBar (which needs a Scaffold to be built).
    WidgetsBinding.instance.addPostFrameCallback((_) {
      _showConnectionMessage();
    });
  }

  // This function calls a Supabase stored procedure and shows the result in a SnackBar.
  Future<void> _showConnectionMessage() async {
    try {
      // Makes an RPC (remote procedure call) to the stored procedure 'testconnectionwitharguments'
      // It passes a parameter 'app' with value 'Mobile'
      final result = await Supabase.instance.client.rpc(
        'testconnectionwitharguments',
        params: {'app': 'Mobile'},
      );

      // If the call is successful, show a SnackBar with the result
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text(result.toString())));
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
    return Scaffold(
      appBar: AppBar(title: Text('Home')),
      body: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Text(
                'Not Signed In',
                style: Theme.of(context).textTheme.headlineLarge
              )
            ]
          ),
          SizedBox(height: 16),
          Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              ElevatedButton(
                onPressed: () {
                  Navigator.push(
                    context,
                    MaterialPageRoute(builder: (context) => LoginScreen()),
                  );
                },
                child: Text('Sign In'),
              ),
            ],
          ),
          Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              ElevatedButton(
                onPressed: () {
                  Navigator.push(
                    context,
                    MaterialPageRoute(builder: (context) => RegisterScreen()),
                  );
                },
                child: Text('Sign Up'),
              ),
            ],
          ),
        ],
      ),
    );
  }
}
