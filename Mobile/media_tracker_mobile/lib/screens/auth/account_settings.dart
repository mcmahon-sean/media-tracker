import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:media_tracker_test/providers/auth_provider.dart';
import 'package:media_tracker_test/services/user_account_services.dart';

class AccountSettings extends ConsumerStatefulWidget {
  final String initialUsername;

  const AccountSettings({super.key, required this.initialUsername});

  @override
  ConsumerState<AccountSettings> createState() => _AccountSettingsScreenState();
}

class _AccountSettingsScreenState extends ConsumerState<AccountSettings> {
  final _formKey = GlobalKey<FormState>();

  final _firstNameController = TextEditingController();
  final _lastNameController = TextEditingController();
  final _emailController = TextEditingController();
  final _usernameController = TextEditingController();
  final _passwordController = TextEditingController();
  final _confirmPasswordController = TextEditingController();

  bool _isUsernameEditable = false;

  @override
  void initState() {
    super.initState();
    _usernameController.text = widget.initialUsername;
  }

  @override
  void dispose() {
    _firstNameController.dispose();
    _lastNameController.dispose();
    _emailController.dispose();
    _usernameController.dispose();
    _passwordController.dispose();
    _confirmPasswordController.dispose();
    super.dispose();
  }

  // Passes null if a field is left empty
  String? nullIfEmpty(String? value) {
    final trimmed = value?.trim();
    return (trimmed == null || trimmed.isEmpty) ? null : trimmed;
  }

  Future<void> _saveProfile() async {
    final password = nullIfEmpty(_passwordController.text);
    final confirmPassword = nullIfEmpty(_confirmPasswordController.text);

    if (password != null && password != confirmPassword) {
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(const SnackBar(content: Text('Passwords do not match')));
      return;
    }

    if (_formKey.currentState?.validate() ?? false) {
      final firstName = nullIfEmpty(_firstNameController.text);
      final lastName = nullIfEmpty(_lastNameController.text);
      final email = nullIfEmpty(_emailController.text);

      final success = await UserAccountServices().updateUserProfile(
        username: _usernameController.text.trim(),
        firstName: firstName,
        lastName: lastName,
        email: email,
        password: password,
      );

      if (success) {
        final currentAuth = ref.read(authProvider);

        // Update provider state with new first/last/email
        ref
            .read(authProvider.notifier)
            .updateUserInfo(
              firstName: firstName ?? currentAuth.firstName,
              lastName: lastName ?? currentAuth.lastName,
              email: email ?? currentAuth.email,
            );

        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Profile updated successfully')),
        );

        Navigator.pop(context);
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Failed to update profile')),
        );
      }
      print('First Name: ${_firstNameController.text}');
      print('Last Name: ${_lastNameController.text}');
      print('Email: ${_emailController.text}');
      print('Username: ${_usernameController.text}');
    }
  }

  Widget _buildTextField({
    required TextEditingController controller,
    required String label,
    String? hint,
    TextInputType keyboardType = TextInputType.text,
    bool isRequired = false,
    bool enabled = true,
    bool obscureText = false,
  }) {
    return TextFormField(
      controller: controller,
      keyboardType: keyboardType,
      enabled: enabled,
      obscureText: obscureText,
      decoration: InputDecoration(labelText: label, hintText: hint),
      validator:
          isRequired
              ? (value) {
                if (value == null || value.trim().isEmpty) {
                  return '$label is required';
                }
                return null;
              }
              : null,
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Profile')),
      body: SafeArea(
        child: Padding(
          padding: const EdgeInsets.all(16.0),
          child: Column(
            children: [
              Expanded(
                child: SingleChildScrollView(
                  padding: const EdgeInsets.only(
                    bottom: 24,
                  ), // Add breathing room
                  child: Form(
                    key: _formKey,
                    child: Column(
                      children: [
                        _buildTextField(
                          controller: _usernameController,
                          label: 'Username',
                          isRequired: true,
                          enabled: _isUsernameEditable,
                        ),
                        TextButton(
                          onPressed: () {
                            setState(() {
                              _isUsernameEditable = !_isUsernameEditable;
                            });
                          },
                          child: Text(
                            _isUsernameEditable
                                ? 'Cancel Change Username'
                                : 'Change Username',
                          ),
                        ),
                        const SizedBox(height: 16),
                        _buildTextField(
                          controller: _firstNameController,
                          label: 'First Name',
                          hint: 'Enter your first name',
                        ),
                        const SizedBox(height: 16),
                        _buildTextField(
                          controller: _lastNameController,
                          label: 'Last Name',
                          hint: 'Enter your last name',
                        ),
                        const SizedBox(height: 16),
                        _buildTextField(
                          controller: _emailController,
                          label: 'Email',
                          keyboardType: TextInputType.emailAddress,
                        ),
                        const SizedBox(height: 16),
                        _buildTextField(
                          controller: _passwordController,
                          label: 'New Password',
                          hint: '●●●●●●●',
                          keyboardType: TextInputType.visiblePassword,
                        ),
                        const SizedBox(height: 16),
                        _buildTextField(
                          controller: _confirmPasswordController,
                          label: 'Confirm Password',
                          hint: '●●●●●●●',
                          keyboardType: TextInputType.visiblePassword,
                        ),
                      ],
                    ),
                  ),
                ),
              ),
              SizedBox(
                width: double.infinity,
                child: ElevatedButton(
                  onPressed: _saveProfile,
                  child: const Text('Save'),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
