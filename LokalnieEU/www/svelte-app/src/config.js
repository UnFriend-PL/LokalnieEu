import Login from "./routes/Login.svelte";
import Register from "./routes/Register.svelte";
import NotFound from "./routes/NotFound.svelte";
import Index from "./routes/Index.svelte";
export const API_URL = "https://localhost:7265/api";

// export let routes = {
//     "/": Index,
//     "/login": Login,
//     "/register": Register,
//     "*": NotFound,
// };
// const checkPath = () => {
//     let isCorectPath = false;
//     routes.forEach(element => {
//         if (element === window.location.pathname) {
//             return true;
//         }
//     });
//     window.history.replaceState({}, "", "/#/");
// }