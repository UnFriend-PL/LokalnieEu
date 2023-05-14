const TOKEN_KEY = "jwtToken";

function saveToken(token) {
    localStorage.setItem(TOKEN_KEY, token);
    console.log(TOKEN_KEY, token)
}

function getToken() {
    return localStorage.getItem(TOKEN_KEY);
}

function removeToken() {
    localStorage.removeItem(TOKEN_KEY);
}

export default {
    saveToken,
    getToken,
    removeToken,
};
