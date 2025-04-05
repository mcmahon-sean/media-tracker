import 'package:flutter/material.dart';
import 'media_screen.dart';

class HomeScreen extends StatelessWidget {
  const HomeScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text('Home')),
      body: Center(
        child: ElevatedButton(
          onPressed: () {
            Navigator.push(
              context,
              MaterialPageRoute(builder: (context) => MediaScreen()),
            );
          },
          child: Text('Go to Media'),
        ),
      ),
    );
  }
}
