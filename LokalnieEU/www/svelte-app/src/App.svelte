<script>
	import { Router, Route } from "svelte-routing";
	import Login from "./routes/Login.svelte";
	import Register from "./routes/Register.svelte";
	import Index from "./routes/Index.svelte";
	import Profile from "./routes/Profile.svelte";
</script>

<Router>
	<Route path="/" component={Index} />
	<Route path="/login" component={Login} />
	<Route path="/register" component={Register} />
	<Route path="/profile" component={Profile} />
	<Route path="*" component={Index} />
</Router>
