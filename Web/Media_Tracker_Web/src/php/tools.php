<?php
    // Validate helper for string values
    function sanitizeString($var) {
        return trim(strip_tags($var));
    }

    // Validate helper for numeric values
    function sanitizeInt($input) {
        return (int)filter_var($input, FILTER_VALIDATE_INT);
    }
?>