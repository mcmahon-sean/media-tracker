<?php

class LastFmTrack {
    public string $name;
    public string $artist;
    public string $album;
    public ?string $playCount;
    public string $url;
    public ?int $timestamp;
    public ?string $mbid;
    public ?string $rank;
    public ?string $imageUrl;

    public function __construct(array $json, string $call) {
        $imageList = array_filter($json['image'] ?? [], fn($img) => !empty($img['#text']));
        $image = end($imageList);
        $image = $image ?: ['#text' => null];
    
        $this->name = $json['name'];
        if($call == 'recenttracks') {
            $this->artist = $json['artist']['#text'] ?? '';
        } else {
            $this->artist = $json['artist']['name'] ?? '';
        }
        $this->album = $json['album']['#text'] ?? '';
        $this->playCount = intval($json['playcount'] ?? 0);
        $this->url = $json['url'];
        $this->timestamp = isset($json['date']['uts']) ? intval($json['date']['uts']) : null;
        $this->mbid = $json['mbid'] ?? null;
        $this->rank = intval($json['@attr']['rank'] ?? 0);
        $this->imageUrl = $image['#text'];
    }
    

    public function getFormattedDate(): ?string {
        if (!$this->timestamp) return null;
        return date('Y-m-d H:i', $this->timestamp);
    }
}
