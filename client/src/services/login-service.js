import localStorage from 'store';

const tokenKey = "token";

class LoginService {
  isLoggedIn() {
    return (localStorage.get(tokenKey) != null);
  }

  getToken() {
    return localStorage.get(tokenKey);
  }

  storeToken(token) {
    localStorage.set(tokenKey, token);
  }

  logout() {
    localStorage.remove(tokenKey);
  }
}

module.exports = LoginService;
