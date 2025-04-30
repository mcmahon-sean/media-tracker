// This file will house all the write-to-database functions for the user (update account details/third party credentials)
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
      await supabase.rpc(
        'add_3rd_party_id',
        params: {
          'username_input': username,
          'platform_id_input': platformId,
          'user_plat_id_input': userPlatformId,
        },
      );
      print('Successful save or updating of data.'); // DEBUGGING
      return true;
    } catch (e) {
      print('Failed to link platform ID: $e'); // DEBUGGING
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
      await supabase.rpc(
        'delete_3rd_party',
        params: {
          'username_input': username,
          'platform_id_input': platformId,
          'user_plat_id_input': userPlatformId,
        },
      );
      print('Successfully removed 3rd party credentials'); // DEBUGGING
      return true;
    } catch (e) {
      print('Failed to remove 3rd party credentials: $e'); // DEBUGGING
      return false;
    }
  }

  // Function to fetch Steam ID from a Vanity username
  Future<String> fetchSteamIDFromVanity(String username) async {
    try {
      // If input looks like a 64-bit numeric Steam ID, return it directly
      final isSteamId = RegExp(r'^\d{17}$').hasMatch(username);
      if (isSteamId) {
        return username;
      }

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
        } else {
          print(
            'Vanity URL not found: ${data['response']['message']}',
          ); // DEBUGGING
        }
      } else {
        print(
          'Failed to fetch Steam ID: HTTP ${response.statusCode}',
        ); // DEBUGGING
      }
    } catch (e) {
      print('Failed to resolve Steam ID from Vanity username: $e'); // DEBUGGING
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
      await supabase.rpc(
        'initial_media_fav',
        params: {
          'platform_id_input': platformId,
          'media_type_id_input': mediaTypeId,
          'media_plat_id_input': mediaPlatId,
          'title_input': title,
          'album_input': album ?? '',
          'artist_input': artist ?? '',
          'username_input': username,
        },
      );
      print('Toggled favorite for $mediaPlatId ($title)'); // DEBUGGING
      print('Success favoriting media'); // DEBUGGING
      return true; // Success
    } catch (e) {
      print('Failed to favorite media: $e'); // DEBUGGING
      return false;
    }
  }

  // Fetch all favorited media IDs for a given user
  Future<List<Map<String, dynamic>>> fetchUserFavorites(String username) async {
    try {
      final response = await supabase
          .from('userfavorites')
          .select(
            'media_id, favorites, media (platform_id, media_type_id, media_plat_id, title, album, artist)',
          )
          .eq('username', username);

      if (response.isEmpty) {
        print('Fetched favorites: (empty)'); // DEBUGGING
        return [];
      } else {
        final favorites = List<Map<String, dynamic>>.from(response);
        return favorites;
      }
    } catch (e) {
      print('Error fetching user favorites: $e'); // DEBUGGING
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
      print('Successfully updated user information.');
      return true;
    } catch (e) {
      print('Exception during updateUserProfile: $e');
      return false;
    }
  }
}
