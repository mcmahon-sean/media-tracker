class MediaModel {
  final int mediaId;
  final int platformId;
  final int mediaTypeId;
  final String mediaPlatId;
  final String title;
  final String album;
  final String artist;

  MediaModel({
    required this.mediaId,
    required this.platformId,
    required this.mediaTypeId,
    required this.mediaPlatId,
    required this.title,
    required this.album,
    required this.artist,
  });

  // Convert MediaModel to Map for Supabase insertion
  Map<String, dynamic> toMap() {
    return {
      'MediaID': mediaId,
      'PlatformID': platformId,
      'MediaTypeID': mediaTypeId,
      'MediaPlatID': mediaPlatId,
      'Title': title,
      'Album': album,
      'Artist': artist,
    };
  }

  // Create a MediaModel from Supabase query result
  factory MediaModel.fromMap(Map<String, dynamic> map) {
    return MediaModel(
      mediaId: map['MediaID'] ?? 0,
      platformId: map['PlatformID'] ?? 0,
      mediaTypeId: map['MediaTypeID'] ?? 0,
      mediaPlatId: map['MediaPlatID'] ?? '',
      title: map['Title'] ?? '',
      album: map['Album'] ?? '',
      artist: map['Artist'] ?? '',
    );
  }
}
