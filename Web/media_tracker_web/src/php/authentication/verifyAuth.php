<?php
session_start();

$res = array();

if(isset($_SESSION['username'])){
    $res['auth'] = true;
    $res['username'] = $_SESSION['username'];
    $res['user_platform_ids']['steam'] = $_SESSION['user_platform_ids']['steam'];
    $res['user_platform_ids']['lastfm'] = $_SESSION['user_platform_ids']['lastfm'];
    $res['user_platform_ids']['tmdb'] = $_SESSION['user_platform_ids']['tmdb'];
}else{
    $res['auth'] = false;
}

echo json_encode($res);
?>