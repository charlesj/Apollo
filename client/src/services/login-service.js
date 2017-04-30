var store = require('store');

const tokenKey = "token";

class LoginService {
  isLoggedIn() {
    return (store.get(tokenKey) != null);
  }

  getToken() {
    return store.get(tokenKey);
  }

  storeToken(token) {
    store.set(tokenKey, token);
  }

  logout() {
    store.remove(tokenKey);
  }
}


module.exports = LoginService;
