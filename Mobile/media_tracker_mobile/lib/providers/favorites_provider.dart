import 'package:flutter_riverpod/flutter_riverpod.dart';

// Riverpod provider for managing user favorites
final favoritesProvider = StateProvider<List<Map<String, dynamic>>>((ref) => []);
