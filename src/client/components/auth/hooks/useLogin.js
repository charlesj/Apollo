import { useState } from 'react'
import fetch from 'isomorphic-fetch'
import { useAuthenticatedUser } from '../../../data/auth/useAuthenticatedUser'
import { getApiHost } from '../../../data/server/getApiHost'

export const useLogin = () => {
  const user = useAuthenticatedUser()
  const [error, setError] = useState()
  const [isActive, setIsActive] = useState(false)

  const tryLogin = async (password) => {
    setIsActive(true)
    const response = await fetch(
      `${getApiHost()}/login`,
      {
        headers: { 'Content-Type': 'application/json' },
        method: 'POST',
        body: JSON.stringify({ password })
      }
    )
    if (response.ok) {
      const result = await response.json()
      user.login(result)
    } else {
      setIsActive(false)
      const result = await response.json()
      setError(result.errorMessage)
    }
  }

  return {
    tryLogin,
    error,
    isActive,
  }
}
