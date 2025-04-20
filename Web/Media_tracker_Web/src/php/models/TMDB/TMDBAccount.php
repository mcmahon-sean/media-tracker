<?php

class TMDBAccount {
    public string $username;
    public int $id;

    public function __construct(array $json) {
        $this->username = $json['username'];
        $this->id = intval($json['id']);
    }
}
