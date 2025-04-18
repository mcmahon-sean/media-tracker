<?php

class LastFmArtist {
    public string $name;
    public int $playCount;
    public string $url;
    public ?string $mbid;
    public int $rank;
    public ?string $imageUrl;

    public function __construct(array $json) {
        $imageList = array_filter($json['image'] ?? [], fn($img) => !empty($img['#text']));
        $image = end($imageList);
        $image = $image ?: ['#text' => null];
    
        $this->name = $json['name'];
        $this->playCount = intval($json['playcount'] ?? 0);
        $this->url = $json['url'];
        $this->mbid = $json['mbid'] ?? null;
        $this->rank = intval($json['@attr']['rank'] ?? 0);
        $this->imageUrl = $image['#text'];
    }
    
}
