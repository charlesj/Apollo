import { getConnection } from '../../../data/structure/connection'
import { ok } from '../../utils/http'
import { handleError } from '../../handleError'

export const healthCheck = handleError(async (req, res) => {
  const connection = await getConnection()
  const health = {
    db: !!connection,
  }
  ok(res, health)
})
