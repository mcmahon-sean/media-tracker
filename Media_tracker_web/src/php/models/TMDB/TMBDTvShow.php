<?php

class TMDBTvShow {
    public int $id;
    public string $name;
    public string $overview;

    public function __construct(array $json) {
        $this->id = intval($json['id']);
        $this->name = $json['name'];
        $this->overview = $json['overview'];
    }
}
