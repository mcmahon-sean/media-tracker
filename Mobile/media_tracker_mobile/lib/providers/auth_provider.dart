import 'package:flutter_riverpod/flutter_riverpod.dart';

// Currently have the token information commented out until we can get a token returned from the DB

// Represents the authenticated user's state
class AuthState {
  final String? username;
  final String? firstName;
  final String? lastName;
  final String? email;
  //final String? token;

  // A computed property that returns true if the user is logged in
  bool get isLoggedIn => username != null; // && token != null;

  // Constructor for creating an AuthState instance with optional fields
  const AuthState({
    this.username,
    this.firstName,
    this.lastName,
    this.email,
    //this.token,
  });

  // Creates a new AuthState based on the current state,
  // replacing any non-null provided values
  AuthState copyWith({
    String? username,
    String? firstName,
    String? lastName,
    String? email,
    //String? token,
  }) {
    return AuthState(
      username: username ?? this.username,
      firstName: firstName ?? this.firstName,
      lastName: lastName ?? this.lastName,
      email: email ?? this.email,
      //token: token ?? this.token,
    );
  }

  // Factory constructor to return an empty (logged-out) state
  factory AuthState.empty() => const AuthState();
}

// A StateNotifier that manages the authentication state (AuthState)
class AuthNotifier extends StateNotifier<AuthState> {
  // Starts with an empty state (user is logged out)
  AuthNotifier() : super(AuthState.empty());

  // Updates the state with user login information
  void login({
    required String username,
    required String firstName,
    required String lastName,
    required String email,
    //required String token,
  }) {
    state = AuthState(
      username: username,
      firstName: firstName,
      lastName: lastName,
      email: email,
      //token: token,
    );
  }

  // Clears the state, effectively logging the user out
  void logout() {
    state = AuthState.empty();
  }
}

// Riverpod provider for accessing and modifying the authentication state
final authProvider = StateNotifierProvider<AuthNotifier, AuthState>((ref) {
  return AuthNotifier();
});
