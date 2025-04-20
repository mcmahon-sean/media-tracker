<?php

class TMDBMovie {
    public int $id;
    public string $title;
    public string $overview;

    public function __construct(array $json) {
        $this->id = intval($json['id']);
        $this->title = $json['title'];
        $this->overview = $json['overview'];
    }
}
