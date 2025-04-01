import 'package:flutter/material.dart';

class MediaScreen extends StatefulWidget {
  const MediaScreen({super.key});

  @override
  _MediaScreenState createState() => _MediaScreenState();
}

class _MediaScreenState extends State<MediaScreen> {
  int _selectedIndex = 0; // Index for navigation bar
  final List<String> _platforms = ['Steam', 'TMDB', 'Last.fm'];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text('Media Platform')),
      body: Center(
        child: Text(
          "Selected: ${_platforms[_selectedIndex]}",
          style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
        ),
      ),
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: _selectedIndex,
        onTap: (index) {
          setState(() {
            _selectedIndex = index;
          });
        },
        items: [
          BottomNavigationBarItem(icon: Icon(Icons.videogame_asset), label: 'Steam'),
          BottomNavigationBarItem(icon: Icon(Icons.movie), label: 'TMDB'),
          BottomNavigationBarItem(icon: Icon(Icons.music_note), label: 'Last.fm'),
        ],
      ),
    );
  }
}
