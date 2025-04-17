import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:media_tracker_test/models/theme.dart';

class LinkAccountCard extends ConsumerStatefulWidget {
  final String platformName;
  final String? linkedValue;
  final void Function(String) onLink;
  final VoidCallback onUnlink;
  final VoidCallback onEdit;

  const LinkAccountCard({
    super.key,
    required this.platformName,
    required this.linkedValue,
    required this.onLink,
    required this.onUnlink,
    required this.onEdit,
  });

  @override
  ConsumerState<LinkAccountCard> createState() => _LinkAccountCardState();
}

class _LinkAccountCardState extends ConsumerState<LinkAccountCard> {
  final _controller = TextEditingController();
  bool _showCredential = false;
  bool _editMode = false;

  @override
  void initState() {
    super.initState();
    _controller.text = widget.linkedValue ?? '';
  }

  @override
  void didUpdateWidget(covariant LinkAccountCard oldWidget) {
    super.didUpdateWidget(oldWidget);
    // Case 1: The linkedValue was cleared externally (e.g., user unlinked account)
    // This ensures the UI resets to the "link" state
    // _editMode is turned off to avoid showing the edit field
    // _showCredential is turned off so no masked value shows
    // The controller is cleared so the input field is blank
    if (oldWidget.linkedValue != widget.linkedValue &&
        widget.linkedValue == null) {
      setState(() {
        _editMode = false;
        _showCredential = false;
        _controller.clear();
      });
    }

    // Case 2: A new value was linked externally or updated
    // This ensures the controller reflects the updated linkedValue
    // Only updates if the user is not currently editing (_editMode == false)
    // Prevents overwriting any in-progress edits
    if (oldWidget.linkedValue != widget.linkedValue &&
        widget.linkedValue != null &&
        !_editMode) {
      _controller.text = widget.linkedValue!;
    }
  }

  @override
  Widget build(BuildContext context) {
    print('[${widget.platformName}] linkedValue: ${widget.linkedValue}');
    return SizedBox(
      width: double.infinity, // Ensures full width
      child: Card(
        color: const Color.fromARGB(255, 72, 72, 72),
        margin: const EdgeInsets.symmetric(vertical: 8, horizontal: 16),
        elevation: 4,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
        child: Padding(
          padding: const EdgeInsets.all(16),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                widget.platformName,
                style: Theme.of(
                  context,
                ).textTheme.titleLarge?.copyWith(color: Colors.white),
              ),
              const SizedBox(height: 12),
              AnimatedCrossFade(
                crossFadeState:
                    (widget.linkedValue == null || _editMode)
                        ? CrossFadeState.showFirst
                        : CrossFadeState.showSecond,
                duration: const Duration(milliseconds: 200),
                firstChild: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    TextField(
                      controller: _controller,
                      style: const TextStyle(color: Colors.white),
                      decoration: InputDecoration(
                        labelText: 'Enter ${widget.platformName} ID',
                        labelStyle: const TextStyle(color: Colors.white70),
                        border: const OutlineInputBorder(),
                      ),
                    ),
                    const SizedBox(height: 12),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.end,
                      children: [
                        ElevatedButton.icon(
                          style: ElevatedButton.styleFrom(
                            backgroundColor: colorPrimary,
                          ),
                          icon: const Icon(Icons.link),
                          label: const Text('Link'),
                          onPressed: () {
                            final input = _controller.text.trim();
                            if (input.isNotEmpty) {
                              widget.onLink(input);
                              setState(() {
                                _editMode = false;
                                _showCredential = false;
                              });
                            }
                          },
                        ),
                      ],
                    ),
                  ],
                ),
                secondChild: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      _showCredential
                          ? widget.linkedValue!
                          : '••••••••••••••••',
                      style: const TextStyle(fontSize: 16, color: Colors.white),
                    ),
                    const SizedBox(height: 8),
                    Center(
                      child: Row(
                        mainAxisSize: MainAxisSize.min,
                        children: [
                          TextButton.icon(
                            icon: Icon(
                              _showCredential
                                  ? Icons.visibility_off
                                  : Icons.visibility,
                            ),
                            label: Text(_showCredential ? 'Hide' : 'Reveal'),
                            onPressed: () {
                              setState(() {
                                _showCredential = !_showCredential;
                              });
                            },
                            style: TextButton.styleFrom(
                              foregroundColor: Colors.black,
                            ),
                          ),
                          TextButton.icon(
                            icon: const Icon(Icons.edit),
                            label: const Text('Edit'),
                            onPressed: () {
                              _controller.text = widget.linkedValue!;
                              setState(() {
                                _editMode = true;
                              });
                              widget.onEdit();
                            },
                            style: TextButton.styleFrom(
                              foregroundColor: Colors.black,
                            ),
                          ),
                          TextButton.icon(
                            icon: const Icon(Icons.delete),
                            label: const Text('Unlink'),
                            onPressed: () {
                              widget
                                  .onUnlink(); // properly calls external clear logic
                              setState(() {
                                _showCredential = false;
                                _editMode = false;
                                _controller.clear();
                              });
                            },
                            style: TextButton.styleFrom(
                              foregroundColor: Colors.red,
                            ),
                          ),
                        ],
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
