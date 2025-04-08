import 'package:flutter/material.dart';
import 'screens/home_screen.dart';
import 'package:supabase_flutter/supabase_flutter.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();

  // Initializes the Supabase client with your project's URL and anonymous public key.
  // This must be done before using any Supabase features in your app.
  await Supabase.initialize(
    url: 'https://hrqakudeaalvgstpupdu.supabase.co/',
    anonKey: 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImhycWFrdWRlYWFsdmdzdHB1cGR1Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDI5NDk3MzAsImV4cCI6MjA1ODUyNTczMH0.k30q2Ndf-YI0RPGiwllMGJFPYMp5XoRQilCktlMmqFU',
  );

  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'Media Tracker',
      theme: ThemeData(primarySwatch: Colors.blue),
      home: HomeScreen(),
    );
  }
}
