@echo off
"C:\\Users\\mcse0\\AppData\\Local\\Android\\sdk\\cmake\\3.22.1\\bin\\cmake.exe" ^
  "-HC:\\Users\\mcse0\\Downloads\\flutter_windows_3.29.2-stable\\flutter\\packages\\flutter_tools\\gradle\\src\\main\\groovy" ^
  "-DCMAKE_SYSTEM_NAME=Android" ^
  "-DCMAKE_EXPORT_COMPILE_COMMANDS=ON" ^
  "-DCMAKE_SYSTEM_VERSION=21" ^
  "-DANDROID_PLATFORM=android-21" ^
  "-DANDROID_ABI=x86_64" ^
  "-DCMAKE_ANDROID_ARCH_ABI=x86_64" ^
  "-DANDROID_NDK=C:\\Users\\mcse0\\AppData\\Local\\Android\\sdk\\ndk\\26.3.11579264" ^
  "-DCMAKE_ANDROID_NDK=C:\\Users\\mcse0\\AppData\\Local\\Android\\sdk\\ndk\\26.3.11579264" ^
  "-DCMAKE_TOOLCHAIN_FILE=C:\\Users\\mcse0\\AppData\\Local\\Android\\sdk\\ndk\\26.3.11579264\\build\\cmake\\android.toolchain.cmake" ^
  "-DCMAKE_MAKE_PROGRAM=C:\\Users\\mcse0\\AppData\\Local\\Android\\sdk\\cmake\\3.22.1\\bin\\ninja.exe" ^
  "-DCMAKE_LIBRARY_OUTPUT_DIRECTORY=C:\\Users\\mcse0\\OneDrive\\Desktop\\School\\Spring 25\\Systems Analysis\\media-tracker-test\\media_tracker_test\\build\\app\\intermediates\\cxx\\Debug\\165q4o5o\\obj\\x86_64" ^
  "-DCMAKE_RUNTIME_OUTPUT_DIRECTORY=C:\\Users\\mcse0\\OneDrive\\Desktop\\School\\Spring 25\\Systems Analysis\\media-tracker-test\\media_tracker_test\\build\\app\\intermediates\\cxx\\Debug\\165q4o5o\\obj\\x86_64" ^
  "-DCMAKE_BUILD_TYPE=Debug" ^
  "-BC:\\Users\\mcse0\\OneDrive\\Desktop\\School\\Spring 25\\Systems Analysis\\media-tracker-test\\media_tracker_test\\android\\app\\.cxx\\Debug\\165q4o5o\\x86_64" ^
  -GNinja ^
  -Wno-dev ^
  --no-warn-unused-cli
