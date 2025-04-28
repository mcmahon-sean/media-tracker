class TMDBTvShow {
  final int id;
  final String title;
  final String overview;
  final List<int> genreIds;
  final String releaseDate;
  final String posterPath;
  bool isFavorite;

  TMDBTvShow({
    required this.id,
    required this.title,
    required this.overview,
    required this.genreIds,
    required this.releaseDate,
    required this.posterPath,
    this.isFavorite = false,
  });

  factory TMDBTvShow.fromJson(Map<String, dynamic> json) {
    return TMDBTvShow(
      id: json['id'],
      title: json['name'],
      overview: json['overview'],
      genreIds: List<int>.from(json['genre_ids']),
      releaseDate: json['first_air_date'] ?? 'Unknown',
      posterPath: json['poster_path'] ?? '',
      isFavorite: false,
    );
  }
}
