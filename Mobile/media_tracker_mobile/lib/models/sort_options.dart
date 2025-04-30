// Defines the sorting direction
enum SortDirection {
  asc,
  desc,
}

// Defines the sorting criteria the user can choose
enum SortOption {
  name,
  playtime, // Sort by playtime, play count, or release date (platform-specific)
  favorite,
}

// Applies generic sorting logic to a list of any type T
List<T> applySorting<T>({
  required List<T> list,
  required SortOption option,
  required SortDirection direction,
  required Comparable Function(T item) getField,
  bool Function(T item)? isFavorite,
}) {
  // Make a copy of the list to avoid mutating the original
  final sorted = List<T>.from(list);

  if (option == SortOption.favorite && isFavorite != null) {
    // Sort favorites to the top
    sorted.sort((a, b) {
      final aFav = isFavorite(a); 
      final bFav = isFavorite(b); 

      if (aFav && !bFav) return -1; // a is favorite, b is not
      if (!aFav && bFav) return 1; // b is favorite, a is not
      return 0; // Both are favorites or both are not, leave unchanged
    });
  } else {
    // Sort by field (name, playtime, etc.)
    sorted.sort((a, b) {
      final aField = getField(a);
      final bField = getField(b);

      return direction == SortDirection.asc
          ? aField.compareTo(bField) // ascending
          : bField.compareTo(aField); // descending
    });
  }

  return sorted;
}
