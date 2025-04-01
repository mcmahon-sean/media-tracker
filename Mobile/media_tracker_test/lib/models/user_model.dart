class UserModel {
  final String username;
  final String firstName;
  final String lastName;
  final String email;
  final String password; // Will encrypt before storage, potentially using Supabase Auth instead

  UserModel({
    required this.username,
    required this.firstName,
    required this.lastName,
    required this.email,
    required this.password,
  });

  // Convert UserModel to Map for Supabase insertion
  Map<String, dynamic> toMap() {
    return {
      'username': username,
      'firstName': firstName,
      'lastName': lastName,
      'email': email,
      'password': password, // Store hashed password in DB
    };
  }

  // Create a UserModel from Supabase query result
  factory UserModel.fromMap(Map<String, dynamic> map) {
    return UserModel(
      username: map['username'] ?? '',
      firstName: map['firstName'] ?? '',
      lastName: map['lastName'] ?? '',
      email: map['email'] ?? '',
      password: map['password'] ?? '',
    );
  }
}
