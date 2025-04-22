import 'package:flutter/material.dart';
import 'package:media_tracker_test/models/lastfm/lastfm_top_artist.dart';
import '../../models/lastfm/lastfm_track.dart';
import '../../models/lastfm/lastfm_user.dart';
import '../../screens/media/media_details_screen.dart';
import '../../services/media_api/lastfm_service.dart'; // Assuming fetchArtistDetail is here

Widget buildLastFmSection(
  LastFmUser? user,
  List<TopArtist> topArtists,
  List<LastFmTrack> recentTracks,
) {
  return Builder(
    builder:
        (context) => ListView(
          padding: const EdgeInsets.all(12),
          children: [
            if (user != null) ...[
              Text(
                "User: ${user.name}",
                style: const TextStyle(
                  fontSize: 18,
                  fontWeight: FontWeight.bold,
                ),
              ),
              Text("Plays: ${user.playCount}"),
              const SizedBox(height: 12),
            ],
            const Text(
              "Top Artists",
              style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
            ),
            ...topArtists.map(
              (artist) => ListTile(
                title: Text(artist.name),
                subtitle: Text("Plays: ${artist.playCount}"),
                onTap: () async {
                  try {
                    final details = await fetchArtistDetail(artist.name);

                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder:
                            (_) => MediaDetailsScreen(
                              appId: '', // Not needed for Last.fm
                              title: details.name,
                              subtitle:
                                  'Plays: ${details.playCount} • Listeners: ${details.listenerCount}',
                              mediaType: MediaType.lastfm,
                              genreIds:
                                  null, // Could convert tags to something meaningful
                              releaseDate: '',
                              posterPath: details.imageUrl,
                            ),
                      ),
                    );
                  } catch (e) {
                    ScaffoldMessenger.of(context).showSnackBar(
                      SnackBar(content: Text('Failed to load artist details')),
                    );
                  }
                },
              ),
            ),
            const SizedBox(height: 12),
            const Text(
              "Recent Tracks",
              style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
            ),
            ...recentTracks.map(
              (track) => ListTile(
                title: Text(track.name),
                subtitle: Text("By: ${track.artist}"),
                trailing: Text(track.formattedDate ?? "Now Playing"),
              ),
            ),
          ],
        ),
  );
}
