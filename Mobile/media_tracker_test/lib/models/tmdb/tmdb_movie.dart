class TMDBMovie {
  final int id;
  final String title;
  final String overview;

  TMDBMovie({required this.id, required this.title, required this.overview});

  factory TMDBMovie.fromJson(Map<String, dynamic> json) {
    return TMDBMovie(
      id: json['id'],
      title: json['title'],
      overview: json['overview'],
    );
  }
}
