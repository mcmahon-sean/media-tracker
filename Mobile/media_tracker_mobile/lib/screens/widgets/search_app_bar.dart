import 'package:flutter/material.dart';
import 'package:media_tracker_test/models/sort_options.dart';

class SearchAppBar<T> extends StatelessWidget implements PreferredSizeWidget {
  final bool isSearching;
  final String firstName;
  final TextEditingController searchController;
  final VoidCallback onSearchToggle;
  final ValueChanged<String> onSearchQueryChanged;
  final SortOption currentSortOption;
  final SortDirection currentSortDirection;
  final ValueChanged<SortOption> onSortOptionChanged;
  final ValueChanged<SortDirection> onSortDirectionChanged;
  final String platformLabel;

  const SearchAppBar({
    super.key,
    required this.isSearching,
    required this.firstName,
    required this.searchController,
    required this.onSearchToggle,
    required this.onSearchQueryChanged,
    required this.currentSortOption,
    required this.currentSortDirection,
    required this.onSortDirectionChanged,
    required this.onSortOptionChanged,
    required this.platformLabel,
  });

  @override
  Size get preferredSize => const Size.fromHeight(kToolbarHeight);

  @override
  Widget build(BuildContext context) {
    return AppBar(
      title:
          !isSearching
              ? Text('$firstName\'s Media')
              : TextField(
                controller: searchController,
                autofocus: true,
                decoration: const InputDecoration(
                  hintText: 'Search...',
                  border: InputBorder.none,
                  focusedBorder: InputBorder.none,
                  enabledBorder: InputBorder.none,
                ),
                style: const TextStyle(color: Colors.white, fontSize: 18),
                onChanged: onSearchQueryChanged,
              ),
      actions: [
        IconButton(
          icon: Icon(isSearching ? Icons.close : Icons.search),
          color: Colors.grey,
          tooltip: isSearching ? 'Close' : 'Search',
          onPressed: onSearchToggle,
        ),
        IconButton(
          icon: Icon(
            currentSortDirection == SortDirection.asc
                ? Icons.arrow_upward
                : Icons.arrow_downward,
          ),
          tooltip:
              currentSortDirection == SortDirection.asc
                  ? 'Ascending'
                  : 'Descending',
          onPressed: () {
            onSortDirectionChanged(
              currentSortDirection == SortDirection.asc
                  ? SortDirection.desc
                  : SortDirection.asc,
            );
          },
        ),
        PopupMenuButton<SortOption>(
          icon: const Icon(Icons.sort),
          tooltip: 'Sort',
          onSelected: onSortOptionChanged,
          itemBuilder:
              (_) => [
                const PopupMenuItem(
                  value: SortOption.name,
                  child: Text('Name'),
                ),
                PopupMenuItem(
                  value: SortOption.playtime,
                  child: Text(_getPlaytimeLabel(platformLabel)),
                ),
                const PopupMenuItem(
                  value: SortOption.favorite,
                  child: Text('Favorites'),
                ),
              ],
        ),
      ],
    );
  }

  String _getPlaytimeLabel(String platform) {
    switch (platform.toLowerCase()) {
      case 'steam':
        return 'Playtime';
      case 'last.fm':
        return 'Play Count';
      case 'tmdb':
        return 'Release Date';
      default:
        return 'Playtime';
    }
  }
}
