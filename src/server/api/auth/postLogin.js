import { ok } from '../../http'
import { wait } from '../../../util/wait'

export const postLogin = async (req, res) => {
  const { password } = req.body
  await wait(1000)
  console.log('password', password)
  ok(res, { token: 'i am token' })
}
