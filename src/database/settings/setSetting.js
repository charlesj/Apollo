import { query } from '../client'

export const setSetting = async (key, value) => {
  try {
    await query('insert into settings (key, value) values ($1::text, $2::text)', [key, value])
  } catch (err) {
    console.log(err)
  }
}
