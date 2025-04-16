<?php
    require "./db.php";

    $conn_string = "host=$host port=$port dbname=$dbname user=$user password=$password";

    $conn = pg_connect($conn_string);

    $query = pg_query($conn, "SELECT testconnection()");

    // DB function that doesnt accepts parameters
    if($query){
        $row = pg_fetch_assoc($query);
        echo $row["testconnection"];
        echo "<br>";
    }else {
        echo "Query failed: " . pg_last_error($conn);
    }


    // DB function that accepts parameters
    $params = ["app" => "Desktop"];
    $query2 = pg_query_params($conn, "SELECT testconnectionwitharguments($1)", $params);
    
    if($query2){
        $row = pg_fetch_assoc($query2);
        echo "<br>";
        echo  $row["testconnectionwitharguments"];
        
    }else {
        echo "Query failed: " . pg_last_error($conn);
    }
    // Close the database connection
    pg_close($conn);

?>