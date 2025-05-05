import 'package:flutter/material.dart';

TextStyle textWhite = TextStyle(color: Colors.white);
Color colorInput = Color.fromARGB(255, 72, 72, 72);
Color colorPrimary = Color.fromARGB(255, 32, 32, 32);
Color colorSecondary = Color.fromARGB(255, 16, 16, 16);

ThemeData themeData = ThemeData(
  scaffoldBackgroundColor: colorPrimary,
  textSelectionTheme: TextSelectionThemeData(
    cursorColor: Colors.white,
    selectionHandleColor: Colors.white,
  ),
  textTheme: TextTheme(
    headlineLarge: textWhite,
    bodyLarge: textWhite,
    bodyMedium: textWhite,
    titleMedium: textWhite,
    labelSmall: textWhite,
  ),
  inputDecorationTheme: InputDecorationTheme(
    labelStyle: TextStyle(color: Colors.white, fontSize: 16),
    counterStyle: TextStyle(color: Colors.white),
    focusedBorder: UnderlineInputBorder(
      borderSide: BorderSide(color: Colors.white),
    ),
  ),
  appBarTheme: AppBarTheme(
    backgroundColor: colorSecondary,
    surfaceTintColor: Colors.black,
    foregroundColor: Colors.white,
  ),
  elevatedButtonTheme: ElevatedButtonThemeData(
    style: ButtonStyle(
      backgroundColor: WidgetStateProperty.all<Color>(colorInput),
      foregroundColor: WidgetStateProperty.all<Color>(Colors.white),
    ),
  ),
  textButtonTheme: TextButtonThemeData(
    style: ButtonStyle(
      foregroundColor: WidgetStateProperty.all<Color>(Colors.white),
      textStyle: WidgetStateProperty.all<TextStyle>(
        TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
      ),
      overlayColor: WidgetStateProperty.all<Color>(Colors.white24),
    ),
  ),
  bottomNavigationBarTheme: BottomNavigationBarThemeData(
    backgroundColor: colorSecondary,
    unselectedItemColor: Colors.white70,
    selectedIconTheme: IconThemeData(size: 32),
    selectedItemColor: Colors.white,
    selectedLabelStyle: TextStyle(fontSize: 16),
  ),
  listTileTheme: ListTileThemeData(
    titleTextStyle: TextStyle(
      color: Colors.white,
      fontSize: 16,
      fontWeight: FontWeight.bold,
    ),
    subtitleTextStyle: textWhite,
  ),
  dialogTheme: DialogTheme(
    backgroundColor: colorSecondary,
    titleTextStyle: TextStyle(
      color: Colors.white,
      fontSize: 20,
      fontWeight: FontWeight.bold,
    ),
    contentTextStyle: TextStyle(color: Colors.white70, fontSize: 16),
    shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
  ),
);
