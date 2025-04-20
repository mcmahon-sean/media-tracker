import supabase from "../Media_Tracker_Web/supabaseClient.js";

let { data, error } = await supabase.rpc('testconnectionwitharguments', { app: 'Web' })
if (error) {
    console.error(error);
} else {
    console.log(data);
}