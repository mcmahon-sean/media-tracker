import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:media_tracker_test/screens/account_linking_screen.dart';
import 'package:media_tracker_test/screens/auth/login_screen.dart';
import 'package:media_tracker_test/screens/auth/register_screen.dart';
import 'package:media_tracker_test/screens/media_screen.dart';
import 'screens/home_screen.dart';
import 'package:supabase_flutter/supabase_flutter.dart';
import 'models/theme.dart' as theme;

void main() async {
  WidgetsFlutterBinding.ensureInitialized();

  // Initializes the Supabase client with your project's URL and anonymous public key.
  // This must be done before using any Supabase features in your app.
  await Supabase.initialize(
    url: 'https://hrqakudeaalvgstpupdu.supabase.co/',
    anonKey:
        'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImhycWFrdWRlYWFsdmdzdHB1cGR1Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDI5NDk3MzAsImV4cCI6MjA1ODUyNTczMH0.k30q2Ndf-YI0RPGiwllMGJFPYMp5XoRQilCktlMmqFU',
  );

  runApp(ProviderScope(child: MyApp()));
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'Media Tracker',
      theme: theme.themeData,
      initialRoute: '/',
      routes: {
        '/': (context) => const HomeScreen(),
        '/login': (context) => const LoginScreen(),
        '/register': (context) => const RegisterScreen(),
        '/media': (context) => const MediaScreen(),
        '/linkAccounts': (context) => AccountLinkingScreen(),
      },
    );
  }
}
