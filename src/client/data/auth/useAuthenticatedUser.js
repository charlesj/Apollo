import { useData } from '../useData'
import { paths } from '../paths'

export const useAuthenticatedUser = () => {

  const [user, updateUser] = useData(paths.auth.user())

  const login = () => {
    updateUser('logged in')
  }

  return {
    user,
    login,
  }
}
