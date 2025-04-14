import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:media_tracker_test/config/api_connections.dart';

// Represents the authenticated user's state
class AuthState {
  final String? username;
  final String? firstName;
  final String? lastName;
  final String? email;
  final String? token;
  final String? steamId;
  final String? tmdbSessionId;
  final String? lastFmUsername;

  // A computed property that returns true if the user is logged in
  bool get isLoggedIn => username != null && token != null;

  // A computed property that returns true if the user has linked third party API services
  bool get anyMediaLinked => steamId != null || tmdbSessionId != null || lastFmUsername != null;

  // Constructor for creating an AuthState instance with optional fields
  const AuthState({
    this.username,
    this.firstName,
    this.lastName,
    this.email,
    this.token,
    this.steamId,
    this.tmdbSessionId,
    this.lastFmUsername,
  });

  // Creates a new AuthState based on the current state,
  // replacing any non-null provided values
  AuthState copyWith({
    String? username,
    String? firstName,
    String? lastName,
    String? email,
    String? token,
    String? steamId,
    String? tmdbSessionId,
    String? lastFmUsername,
  }) {
    return AuthState(
      username: username ?? this.username,
      firstName: firstName ?? this.firstName,
      lastName: lastName ?? this.lastName,
      email: email ?? this.email,
      token: token ?? this.token,
      steamId: steamId ?? this.steamId,
      tmdbSessionId: tmdbSessionId ?? this.tmdbSessionId,
      lastFmUsername: lastFmUsername ?? this.lastFmUsername
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
    required String token,
    String? steamID,
    String? tmdbSessionId,
    String? lastFmUsername
  }) {
    state = AuthState(
      username: username,
      firstName: firstName,
      lastName: lastName,
      email: email,
      token: token,
      steamId: steamID,
      tmdbSessionId: tmdbSessionId,
      lastFmUsername: lastFmUsername
    );
  }

  // Clears the state, effectively logging the user out
  void logout() {
    // Clear the auth state
    state = AuthState.empty();

    // Clear any global third-party API user ids
    ApiServices.steamUserId = "";
    ApiServices.lastFmUser = "";
    ApiServices.tmdbUser = "";
  }
}

// Riverpod provider for accessing and modifying the authentication state
final authProvider = StateNotifierProvider<AuthNotifier, AuthState>((ref) {
  return AuthNotifier();
});
