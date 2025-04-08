class FavoriteModel {
  final String username;
  final int mediaId;
  final bool isFavorite;

  FavoriteModel({
    required this.username,
    required this.mediaId,
    required this.isFavorite,
  });

  // Convert FavoriteModel to Map for Supabase insertion
  Map<String, dynamic> toMap() {
    return {
      'Username': username,
      'MediaID': mediaId,
      'Favorite': isFavorite,
    };
  }

  // Create a FavoriteModel from Supabase query result
  factory FavoriteModel.fromMap(Map<String, dynamic> map) {
    return FavoriteModel(
      username: map['Username'] ?? '',
      mediaId: map['MediaID'] ?? 0,
      isFavorite: map['Favorite'] ?? false,
    );
  }
}
