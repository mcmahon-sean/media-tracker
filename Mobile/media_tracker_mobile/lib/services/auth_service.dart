import 'package:supabase_flutter/supabase_flutter.dart';
import 'package:bcrypt/bcrypt.dart';
import '../config/api_connections.dart';

class AuthService {
  final supabase = Supabase.instance.client;

  Future<bool> login(String username, String password) async {
    final result = await supabase.rpc(
      'AuthenticateUser',
      params: {'usernamevar': username, 'passwordvar': password},
    );

    if (result == true) {
      final steamID =
          await supabase
              .from('useraccounts')
              .select('user_platform_id')
              .eq('username', username)
              .eq('platform_id', 1)
              .single();
      final lastfmID =
          await supabase
              .from('useraccounts')
              .select('user_platform_id')
              .eq('username', username)
              .eq('platform_id', 2)
              .single();
      final imdbID =
          await supabase
              .from('useraccounts')
              .select('user_platform_id')
              .eq('username', username)
              .eq('platform_id', 3)
              .single();

      ApiServices.steamUserId = steamID['user_platform_id'].toString();
      ApiServices.lastFmUser = lastfmID['user_platform_id'].toString();
      ApiServices.tmdbUser = imdbID['user_platform_id'].toString();

      return true;
    }

    return false;
  }

  Future<bool> register({
    required String username,
    required String password,
    required String firstName,
    required String lastName,
    required String email,
  }) async {
    final hashedPassword = BCrypt.hashpw(password, BCrypt.gensalt());
    final updatedHash = hashedPassword.replaceFirst('\$2a\$', '\$2b\$');

    final result = await supabase.rpc(
      'CreateUser',
      params: {
        'usernamevar': username,
        'firstnamevar': firstName,
        'lastnamevar': lastName,
        'emailvar': email,
        'passwordvar': updatedHash,
      },
    );

    return result == true;
  }
}