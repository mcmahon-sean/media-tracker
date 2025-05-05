import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:media_tracker_test/config/api_connections.dart';
import 'package:supabase_flutter/supabase_flutter.dart';

class UserAccountServices {
  final supabase = Supabase.instance.client;

  // Function to save/update third party credentials to the DB
  Future<bool> savePlatformCredentials({
    required String username,
    required int platformId,
    required String userPlatformId,
  }) async {
    try {
      // Call the PostgreSQL RPC to commit/update the 3rd party service credentials
      if (platformId <= 0 ||
          username.trim().isEmpty ||
          userPlatformId.trim().isEmpty)
        return false;

      final cleanedUsername = username.trim();
      final cleanedPlatformId = platformId;
      final cleanedUserPlatformId = userPlatformId.trim();

      await supabase.rpc(
        'add_3rd_party_id',
        params: {
          'username_input': cleanedUsername,
          'platform_id_input': cleanedPlatformId,
          'user_plat_id_input': cleanedUserPlatformId,
        },
      );
      return true;
    } catch (e) {
      print('Failed to link platform ID: $e');
      return false;
    }
  }

  Future<bool> removePlatformCredentials({
    required String username,
    required int platformId,
    required String userPlatformId,
  }) async {
    try {
      // Call the PostgreSQL RPC to remove 3rd party service credentials
      if (platformId <= 0 ||
          username.trim().isEmpty ||
          userPlatformId.trim().isEmpty)
        return false;

      final cleanedUsername = username.trim();
      final cleanedUserPlatformId = userPlatformId.trim();

      await supabase.rpc(
        'delete_3rd_party',
        params: {
          'username_input': cleanedUsername,
          'platform_id_input': platformId,
          'user_plat_id_input': cleanedUserPlatformId,
        },
      );
      return true;
    } catch (e) {
      print('Failed to remove 3rd party credentials: $e');
      return false;
    }
  }

  // Function to fetch Steam ID from a Vanity username
  Future<String> fetchSteamIDFromVanity(String username) async {
    try {
      final isSteamId = RegExp(r'^\d{17}$').hasMatch(username);
      // If input looks like a 64-bit numeric Steam ID, return it directly
      if (isSteamId) return username;

      // Otherwise, treat it as a vanity URL and resolve it using Steam Web API
      final response = await http.get(
        Uri.parse(
          'http://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001/?key=${ApiServices.steamApiKey}&vanityurl=$username',
        ),
      );

      // If resolution was successful, return the resolved Steam ID
      if (response.statusCode == 200) {
        final data = json.decode(response.body);
        if (data['response']['success'] == 1) {
          return data['response']['steamid'];
        }
      }
    } catch (e) {
      print('Failed to resolve Steam ID: $e');
    }
    return '';
  }

  // Call stored procedure to favorite a media item
  Future<bool> toggleFavoriteMedia({
    required int platformId,
    required int mediaTypeId,
    required String mediaPlatId,
    required String title,
    String? album,
    String? artist,
    required String username,
  }) async {
    try {
      if (platformId <= 0 || mediaTypeId <= 0) return false;
      if (mediaPlatId.trim().isEmpty ||
          title.trim().isEmpty ||
          username.trim().isEmpty)
        return false;

      final cleanedMediaPlatId = mediaPlatId.trim();
      final cleanedTitle = title.trim();
      final cleanedAlbum = (album ?? '').trim();
      final cleanedArtist = (artist ?? '').trim();
      final cleanedUsername = username.trim();

      await supabase.rpc(
        'initial_media_fav',
        params: {
          'platform_id_input': platformId,
          'media_type_id_input': mediaTypeId,
          'media_plat_id_input': cleanedMediaPlatId,
          'title_input': cleanedTitle,
          'album_input': cleanedAlbum,
          'artist_input': cleanedArtist,
          'username_input': cleanedUsername,
        },
      );

      return true;
    } catch (e) {
      print('Failed to favorite media: $e');
      return false;
    }
  }

  // Fetch all favorited media IDs for a given user
  Future<List<Map<String, dynamic>>> fetchUserFavorites(String username) async {
    try {
      final cleanedUsername = username.trim();
      final response = await supabase
          .from('userfavorites')
          .select(
            'media_id, favorites, media (platform_id, media_type_id, media_plat_id, title, album, artist)',
          )
          .eq('username', cleanedUsername);

      if (response.isEmpty) return [];
      return List<Map<String, dynamic>>.from(response);
    } catch (e) {
      print('Error fetching user favorites: $e');
      return [];
    }
  }

  Future<bool> updateUserProfile({
    required String username,
    String? firstName,
    String? lastName,
    String? email,
    String? password,
  }) async {
    try {
      await supabase.rpc(
        'update_user',
        params: {
          'username_input': username,
          'first_name_input': firstName,
          'last_name_input': lastName,
          'email_input': email,
          'password_input': password,
        },
      );
      return true;
    } catch (e) {
      print('Exception during updateUserProfile: $e');
      return false;
    }
  }

  // Function to delete a user
  Future<bool> deleteUser(String username) async {
    try {
      final result = await supabase.rpc(
        'delete_user',
        params: {'username_input': username},
      );
      return result != null;
    } catch (e) {
      print('Error deleting user: $e');
      return false;
    }
  }
}
