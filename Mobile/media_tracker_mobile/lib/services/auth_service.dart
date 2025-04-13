import 'package:media_tracker_test/providers/auth_provider.dart';
import 'package:supabase_flutter/supabase_flutter.dart';
import '../config/api_connections.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

// Registering the AuthService as a Riverpod provider
// This allows us to inject the Ref object, which is needed to interact with other providers like authProvider
final authServiceProvider = Provider<AuthService>((ref) {
  return AuthService(ref);
});

// AuthService handles login and registration logic using Supabase
class AuthService {
  final supabase = Supabase.instance.client;
  final Ref
  ref; // Reference to Riverpod's Ref, used to interact with other providers
  AuthService(this.ref); // Constructor accepting the ref object from Riverpod

  Future<String?> _getPlatformID(String username, int platformId) async {
    try {
      final response =
          await Supabase.instance.client
              .from('useraccounts')
              .select('user_platform_id')
              .eq('username', username)
              .eq('platform_id', platformId)
              .maybeSingle();

      return response!['user_platform_id'].toString();
    } catch (e) {
      print('Error fetching platform ID for $platformId: $e');
      return null;
    }
  }

  // Logs in a user by calling a stored procedure and loading their profile info
  Future<bool> login(String username, String password) async {
    try {
      // Call the PostgreSQL RPC to authenticate and get a token
      final result = await supabase.rpc(
        'auth_user',
        params: {'username_input': username, 'password_input': password},
      );

      // If token is null or empty, login failed
      if (result == null || result is! String || result.isEmpty) {
        return false;
      }

      // Fetch user's basic profile information (first name, last name, email)
      final userInfo = await supabase
          .from('users')
          .select('first_name, last_name, email')
          .eq('username', username);

      // Fetch linked platform IDs (Steam, Last.fm, TMDB) from the useraccounts table
      // If the Steam ID, last.fm ID, or TMDB ID exists, assign them globally via ApiServices
      final steamID = await _getPlatformID(username, 1);
      if (steamID != null && steamID.isNotEmpty) {
        ApiServices.steamUserId = steamID;
      }
      final lastfmID = await _getPlatformID(username, 2);
      if (lastfmID != null && lastfmID.isNotEmpty) {
        ApiServices.lastFmUser = lastfmID;
      }
      final tmdbID = await _getPlatformID(username, 3);
      if (tmdbID != null && tmdbID.isNotEmpty) {
        ApiServices.tmdbUser = tmdbID;
      }

      // If user profile data was found, update the global auth state using authProvider
      if (userInfo.isNotEmpty) {
        ref
            .read(authProvider.notifier)
            .login(
              username: username,
              firstName: userInfo[0]['first_name'] ?? '',
              lastName: userInfo[0]['last_name'] ?? '',
              email: userInfo[0]['email'] ?? '',
              token: result,
            );
      }
      return true;
    } catch (e) {
      print('Login failed: $e');
      return false;
    }
  }

  // Registers a new user using a stored procedure
  Future<bool> register({
    required String username,
    required String password,
    required String firstName,
    required String lastName,
    required String email,
  }) async {
    // Call the CreateUser stored procedure with the user's registration data
    final result = await supabase.rpc(
      'CreateUser',
      params: {
        'usernamevar': username,
        'firstnamevar': firstName,
        'lastnamevar': lastName,
        'emailvar': email,
        'passwordvar': password,
      },
    );

    return result == true;
  }
}
