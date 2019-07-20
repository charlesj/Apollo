import { query } from '../../../database/client'
import { ok } from '../../http'

export const healthCheck = async (req, res) => {
  const result = await query('select true as result')
  const health = {
    db: result[0].result,
  }
  ok(res, health)
}
