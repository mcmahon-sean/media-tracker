import 'package:flutter/material.dart';

class SearchAppBar extends StatelessWidget implements PreferredSizeWidget {
  final bool isSearching;
  final String firstName;
  final TextEditingController searchController;
  final VoidCallback onSearchToggle;
  final ValueChanged<String> onSearchQueryChanged;

  const SearchAppBar({
    super.key,
    required this.isSearching,
    required this.firstName,
    required this.searchController,
    required this.onSearchToggle,
    required this.onSearchQueryChanged,
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
      ],
    );
  }
}
