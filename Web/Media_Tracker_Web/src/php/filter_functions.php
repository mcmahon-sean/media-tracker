<?php

    function revDir($dir): string {
        if ($dir == "asc") {
            return "desc";
        } else {
            return "asc";
        }
    }

    function sortLink($oldField, $oldDir, $field, $search = "" ): string {
        $dir = ($oldField == $field) ? revDir($oldDir) : "asc";
        if ($search != "") {
            return "href='?sort=$field". "_" ."$dir&$search'";
        }
        return "href='?sort=$field". "_" ."$dir'";
    }

    function sortBy($arr, $field, $dir = "asc") {
        // Exits early if the field is empty or the direction is neither "asc" nor "desc"
        if ($field == "" || ($dir != "asc" && $dir != "desc") || $arr == null) {
            return $arr;
        }

        usort($arr, function ($a, $b) use ($field) {
            switch ($field) {
                case "name":
                    return strcmp($a->name, $b->name);
                case "title":
                    return strcmp($a->title, $b->title);
                case "artist":
                    return strcmp($a->artist, $b->artist);
                case "date":
                    return $a->timestamp <=> $b->timestamp;
                case "playcount":
                    return $a->playCount <=> $b->playCount;
                case "playtime":
                    return $a->playtimeForever <=> $b->playtimeForever;
                case "id":
                    return $a->id <=> $b->id;
                case "userRating":
                    return $a->user_rating <=> $b->user_rating;
                case "avgRating":
                    return $a->average_rating <=> $b->average_rating;
                case "votes":
                    return $a->vote_count <=> $b->vote_count;
            }
        });

        if ($dir == "desc") {
            $sorted = array_reverse($arr);
        } else {
            $sorted = $arr;
        }

        return $sorted;
    }

    function filter($arr, $category, $substr, $movie = false) {
        // Exits early if the filter substring or category is empty
        if ($substr == "" || $category == "") {
            return $arr;
        }

        $filtered_arr = array();

        switch ($category) {
            case "name":
                foreach($arr as $x) {
                    // $title is assigned to $x->title property if it is a movie, otherwise it uses $x->name
                    $title = $movie ? strtolower(htmlspecialchars($x->title)) : strtolower(htmlspecialchars($x->name));

                    // Add the media to the filtered list if it matches the filter
                    if (str_contains($title, strtolower($substr))) {
                        $filtered_arr[] = $x;
                    }
                }
                break;
            case "artist":
                foreach($arr as $x) {
                    $artist = strtolower(htmlspecialchars($x->artist));

                    // Add the media to the filtered list if it matches the filter
                    if (str_contains($artist, strtolower($substr))) {
                        $filtered_arr[] = $x;
                    }
                }
                break;
            default:
                break;
        }

        return $filtered_arr;
    }


?>