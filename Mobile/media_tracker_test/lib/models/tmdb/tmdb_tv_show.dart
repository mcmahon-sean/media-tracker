class TMDBTvShow {
  final int id;
  final String name;
  final String overview;

  TMDBTvShow({required this.id, required this.name, required this.overview});

  factory TMDBTvShow.fromJson(Map<String, dynamic> json) {
    return TMDBTvShow(
      id: json['id'],
      name: json['name'],
      overview: json['overview'],
    );
  }
}
