<?php

class SteamGame {
    public int $appId;
    public string $name;
    public int $playtimeForever;
    public string $imgIconUrl;
    public bool $hasCommunityStats;
    public int $playtimeWindows;
    public int $playtimeMac;
    public int $playtimeLinux;
    public int $playtimeDeck;
    public int $rtimeLastPlayed;
    public int $playtimeDisconnected;

    public function __construct(array $json) {
        $this->appId = intval($json['appid']);
        $this->name = $json['name'];
        $this->playtimeForever = intval($json['playtime_forever']);
        $this->imgIconUrl = $json['img_icon_url'];
        $this->hasCommunityStats = isset($json['has_community_visible_stats']) 
            ? (bool) $json['has_community_visible_stats'] 
            : false;
        $this->playtimeWindows = intval($json['playtime_windows_forever'] ?? 0);
        $this->playtimeMac = intval($json['playtime_mac_forever'] ?? 0);
        $this->playtimeLinux = intval($json['playtime_linux_forever'] ?? 0);
        $this->playtimeDeck = intval($json['playtime_deck_forever'] ?? 0);
        $this->rtimeLastPlayed = intval($json['rtime_last_played'] ?? 0);
        $this->playtimeDisconnected = intval($json['playtime_disconnected'] ?? 0);
    }
}
