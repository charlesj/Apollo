import localStorage from 'store'

const tokenKey = 'token'

function isLoggedIn() {
  return localStorage.get(tokenKey) != null
}

function getToken() {
  return localStorage.get(tokenKey)
}

function storeToken(token) {
  localStorage.set(tokenKey, token)
}

function logout() {
  localStorage.remove(tokenKey)
}

export default {
  isLoggedIn,
  getToken,
  storeToken,
  logout,
}
