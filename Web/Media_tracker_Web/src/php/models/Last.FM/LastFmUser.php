<?php

class LastFmUser {
    public string $name;
    public int $age;
    public bool $isSubscriber;
    public string $realName;
    public int $playCount;
    public int $artistCount;
    public int $albumCount;
    public int $trackCount;
    public string $country;
    public string $gender;
    public string $profileUrl;
    public DateTime $registeredAt;
    public ?string $imageUrl;

    public function __construct(array $json) {
        $user = $json['user'];
        $imageList = $user['image'] ?? [];
        $image = end(array_filter($imageList, fn($img) => !empty($img['#text']))) ?: ['#text' => null];

        $this->name = $user['name'];
        $this->age = intval($user['age'] ?? 0);
        $this->isSubscriber = ($user['subscriber'] ?? '0') === '1';
        $this->realName = $user['realname'] ?? '';
        $this->playCount = intval($user['playcount'] ?? 0);
        $this->artistCount = intval($user['artist_count'] ?? 0);
        $this->albumCount = intval($user['album_count'] ?? 0);
        $this->trackCount = intval($user['track_count'] ?? 0);
        $this->country = $user['country'] ?? '';
        $this->gender = $user['gender'] ?? '';
        $this->profileUrl = $user['url'] ?? '';
        $this->registeredAt = new DateTime("@".intval($user['registered']['unixtime']));
        $this->imageUrl = $image['#text'];
    }
}
