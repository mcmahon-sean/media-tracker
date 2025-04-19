import 'package:flutter/material.dart';
import '../models/steam/steam_model.dart';
import '../models/tmdb/tmdb_movie.dart';
import '../models/tmdb/tmdb_tv_show.dart';
import '../models/lastfm/lastfm_track.dart';
import '../models/lastfm/lastfm_artist.dart';

class MediaDetailsScreen extends StatelessWidget {
  final dynamic mediaItem;

  const MediaDetailsScreen({super.key, required this.mediaItem});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Media Details'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: _buildDetails(context),

      ),
    );
  }

  Widget _buildDetails(BuildContext context) {
    final textTheme = Theme.of(context).textTheme;

    if (mediaItem is SteamGame) {
      final game = mediaItem as SteamGame;
      return Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          RichText(
            text: TextSpan(
              style: textTheme.bodyLarge,
              children: [
                const TextSpan(
                    text: "Game: ",
                    style: TextStyle(fontWeight: FontWeight.bold)),
                TextSpan(text: game.name),
              ],
            ),
          ),
          const SizedBox(height: 10),
          RichText(
            text: TextSpan(
              style: textTheme.bodyMedium,
              children: [
                const TextSpan(
                    text: "Playtime: ",
                    style: TextStyle(fontWeight: FontWeight.bold)),
                TextSpan(text: "${game.playtimeForever} mins"),
              ],
            ),
          ),
        ],
      );
    } else if (mediaItem is TMDBMovie) {
      final movie = mediaItem as TMDBMovie;
      return Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          RichText(
            text: TextSpan(
              style: textTheme.bodyLarge,
              children: [
                const TextSpan(
                    text: "Movie: ",
                    style: TextStyle(fontWeight: FontWeight.bold)),
                TextSpan(text: movie.title),
              ],
            ),
          ),
          const SizedBox(height: 10),
          RichText(
            text: TextSpan(
              style: textTheme.bodyMedium,
              children: [
                const TextSpan(
                    text: "Overview: ",
                    style: TextStyle(fontWeight: FontWeight.bold)),
                TextSpan(text: movie.overview),
              ],
            ),
          ),
        ],
      );
    } else if (mediaItem is TMDBTvShow) {
      final show = mediaItem as TMDBTvShow;
      return Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          RichText(
            text: TextSpan(
              style: textTheme.bodyLarge,
              children: [
                const TextSpan(
                    text: "TV Show: ",
                    style: TextStyle(fontWeight: FontWeight.bold)),
                TextSpan(text: show.name),
              ],
            ),
          ),
          const SizedBox(height: 10),
          RichText(
            text: TextSpan(
              style: textTheme.bodyMedium,
              children: [
                const TextSpan(
                    text: "Overview: ",
                    style: TextStyle(fontWeight: FontWeight.bold)),
                TextSpan(text: show.overview),
              ],
            ),
          ),
        ],
      );
    } else if (mediaItem is LastFmTrack) {
      final track = mediaItem as LastFmTrack;
      return Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          RichText(
            text: TextSpan(
              style: textTheme.bodyLarge,
              children: [
                const TextSpan(
                    text: "Track: ",
                    style: TextStyle(fontWeight: FontWeight.bold)),
                TextSpan(text: track.name),


              ],
            ),
          ),
          const SizedBox(height: 10),
          RichText(
            text: TextSpan(
              style: textTheme.bodyMedium,
              children: [
                const TextSpan(
                    text: "Artist: ",
                    style: TextStyle(fontWeight: FontWeight.bold)),
                TextSpan(text: track.artist),
              ],
            ),
          ),
          RichText(
            text: TextSpan(
              style: textTheme.bodyMedium,
              children: [
                const TextSpan(
                    text: "Played: ",
                    style: TextStyle(fontWeight: FontWeight.bold)),
                TextSpan(text: track.formattedDate ?? 'Now Playing'),
              ],
            ),
          ),
        ],
      );
    } else if (mediaItem is LastFmArtist) {
      final artist = mediaItem as LastFmArtist;
      return Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          RichText(
            text: TextSpan(
              style: textTheme.bodyLarge,
              children: [
                const TextSpan(
                    text: "Artist: ",
                    style: TextStyle(fontWeight: FontWeight.bold)),
                TextSpan(text: artist.name),
              ],
            ),
          ),

          
          const SizedBox(height: 10),
          RichText(
            text: TextSpan(
              style: textTheme.bodyMedium,
              children: [
                const TextSpan(
                    text: "Play Count: ",
                    style: TextStyle(fontWeight: FontWeight.bold)),
                TextSpan(text: artist.playCount.toString()),
              ],
            ),
          ),
        ],
      );
    } else {
      return const Text("Unsupported media type.");
    }
  }
}
