// This file will house all the write-to-database functions for the user (update account details/third party credentials)
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
}
