import React from 'react'
import { useAuthenticatedUser } from './data/auth/useAuthenticatedUser'

const App = () => {
  const { user, login } = useAuthenticatedUser()
  if (!user) {
    return <div>Not Logged In <button onClick={() => login()}>login</button></div>
  }
  console.log('user', user)
  return <div>Logged in</div>
}

export default App
