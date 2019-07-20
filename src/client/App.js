import React from 'react'
import { useAuthenticatedUser } from './data/auth/useAuthenticatedUser'
import Login from './components/auth/Login'

const App = () => {
  const { user } = useAuthenticatedUser()
  if (!user) {
    return <Login />
  }

  return <div>Logged in</div>
}

export default App
