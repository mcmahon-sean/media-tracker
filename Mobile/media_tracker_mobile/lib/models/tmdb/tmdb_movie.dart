class TMDBMovie {
  final int id;
  final String title;
  final String overview;
  final List<int> genreIds;
  final String releaseDate;
  final String posterPath;

  TMDBMovie({
    required this.id,
    required this.title,
    required this.overview,
    required this.genreIds,
    required this.releaseDate,
    required this.posterPath,
  });

  factory TMDBMovie.fromJson(Map<String, dynamic> json) {
    return TMDBMovie(
      id: json['id'],
      title: json['title'],
      overview: json['overview'],
      genreIds: List<int>.from(json['genre_ids']),
      releaseDate: json['release_date'] ?? 'Unknown',
      posterPath: json['poster_path'] ?? '',
    );
  }

  bool get isFavorite => false;
}
