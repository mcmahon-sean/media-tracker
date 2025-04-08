import 'package:flutter_riverpod/flutter_riverpod.dart';


class AuthState {
  final String? username;
  final String? firstName;
  final String? lastName;
  final String? email;

  bool get isLoggedIn => username != null;

  const AuthState({
    this.username,
    this.firstName,
    this.lastName,
    this.email,
  });

  AuthState copyWith({
    String? username,
    String? firstName,
    String? lastName,
    String? email,
  }) {
    return AuthState(
      username: username ?? this.username,
      firstName: firstName ?? this.firstName,
      lastName: lastName ?? this.lastName,
      email: email ?? this.email,
    );
  }

  factory AuthState.empty() => const AuthState();
}

class AuthNotifier extends StateNotifier<AuthState> {
  AuthNotifier() : super(AuthState.empty());

  void login({
    required String username,
    required String firstName,
    required String lastName,
    required String email,
  }) {
    state = AuthState(
      username: username,
      firstName: firstName,
      lastName: lastName,
      email: email,
    );
  }

  void logout() {
    state = AuthState.empty();
  }
}

final authProvider = StateNotifierProvider<AuthNotifier, AuthState>((ref) {
  return AuthNotifier();
});
