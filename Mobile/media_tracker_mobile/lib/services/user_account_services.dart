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
      print('Successful save or updating of data.');
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
      await supabase.rpc(
        'delete_3rd_party',
        params: {
          'username_input': username,
          'platform_id_input': platformId,
          'user_plat_id_input': userPlatformId,
        },
      );
      print('Successfully removed 3rd party credentials');
      return true;
    } catch (e) {
      print('Failed to remove 3rd party credentials: $e');
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
          print('Vanity URL not found: ${data['response']['message']}');
        }
      } else {
        print('Failed to fetch Steam ID: HTTP ${response.statusCode}');
      }
    } catch (e) {
      print('Failed to resolve Steam ID from Vanity username: $e');
    }
    return '';
  }
}
