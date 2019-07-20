import { ok, badRequest } from '../../http'
import { getSetting } from '../../../database/settings/getSetting'
import { compare } from '../../../security/passwords'

export const postLogin = async (req, res) => {
  const { password } = req.body
  const configuredPasword = await getSetting('password')
  if (compare(password, configuredPasword.value)) {
    return ok(res, { token: 'i am token' })
  }
  return badRequest(res, 'Invalid Password')
}
