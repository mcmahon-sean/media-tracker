import 'package:flutter/material.dart';
import '../../models/lastfm/lastfm_artist.dart';
import '../../models/lastfm/lastfm_track.dart';
import '../../models/lastfm/lastfm_user.dart';

Widget buildLastFmSection(
  LastFmUser? user,
  List<LastFmArtist> topArtists,
  List<LastFmTrack> recentTracks,
) {
  return ListView(
    padding: const EdgeInsets.all(12),
    children: [
      if (user != null) ...[
        Text(
          "User: ${user.name}",
          style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
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
  );
}
