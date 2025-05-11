import supabase from "../Media_Tracker_Web/supabaseClient.js";

console.log("Script loaded");

let { data, error } = await supabase.rpc('testconnectionwitharguments', { app: 'Web' })
if (error) {
    console.error(error);
} else {
    console.log(data);
}

// Favorite button toggle
function setupFavoriteToggles(selector = ".favorite-icon") {
    document.querySelectorAll(selector).forEach(icon => {
        icon.addEventListener("click", async () => {
            icon.classList.toggle("favorited");
            icon.textContent = icon.classList.contains("favorited") ? "★" : "☆";

            const payload = {
                platform_id: icon.dataset.platform_id,
                media_type_id: icon.dataset.media_type_id,
                media_plat_id: icon.dataset.media_plat_id,
                title: icon.dataset.title,
                album: icon.dataset.album || "",
                artist: icon.dataset.artist || "",
                username: icon.dataset.username
            };

            try {
                const res = await fetch("../../php/database/initial_media_fav.php", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/x-www-form-urlencoded"
                    },
                    body: new URLSearchParams(payload)
                });

                const text = await res.text();
                console.log("RPC response:", text);
            } catch (err) {
                console.error("Favorite error:", err);
            }
        });
    });
}

setupFavoriteToggles();

// Wait for the DOM to finish rendering before querying it
await new Promise(resolve => {
    if (document.readyState === "complete" || document.readyState === "interactive") {
        resolve();
    } else {
        document.addEventListener("DOMContentLoaded", resolve);
    }
});

const username = document.querySelector(".favorite-icon")?.dataset.username;
if (username) {
    const favorites = await fetchFavoritesForUser(username);

    // Normalize favorites for quick matching
    const favoriteSet = new Set(
        favorites
            .filter(fav => fav.favorites === true || fav.favorites === "true" || fav.favorites === 1 || fav.favorites === "1")
            .map(fav => `${fav.platform_id}:${fav.media_plat_id.toLowerCase()}`)
    );

    // Match DOM icons
    document.querySelectorAll(".favorite-icon").forEach(icon => {
        const key = `${icon.dataset.platform_id}:${icon.dataset.media_plat_id.toLowerCase()}`;
        if (favoriteSet.has(key)) {
            icon.classList.add("favorited");
            icon.textContent = "★";
        }
    });
}

// Fetch user favorites when the page loads
async function fetchFavoritesForUser(username) {
    try {
        const res = await fetch(`/media-tracker/Web/Media_Tracker_Web/src/php/database/get_user_favorites.php?username=${encodeURIComponent(username)}`);
        if (!res.ok) throw new Error("Request failed");

        const favorites = await res.json();
        console.log("Favorites:", favorites);
        return favorites;
    } catch (err) {
        console.error("Error fetching favorites:", err);
        return [];
    }
}
