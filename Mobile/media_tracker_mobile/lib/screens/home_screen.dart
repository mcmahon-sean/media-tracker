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
      print(result.toString());
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
      body: SafeArea(
        child: Stack(
          children: [
            LayoutBuilder(
              builder: (context, constraints) {
                final screenHeight = MediaQuery.of(context).size.height;

                final content = ConstrainedBox(
                  constraints: BoxConstraints(minHeight: constraints.maxHeight),
                  child: IntrinsicHeight(
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        if (MediaQuery.of(context).orientation ==
                            Orientation.portrait)
                          SizedBox(height: 80),

                        const Icon(
                          Icons.movie_creation_rounded,
                          color: Colors.white,
                          size: 80,
                        ),
                        Column(
                          children: [
                            Text(
                              'Welcome to Media Tracker',
                              style: Theme.of(
                                context,
                              ).textTheme.headlineMedium?.copyWith(
                                color: Colors.white,
                                fontWeight: FontWeight.bold,
                              ),
                              textAlign: TextAlign.center,
                            ),
                            const SizedBox(height: 16),
                            Text(
                              'Track your favorite games, movies, shows, and music — all in one place.',
                              style: Theme.of(context).textTheme.bodyLarge
                                  ?.copyWith(color: Colors.white70),
                              textAlign: TextAlign.center,
                            ),
                            const SizedBox(height: 40),
                            SizedBox(
                              width: double.infinity,
                              child: ElevatedButton(
                                onPressed: () {
                                  Navigator.push(
                                    context,
                                    MaterialPageRoute(
                                      builder: (_) => LoginScreen(),
                                    ),
                                  );
                                },
                                style: ElevatedButton.styleFrom(
                                  padding: const EdgeInsets.symmetric(
                                    vertical: 16,
                                  ),
                                ),
                                child: const Text('Sign In'),
                              ),
                            ),
                            const SizedBox(height: 16),
                            SizedBox(
                              width: double.infinity,
                              child: OutlinedButton(
                                onPressed: () {
                                  Navigator.push(
                                    context,
                                    MaterialPageRoute(
                                      builder: (_) => RegisterScreen(),
                                    ),
                                  );
                                },

                                style: OutlinedButton.styleFrom(
                                  padding: const EdgeInsets.symmetric(
                                    vertical: 16,
                                  ),
                                  side: const BorderSide(color: Colors.white),
                                  foregroundColor: Colors.white,
                                ),
                                child: const Text('Register'),
                              ),
                            ),
                          ],
                        ),
                        const SizedBox(height: 24),
                        const Text(
                          'v1.0.0',
                          style: TextStyle(color: Colors.white38, fontSize: 12),
                        ),
                      ],
                    ),
                  ),
                );

                if (screenHeight < 600) {
                  return SingleChildScrollView(
                    padding: const EdgeInsets.symmetric(
                      horizontal: 32,
                      vertical: 48,
                    ),
                    child: content,
                  );
                } else {
                  return Padding(
                    padding: const EdgeInsets.symmetric(
                      horizontal: 32,
                      vertical: 48,
                    ),
                    child: content,
                  );
                }
              },
            ),
          ],
        ),
      ),
    );
  }
}
