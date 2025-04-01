class TMDBAccount {
  final String username;
  final int id;

  TMDBAccount({required this.username, required this.id});

  factory TMDBAccount.fromJson(Map<String, dynamic> json) {
    return TMDBAccount(
      username: json['username'],
      id: json['id'],
    );
  }
}
