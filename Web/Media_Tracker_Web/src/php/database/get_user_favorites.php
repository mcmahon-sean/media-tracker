<?php
require '../db.php';

header('Content-Type: application/json');

try {
    if (!isset($_GET['username'])) {
        http_response_code(400);
        echo json_encode(['error' => 'Missing username']);
        exit;
    }

    $username = trim($_GET['username']);

    // Query to join userfavorites and media table
    $stmt = $pdo->prepare("
        SELECT 
            uf.media_id,
            uf.favorites,
            m.platform_id,
            m.media_type_id,
            m.media_plat_id,
            m.title,
            m.album,
            m.artist
        FROM userfavorites uf
        INNER JOIN media m ON uf.media_id = m.media_id
        WHERE uf.username = ?
    ");
    $stmt->execute([$username]);
    $favorites = $stmt->fetchAll(PDO::FETCH_ASSOC);

    echo json_encode($favorites);
} catch (PDOException $e) {
    http_response_code(500);
    echo json_encode(['error' => $e->getMessage()]);
}
