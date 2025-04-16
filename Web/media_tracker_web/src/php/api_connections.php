<?php
    class ApiServices {
        // STEAM CREDENTIALS
        const STEAM_API_KEY = '5553B2F6E49998D47EB298C086A05084';
        public static $steamUserId = '';

        // LAST.FM CREDENTIALS

        public static $LAST_FM_API_KEY = '1456c592b2d19dabd13468f0eee62dc9';
        public static $lastFmBaseUrl = 'https://ws.audioscrobbler.com/2.0/';
        // TMDB CREDENTIALS
        public static $tmdbUser = '';
        const TMDB_AUTH_TOKEN = 'eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJhOWNhY2ExNDI2MzI3YmQ5ZmE2MTI2NTRiNTk5NTIwNCIsIm5iZiI6MTczNzk5ODQ1OS40MDIsInN1YiI6IjY3OTdjMDdiMDljMjUyZTNhYjIzZGY2YyIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.zmcMBdlO7wllH_BvB1mZaqvydJC98yN7WuqhQePF004';

        // Steam Owned Games URL
        public static function getSteamOwnedGamesUrl() {
            return 'http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=' . self::STEAM_API_KEY . '&steamid=' . self::$steamUserId . '&include_appinfo=1&format=json';
        }

        // Last.fm Base URL
        public static function lastFmBaseUrl() {
            return 'https://ws.audioscrobbler.com/2.0/';
        }
    }
?>