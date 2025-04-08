import 'package:flutter/material.dart';
import 'package:media_tracker_test/providers/auth_provider.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../models/lastfm/lastfm_artist.dart';
import '../models/lastfm/lastfm_track.dart';
import '../models/lastfm/lastfm_user.dart';
import '../models/steam/steam_model.dart';
import '../models/tmdb/tmdb_account.dart';
import '../models/tmdb/tmdb_movie.dart';
import '../models/tmdb/tmdb_tv_show.dart';
import '../services/media_api/steam_service.dart';
import '../services/media_api/lastfm_service.dart';
import '../services/media_api/tmdb_service.dart';

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

  void _loadPlatformData(int index) {
    switch (index) {
      case 0:
        if (_steamGames.isEmpty) _loadSteamGames();
        break;
      case 1:
        if (_tmdbAccount == null) _loadTmdbData();
        break;
      case 2:
        if (_topArtists.isEmpty || _recentTracks.isEmpty) _loadLastFmData();
        break;
      // TMDB to be added later
    }
  }

  Future<void> _loadSteamGames() async {
    setState(() => _isLoadingSteam = true);
    try {
      final games = await fetchSteamGames();
      setState(() {
        _steamGames = games;
        _isLoadingSteam = false;
      });
    } catch (e) {
      print('Steam load error: $e');
      setState(() => _isLoadingSteam = false);
    }
  }

  Future<void> _loadLastFmData() async {
    setState(() => _isLoadingLastFm = true);
    try {
      final user = await fetchLastFmUser();
      final artists = await fetchLastFmTopArtists();
      final tracks = await fetchLastFmRecentTracks();

      setState(() {
        _lastFmUser = user;
        _topArtists = artists;
        _recentTracks = tracks;
        _isLoadingLastFm = false;
      });
    } catch (e) {
      print('Last.fm load error: $e');
      setState(() => _isLoadingLastFm = false);
    }
  }

  Future<void> _loadTmdbData() async {
    setState(() => _isLoadingTmdb = true);
    try {
      final account = await TMDBService.fetchAccountDetails();
      final movies = await TMDBService.fetchRatedMovies();
      final shows = await TMDBService.fetchFavoriteTvShows();

      setState(() {
        _tmdbAccount = account;
        _ratedMovies = movies;
        _favoriteTvShows = shows;
        _isLoadingTmdb = false;
      });
    } catch (e) {
      print('TMDB load error: $e');
      setState(() => _isLoadingTmdb = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    final auth = ref.watch(authProvider);
    //final isLoggedIn = auth.isLoggedIn;
    final firstName = auth.firstName;

    return Scaffold(
      appBar: AppBar(title: Text('$firstName\'s Platform')),
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
          BottomNavigationBarItem(icon: Icon(Icons.movie), label: 'TMDB'),
          BottomNavigationBarItem(
            icon: Icon(Icons.music_note),
            label: 'Last.fm',
          ),
        ],
      ),
    );
  }

  Widget _buildBody() {
    switch (_selectedIndex) {
      case 0:
        return _isLoadingSteam ? _loading() : _buildSteamList();
      case 1:
        return _isLoadingTmdb ? _loading() : _buildTmdbSection();
      case 2:
        return _isLoadingLastFm ? _loading() : _buildLastFmSection();
      default:
        return Center(child: Text("Coming soon..."));
    }
  }

  Widget _loading() => const Center(child: CircularProgressIndicator());

  Widget _buildSteamList() {
    return ListView.builder(
      itemCount: _steamGames.length,
      itemBuilder: (context, index) {
        final game = _steamGames[index];
        return ListTile(
          title: Text(game.name),
          subtitle: Text('Played: ${game.playtimeForever} mins'),
        );
      },
    );
  }

  Widget _buildLastFmSection() {
    return ListView(
      padding: const EdgeInsets.all(12),
      children: [
        if (_lastFmUser != null) ...[
          Text(
            "User: ${_lastFmUser!.name}",
            style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
          ),
          Text("Plays: ${_lastFmUser!.playCount}"),
          const SizedBox(height: 12),
        ],
        Text(
          "Top Artists",
          style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
        ),
        ..._topArtists.map(
          (artist) => ListTile(
            title: Text(artist.name),
            subtitle: Text("Plays: ${artist.playCount}"),
          ),
        ),
        const SizedBox(height: 12),
        Text(
          "Recent Tracks",
          style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
        ),
        ..._recentTracks.map(
          (track) => ListTile(
            title: Text(track.name),
            subtitle: Text("By: ${track.artist}"),
            trailing: Text(track.formattedDate ?? "Now Playing"),
          ),
        ),
      ],
    );
  }

  Widget _buildTmdbSection() {
    return ListView(
      padding: const EdgeInsets.all(12),
      children: [
        if (_tmdbAccount != null) ...[
          Text(
            "User: ${_tmdbAccount!.username}",
            style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
          ),
          const SizedBox(height: 12),
        ],
        Text(
          "Rated Movies",
          style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
        ),
        ..._ratedMovies.map(
          (movie) => ListTile(
            title: Text(movie.title),
            subtitle: Text(movie.overview),
          ),
        ),
        const SizedBox(height: 12),
        Text(
          "Favorite TV Shows",
          style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
        ),
        ..._favoriteTvShows.map(
          (show) =>
              ListTile(title: Text(show.name), subtitle: Text(show.overview)),
        ),
      ],
    );
  }
}
