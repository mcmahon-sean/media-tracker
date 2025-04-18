import 'package:flutter/material.dart';
import 'package:media_tracker_test/providers/auth_provider.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../models/lastfm/lastfm_artist.dart';
import '../../models/lastfm/lastfm_track.dart';
import '../../models/lastfm/lastfm_user.dart';
import '../../models/steam/steam_model.dart';
import '../../models/tmdb/tmdb_account.dart';
import '../../models/tmdb/tmdb_movie.dart';
import '../../models/tmdb/tmdb_tv_show.dart';
import '../../services/media_api/steam_service.dart';
import '../../services/media_api/lastfm_service.dart';
import '../../services/media_api/tmdb_service.dart';
import 'steam_section.dart';
import 'lastfm_section.dart';
import 'tmdb_section.dart';
import '../home_screen.dart';
import '../widgets/drawer_menu.dart';

class MediaScreen extends ConsumerStatefulWidget {
  const MediaScreen({super.key});

  @override
  ConsumerState<MediaScreen> createState() => _MediaScreenState();
}

class _MediaScreenState extends ConsumerState<MediaScreen> {
  int _selectedIndex = 0;

  // Steam
  List<SteamGame> _steamGames = [];
  bool _isLoadingSteam = false;

  // Last.fm
  LastFmUser? _lastFmUser;
  List<LastFmArtist> _topArtists = [];
  List<LastFmTrack> _recentTracks = [];
  bool _isLoadingLastFm = false;

  // TMDB
  TMDBAccount? _tmdbAccount;
  List<TMDBMovie> _ratedMovies = [];
  List<TMDBTvShow> _favoriteTvShows = [];
  bool _isLoadingTmdb = false;

  @override
  void initState() {
    super.initState();
    _loadPlatformData(_selectedIndex); // Load Steam by default
  }

  // Load the data for the selected platform based on index
  void _loadPlatformData(int index) {
    final auth = ref.read(authProvider);
    switch (index) {
      case 0:
        if (auth.steamId != null && _steamGames.isEmpty) _loadSteamGames();
        break;
      case 1:
        if (auth.lastFmUsername != null &&
            (_topArtists.isEmpty || _recentTracks.isEmpty)) {
          _loadLastFmData();
        }
        break;
      case 2:
        if (auth.tmdbSessionId != null && _tmdbAccount == null) _loadTmdbData();
        break;
    }
  }

  // Fetches and sets Steam game data
  Future<void> _loadSteamGames() async {
    setState(() => _isLoadingSteam = true);
    try {
      final games = await fetchSteamGames();
      setState(() => _steamGames = games); // Set game list
    } catch (e) {
      print('Steam load error: $e');
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text("Failed to load Steam data. Try again later.")),
      );
    } finally {
      setState(() => _isLoadingSteam = false);
    }
  }

  // Fetches and sets Last.fm user data
  Future<void> _loadLastFmData() async {
    setState(() => _isLoadingLastFm = true);
    try {
      final user = await fetchLastFmUser();
      final artists = await fetchLastFmTopArtists();
      final tracks = await fetchLastFmRecentTracks();

      setState(() {
        _lastFmUser = user; // Get user profile
        _topArtists = artists; // Get top artists
        _recentTracks = tracks; // Get recent tracks
        _isLoadingLastFm = false; // Stop loading
      });
    } catch (e) {
      print('Last.fm load error: $e');
      setState(() => _isLoadingLastFm = false);
    }
  }

  // Fetches and sets TMDB user data
  Future<void> _loadTmdbData() async {
    setState(() => _isLoadingTmdb = true);
    try {
      final account = await TMDBService.fetchAccountDetails();
      final movies = await TMDBService.fetchRatedMovies();
      final shows = await TMDBService.fetchFavoriteTvShows();

      setState(() {
        _tmdbAccount = account; // Get user profile
        _ratedMovies = movies; // Get rated movies
        _favoriteTvShows = shows; // Get favorite shows
        _isLoadingTmdb = false; // Stop loading
      });
    } catch (e) {
      print('TMDB load error: $e');
      setState(() => _isLoadingTmdb = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    final auth = ref.watch(authProvider);
    final firstName = auth.firstName;

    return Scaffold(
      appBar: AppBar(
        title: Text('$firstName\'s Media'),
        actions: [
          IconButton(
            icon: const Icon(Icons.logout),
            color: Colors.grey,
            tooltip: 'Logout',
            onPressed: () {
              // Clear auth state
              ref.read(authProvider.notifier).logout();
              // Navigate back to the home screen
              Navigator.of(context).pushReplacement(
                MaterialPageRoute(builder: (_) => const HomeScreen()),
              );
            },
          ),
        ],
      ),
      drawer: DrawerMenu(
        firstName: firstName ?? '',
        onSectionSelected: (index) {
          setState(() {
            _selectedIndex = index;
          });
          _loadPlatformData(index);
        },
      ),
      body: _buildBody(),
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: _selectedIndex,
        onTap: (index) {
          setState(() {
            _selectedIndex = index;
          });
          _loadPlatformData(index);
        },
        items: const [
          BottomNavigationBarItem(
            icon: Icon(Icons.videogame_asset),
            label: 'Steam',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.music_note),
            label: 'Last.fm',
          ),
          BottomNavigationBarItem(icon: Icon(Icons.movie), label: 'TMDB'),
        ],
      ),
    );
  }

  Widget _buildBody() {
    final auth = ref.watch(authProvider);

    switch (_selectedIndex) {
      case 0:
        if (_isLoadingSteam) return _loading();
        if (auth.steamId == null) return _noMediaLinkedPrompt("Steam");
        return buildSteamSection(_steamGames);
      case 1:
        if (_isLoadingLastFm) return _loading();
        if (auth.lastFmUsername == null) return _noMediaLinkedPrompt("Last.fm");
        return buildLastFmSection(_lastFmUser, _topArtists, _recentTracks);
      case 2:
        if (_isLoadingTmdb) return _loading();
        if (auth.tmdbSessionId == null) return _noMediaLinkedPrompt("TMDB");
        return buildTmdbSection(_tmdbAccount, _ratedMovies, _favoriteTvShows);
      default:
        return Center(child: Text("Coming soon..."));
    }
  }

  Widget _loading() => const Center(child: CircularProgressIndicator());

  Widget _noMediaLinkedPrompt(String platform) {
    return Center(
      child: Padding(
        padding: const EdgeInsets.all(24.0),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Icon(Icons.link_off, size: 64, color: Colors.grey),
            SizedBox(height: 16),
            Text(
              "$platform account not linked",
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.w500),
            ),
            SizedBox(height: 12),
            ElevatedButton.icon(
              icon: Icon(Icons.link),
              label: Text("Link $platform Account"),
              onPressed: () async {
                final didLink = await Navigator.pushNamed(
                  context,
                  '/linkAccounts',
                );
                if (didLink == true) {
                  _loadPlatformData(_selectedIndex);
                }
              },
            ),
          ],
        ),
      ),
    );
  }
}
