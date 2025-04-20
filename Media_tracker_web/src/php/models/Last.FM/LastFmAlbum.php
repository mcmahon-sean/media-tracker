<?php

class LastFmAlbum {
    public string $name;
    public string $artist;
    public ?string $mbid;
    public string $url;
    public ?string $releaseDate;
    public int $playCount;
    public int $listeners;
    public ?string $imageUrl;
    public array $tags = [];
    public array $tracks = [];

    public function __construct(array $json) {
        $this->name = $json['name'] ?? '';
        $this->artist = $json['artist']['name'] ?? '';
        $this->mbid = $json['mbid'] ?? null;
        $this->url = $json['url'] ?? '';
        $this->releaseDate = trim($json['releasedate'] ?? '');
        $this->playCount = (int) ($json['playcount'] ?? 0);
        $this->listeners = (int) ($json['listeners'] ?? 0);

        // Grab largest image available
        $imageList = array_filter($json['image'] ?? [], fn($img) => !empty($img['#text']));
        $image = null;
        foreach ($imageList as $img) {
            if (($img['size'] ?? '') === 'large') {
                $image = $img;
                break;
            }
        }
        $image = $image ?: end($imageList) ?: ['#text' => null];
        $this->imageUrl = $image['#text'];

        // Tags
        if (!empty($json['toptags']['tag'])) {
            foreach ($json['toptags']['tag'] as $tag) {
                $this->tags[] = $tag['name'] ?? '';
            }
        }

        // Tracks
        if (!empty($json['tracks']['track'])) {
            foreach ($json['tracks']['track'] as $track) {
                $this->tracks[] = [
                    'name' => $track['name'] ?? '',
                    'duration' => (int) ($track['duration'] ?? 0),
                    'url' => $track['url'] ?? '',
                    'rank' => isset($track['@attr']['rank']) ? (int) $track['@attr']['rank'] : null,
                    'artist' => $track['artist']['name'] ?? $this->artist,
                ];
            }
        }
    }
}
