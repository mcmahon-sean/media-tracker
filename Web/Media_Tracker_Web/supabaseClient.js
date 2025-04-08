import { createClient } from 'https://cdn.jsdelivr.net/npm/@supabase/supabase-js/+esm';

	const supabaseUrl = 'https://hrqakudeaalvgstpupdu.supabase.co';
	const supabaseKey = 	'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImhycWFrdWRlYWFsdmdzdHB1cGR1Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDI5NDk3MzAsImV4cCI6MjA1ODUyNTczMH0.k30q2Ndf-YI0RPGiwllMGJFPYMp5XoRQilCktlMmqFU';

	const supabase = createClient(supabaseUrl, supabaseKey);

	export default supabase;