import { Pool } from 'pg'

const pool = new Pool()

export const query = async (text, params) => {
  const result = await pool.query(text, params)
  return result.rows
}

