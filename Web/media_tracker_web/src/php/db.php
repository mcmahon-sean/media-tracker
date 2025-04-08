<?php
$user = "postgres.hrqakudeaalvgstpupdu";
$password = "MediaTrackerDatabase";
$host = "aws-0-us-east-2.pooler.supabase.com";
$port = 5432;
$dbname = "postgres";

try {
    $pdo = new PDO("pgsql:host=$host;port=$port;dbname=$dbname;user=$user;password=$password");
    $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

} catch (PDOException $e) {
    die("Connection failed: " . $e->getMessage());
}
?>
