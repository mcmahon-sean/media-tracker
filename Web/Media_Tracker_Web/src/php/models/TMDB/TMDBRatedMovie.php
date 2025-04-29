<?php

class TMDBRatedMovie {
    public int $id;
    public string $title;
    public int $user_rating;
    public int $average_rating;
    public int $vote_count;
    public string $overview;

    public function __construct(array $json) {
        $this->id = intval($json['id']);
        $this->title = $json['title'];
        $this->user_rating = $json['rating'];
        $this->average_rating = $json['vote_average'];
        $this->vote_count = $json['vote_count'];
        $this->overview = $json['overview'];
    }
}
