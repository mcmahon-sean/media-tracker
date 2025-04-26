import 'package:flutter/material.dart';
import '../../models/lastfm/lastfm_top_artist.dart';
import '../../models/lastfm/lastfm_track.dart';
import '../../models/lastfm/lastfm_user.dart';
import '../../services/media_api/lastfm_service.dart';
import '../../screens/media/media_details_screen.dart';

class LastFmSection extends StatelessWidget {
  final LastFmUser? user;
  final List<TopArtist> topArtists;
  final List<LastFmTrack> recentTracks;

  const LastFmSection({
    super.key,
    required this.user,
    required this.topArtists,
    required this.recentTracks,
  });

  @override
  Widget build(BuildContext context) {
    return DefaultTabController(
      length: 2,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const TabBar(
            tabs: [Tab(text: 'Recent Tracks'), Tab(text: 'Top Artists')],
            indicatorColor: Colors.grey,
            labelColor: Colors.white,
            unselectedLabelColor: Colors.grey,
          ),
          Expanded(
            child: TabBarView(
              children: [
                _buildRecentTracksList(),
                _buildTopArtistsList(context),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildTopArtistsList(BuildContext context) {
    return ListView.builder(
      itemCount: topArtists.length,
      itemBuilder: (context, index) {
        final artist = topArtists[index];
        return ListTile(
          title: Text(artist.name),
          subtitle: Text("Plays: ${artist.playCount}"),
          trailing: IconButton(
            icon: Icon(
              artist.isFavorite ? Icons.star : Icons.star_border,
              color: artist.isFavorite ? Colors.yellow : Colors.grey,
            ),
            onPressed: () {
              // toggle favorite logic
            },
          ),
          onTap: () async {
            try {
              final details = await fetchArtistDetail(artist.name);

              Navigator.push(
                context,
                MaterialPageRoute(
                  builder:
                      (_) => MediaDetailsScreen(
                        appId: '',
                        title: details.name,
                        subtitle:
                            'Plays: ${details.playCount} • Listeners: ${details.listenerCount}',
                        mediaType: MediaType.lastfm,
                        genreIds: null,
                        releaseDate: '',
                        posterPath: details.imageUrl,
                      ),
                ),
              );
            } catch (e) {
              ScaffoldMessenger.of(context).showSnackBar(
                const SnackBar(content: Text('Failed to load artist details')),
              );
            }
          },
        );
      },
    );
  }

  Widget _buildRecentTracksList() {
    return ListView.builder(
      itemCount: recentTracks.length,
      itemBuilder: (context, index) {
        final track = recentTracks[index];
        return ListTile(
          title: Text(track.name),
          subtitle: Text("By: ${track.artist}"),
          trailing: Text(track.formattedDate ?? "Now Playing"),
          onTap: () async {
            try {
              // Fetch album info for this track
              final albumBasic = await fetchAlbumDetail(
                track.artist,
                track.name,
              );

              if (albumBasic['albumName'] == null) {
                ScaffoldMessenger.of(context).showSnackBar(
                  const SnackBar(
                    content: Text('Album info not found for this track.'),
                  ),
                );
                return;
              }

              // Fetch full album info
              final albumDetails = await fetchFullAlbumDetails(
                track.artist,
                albumBasic['albumName'],
              );

              Navigator.push(
                context,
                MaterialPageRoute(
                  builder:
                      (_) => MediaDetailsScreen(
                        appId: '', // Not needed
                        title: albumDetails['albumName'],
                        subtitle: 'Album by ${track.artist}',
                        mediaType: MediaType.lastfmAlbum,
                        genreIds: null,
                        releaseDate: '',
                        posterPath: albumDetails['albumImageUrl'],
                        artistName: track.artist,
                      ),
                ),
              );
            } catch (e) {
              ScaffoldMessenger.of(context).showSnackBar(
                const SnackBar(content: Text('Failed to load album details')),
              );
              print('Failed to load album from track tap: $e');
            }
          },
        );
      },
    );
  }
}
