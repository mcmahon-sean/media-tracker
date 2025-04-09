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
      theme: ThemeData(
        scaffoldBackgroundColor: Color.fromARGB(255, 48, 48, 48),
        textTheme: TextTheme(
          headlineLarge: TextStyle(color: Colors.white)
        ),
        appBarTheme: AppBarTheme(
          backgroundColor: Color.fromARGB(255, 16, 16, 16), 
          surfaceTintColor: Colors.black,
          foregroundColor: Colors.white
        ),
        elevatedButtonTheme: ElevatedButtonThemeData(
          style: ButtonStyle(
            backgroundColor: WidgetStateProperty.all<Color>(Color.fromARGB(255, 96, 96, 96)),
            foregroundColor: WidgetStateProperty.all<Color>(Colors.white)
          )
        ),
        bottomNavigationBarTheme: BottomNavigationBarThemeData(
          backgroundColor: Color.fromARGB(255, 16, 16, 16),
          unselectedItemColor: Colors.white70,
          selectedIconTheme: IconThemeData(size: 32),
          selectedItemColor: Colors.white,
          selectedLabelStyle: TextStyle(fontSize: 16)
        ),
        listTileTheme: ListTileThemeData(
          titleTextStyle: TextStyle(
            color: Colors.white,
            fontSize: 16,
            fontWeight: FontWeight.bold
          ),
          subtitleTextStyle: TextStyle(color: Colors.white)
        )
      ),
      home: HomeScreen(),
    );
  }
}
