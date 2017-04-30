var store = require('store');

const tokenKey = "token";

class LoginService {
  isLoggedIn(){
    return (store.get(tokenKey) != null);
  }

  storeToken(token) {
    store.set(tokenKey, token);
  }

  logout(){
    store.remove(tokenKey);
  }
}


module.exports = LoginService;
