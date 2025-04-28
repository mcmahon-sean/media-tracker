<?php

class TMDBRatedTvShow {
    public int $id;
    public string $name;
    public int $user_rating;
    public int $average_rating;
    public int $vote_count;
    public string $overview;

    public function __construct(array $json) {
        $this->id = intval($json['id']);
        $this->name = $json['name'];
        $this->user_rating = $json['rating'];
        $this->average_rating = $json['vote_average'];
        $this->vote_count = $json['vote_count'];
        $this->overview = $json['overview'];
    }
}
