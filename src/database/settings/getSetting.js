import { query } from '../client'

export const getSetting = async (key) => {
  const results = await query('select * from settings where key=$1::text', [key])
  if (results.length === 1) {
    return results[0]
  }
  if (results.length > 1) {
    throw new Error('Bad Setting Data in database')
  }

  return undefined
}
