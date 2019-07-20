const getPaths = () => {
  const paths = {}
  paths.auth = () => ['auth']
  paths.auth.user = () => [...paths.auth(), 'user']

  return paths
}

export const paths = getPaths()
